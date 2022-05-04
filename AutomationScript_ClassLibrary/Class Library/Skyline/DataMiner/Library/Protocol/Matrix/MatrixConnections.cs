namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents the matrix connections.
	/// </summary>
	internal class MatrixConnections
	{
		private readonly Dictionary<int, HashSet<int>> originalConnections;
		private readonly Dictionary<int, HashSet<int>> updatedConnections;

		internal MatrixConnections(string connectionBuffer)
		{
			originalConnections = new Dictionary<int, HashSet<int>>();
			updatedConnections = new Dictionary<int, HashSet<int>>();

			ReadConnections(connectionBuffer);
		}

		internal Dictionary<int, HashSet<int>> OriginalConnections
		{
			get { return originalConnections; }
		}

		internal Dictionary<int, HashSet<int>> UpdatedConnections
		{
			get { return updatedConnections; }
		}

		internal void ConnectSingleInputWithOutput(int inputIndex, int outputIndex)
		{
			HashSet<int> connections;
			HashSet<int> previousConnections;

			if (updatedConnections.TryGetValue(outputIndex, out connections))
			{
				if (originalConnections.TryGetValue(outputIndex, out previousConnections) && previousConnections.Count == 1 && previousConnections.Contains(inputIndex))
				{
					updatedConnections.Remove(outputIndex); // if value is again the original value, the set shouldn't occur
				}
				else
				{
					connections.Clear();
					connections.Add(inputIndex);
				}
			}
			else if (!originalConnections.TryGetValue(outputIndex, out previousConnections) || previousConnections.Count > 1 || !previousConnections.Contains(inputIndex))
			{
				updatedConnections[outputIndex] = new HashSet<int>(new[] { inputIndex });
			}
			else
			{
				// Do nothing
			}
		}

		// This connects without clearing other connections.
		internal void ConnectInputWithOutput(int inputIndex, int outputIndex)
		{
			HashSet<int> connections;

			if (updatedConnections.TryGetValue(outputIndex, out connections))
			{
				connections.Add(inputIndex);
			}
			else if (originalConnections.TryGetValue(outputIndex, out connections))
			{
				if (connections.Contains(inputIndex))
				{
					return;
				}

				HashSet<int> changed = new HashSet<int>();

				foreach (int number in connections)
				{
					if (number != -1)
					{
						changed.Add(number);
					}
				}

				changed.Add(inputIndex);
				updatedConnections[outputIndex] = changed;
			}
			else
			{
				updatedConnections[outputIndex] = new HashSet<int>(new[] { inputIndex });
			}
		}

		internal void DisconnectInputFromOutput(int inputIndex, int outputIndex)
		{
			HashSet<int> connections;

			if (updatedConnections.TryGetValue(outputIndex, out connections) && connections.Contains(inputIndex))
			{
				connections.Remove(inputIndex);
				if (connections.Count == 0)
				{
					connections.Add(-1);
				}
			}
			else if (originalConnections.TryGetValue(outputIndex, out connections) && connections.Contains(inputIndex))
			{
				HashSet<int> changed = new HashSet<int>();
				foreach (int number in connections)
				{
					if (number != -1 && number != inputIndex)
					{
						changed.Add(number);
					}
				}

				if (changed.Count == 0)
				{
					changed.Add(-1);
				}

				updatedConnections[outputIndex] = changed;
			}
			else
			{
				// Do Nothing
			}
		}

		internal List<int> GetConnectedInputs(int outputIndex)
		{
			HashSet<int> connectedInputs;
			return updatedConnections.TryGetValue(outputIndex, out connectedInputs) || originalConnections.TryGetValue(outputIndex, out connectedInputs)
				? new List<int>(connectedInputs)
				: new List<int>();
		}

		internal void DisconnectAllInputs(int outputIndex)
		{
			updatedConnections[outputIndex] = new HashSet<int>(new[] { -1 });
		}

		private void ReadConnections(string connectionBuffer)
		{
			if (String.IsNullOrEmpty(connectionBuffer))
			{
				return;
			}

			string[] bufferItems = connectionBuffer.Split('|');
			if (bufferItems.Length == 0)
			{
				return;
			}

			int outputNumber = 0;

			foreach (string inputItems in bufferItems[0].Split(new[] { ';' }, StringSplitOptions.None))
			{
				HashSet<int> connections = new HashSet<int>();

				foreach (string inputItem in inputItems.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
				{
					int inputNumber;

					if (Int32.TryParse(inputItem, out inputNumber))
					{
						connections.Add(inputNumber);
					}
				}

				if (connections.Count == 0)
				{
					connections.Add(-1);
				}

				originalConnections[outputNumber] = connections;
				outputNumber++;
			}
		}
	}
}

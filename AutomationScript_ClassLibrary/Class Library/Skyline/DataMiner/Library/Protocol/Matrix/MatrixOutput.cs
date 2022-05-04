﻿namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// Represents a matrix output.
	/// </summary>
	public class MatrixOutput : MatrixPort
	{
		internal MatrixOutput(MatrixPortState portState, int index) : base(index)
		{
			PortState = portState;
		}

		/// <summary>
		/// Gets the zero-based indexes of the inputs that are connected with this matrix output.
		/// </summary>
		/// <returns>The zero-based indexes of the inputs that are connected with this matrix output.</returns>
		public IList<int> ConnectedInputs
		{
			get
			{
				var list = PortState.Connections.GetConnectedInputs(Index);
				list.Sort();

				return list;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this matrix output is enabled.
		/// </summary>
		/// <value><c>true</c> if the matrix output is enabled; otherwise, <c>false</c>.</value>
		public override bool IsEnabled
		{
			get
			{
				return PortState.OutputStates[Index];
			}

			set
			{
				PortState.OutputStates[Index] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this matrix output is locked.
		/// </summary>
		/// <value><c>true</c> if the matrix output is locked; otherwise, <c>false</c>.</value>
		public override bool IsLocked
		{
			get
			{
				return PortState.OutputLocks[Index];
			}

			set
			{
				PortState.OutputLocks[Index] = value;
			}
		}

		/// <summary>
		/// Gets or sets the label of this matrix output.
		/// </summary>
		/// <value>The label of this matrix output.</value>
		public override string Label
		{
			get
			{
				return PortState.OutputLabels[Index];
			}

			set
			{
				PortState.OutputLabels[Index] = value;
			}
		}

		/// <summary>
		/// Connects this output with the specified input.
		/// </summary>
		/// <param name="inputIndex">The zero-based index of the input to connect with this output.</param>
		/// <param name="editMode">Indicates whether the current connected inputs should be disconnected from this output or not.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="inputIndex"/> is not in the range [0, MaxInputs -1].
		/// -or-
		/// When tables are supported, there can be maximum one input connected to an output.
		/// </exception>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="editMode"/> is not a member of <see cref="MatrixEditMode"/>.</exception>
		public void Connect(int inputIndex, MatrixEditMode editMode = MatrixEditMode.Replace)
		{
			if (inputIndex < 0 || inputIndex >= PortState.MaxInputs)
			{
				throw new ArgumentOutOfRangeException("The specified input index must be in the range [0, " + (PortState.MaxInputs - 1) + "].");
			}

			switch (editMode)
			{
				case MatrixEditMode.Replace:
					PortState.Connections.ConnectSingleInputWithOutput(inputIndex, Index);

					break;
				case MatrixEditMode.Add:
					if (PortState.IsTableCapable)
					{
						throw new InvalidOperationException("When tables are supported, there can be maximum one input connected to an output.");
					}

					PortState.Connections.ConnectInputWithOutput(inputIndex, Index);
					break;
				default:
					throw new InvalidEnumArgumentException("The specified connect mode is not a member of MatrixEditMode.");
			}
		}

		/// <summary>
		/// Connects this output with the specified inputs.
		/// </summary>
		/// <param name="inputIndexes">The zero-based indexes of the inputs to connect with this output.</param>
		/// <param name="editMode">Indicates whether the current connected inputs should be disconnected from this output or not.</param>
		/// <exception cref="ArgumentNullException"><paramref name="inputIndexes"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">At least one of the indexes provided in <paramref name="inputIndexes"/> is not in the range [0, MaxInputs -1].
		/// -or-
		/// When tables are supported, there can be maximum one input connected to an output.
		/// </exception>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="editMode"/> is not a member of <see cref="MatrixEditMode"/>.</exception>
		public void Connect(IEnumerable<int> inputIndexes, MatrixEditMode editMode = MatrixEditMode.Replace)
		{
			if (inputIndexes == null)
			{
				throw new ArgumentNullException("inputIndexes");
			}

			if (editMode != MatrixEditMode.Add && editMode != MatrixEditMode.Replace)
			{
				throw new InvalidEnumArgumentException("The specified connect mode is not a member of MatrixEditMode.");
			}

			int inputCount = 0;
			foreach (int inputIndex in inputIndexes)
			{
				inputCount++;
				if (inputIndex < 0 || inputIndex >= PortState.MaxInputs)
				{
					throw new ArgumentOutOfRangeException("The specified input indexes must be in the range [0, " + (PortState.MaxInputs - 1) + "].");
				}
			}

			if (inputCount == 0)
			{
				if (editMode == MatrixEditMode.Replace)
				{
					DisconnectAll();
				}

				return;
			}

			if (PortState.IsTableCapable && inputCount > 1)
			{
				throw new InvalidOperationException("When tables are supported, there can be maximum one input connected to an output.");
			}

			bool isFirstCheck = editMode == MatrixEditMode.Replace;
			foreach (int inputIndex in inputIndexes)
			{
				Connect(inputIndex, isFirstCheck ? MatrixEditMode.Replace : MatrixEditMode.Add);
				isFirstCheck = false;
			}
		}

		/// <summary>
		/// Disconnects the specified input from this matrix output.
		/// </summary>
		/// <param name="inputIndex">The zero-based index of the matrix input.</param>
		/// <exception cref="ArgumentOutOfRangeException">The specified input index is not in the range [0, MaxInputs-1].</exception>
		public void Disconnect(int inputIndex)
		{
			if (inputIndex < 0 || inputIndex >= PortState.MaxInputs)
			{
				throw new ArgumentOutOfRangeException("The specified input index must be in the range [0, " + (PortState.MaxInputs - 1) + "].");
			}

			PortState.Connections.DisconnectInputFromOutput(inputIndex, Index);
		}

		/// <summary>
		/// Disconnects all connected inputs from this matrix output.
		/// </summary>
		public void DisconnectAll()
		{
			PortState.Connections.DisconnectAllInputs(Index);
		}

		/// <summary>
		/// Retrieves a value indicating whether this matrix output is connected with the specified matrix input.
		/// </summary>
		/// <param name="inputIndex">The zero-based index of the matrix input.</param>
		/// <exception cref="ArgumentOutOfRangeException">The specified input index is not in the range [0, MaxInputs-1].</exception>
		/// <returns><c>true</c> if the specified input is connected with this matrix output; otherwise, <c>false</c>.</returns>
		public bool IsConnectedToInput(int inputIndex)
		{
			if (inputIndex < 0 || inputIndex >= PortState.MaxInputs)
			{
				throw new ArgumentOutOfRangeException("The specified input index must be in the range [0, " + (PortState.MaxInputs - 1) + "].");
			}

			return ConnectedInputs.Contains(inputIndex);
		}
	}
}

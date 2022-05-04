namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.IO;
	using System.Security.AccessControl;
	using System.Security.Principal;
	using System.Text;
	using System.Threading;

	internal static class Logger
	{
		private const long SizeLimit = 3 * 1024 * 1024;
		private const string LogFileName = @"C:\Skyline DataMiner\logging\ClassLibrary.txt";
		private const string LogPositionPlaceholder = "**********";
		private const int PlaceHolderSize = 10;
		private static long logPositionPlaceholderStart = -1;

		private static Mutex loggerMutex;

#pragma warning disable S3963 // "static" fields should be initialized inline
		static Logger()
		{
			MutexSecurity mutexSecurity = new MutexSecurity();
			var accessRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow);
			mutexSecurity.AddAccessRule(accessRule);

			bool createdNew;

			loggerMutex = new Mutex(false, "clpMutex", out createdNew, mutexSecurity);
		}
#pragma warning restore S3963 // "static" fields should be initialized inline

		public static void Log(string message)
		{
			try
			{
				loggerMutex.WaitOne();

				string logPrefix = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "|";

				long messageByteCount = Encoding.UTF8.GetByteCount(message);

				// Safeguard for large messages.
				if (messageByteCount > SizeLimit)
				{
					message = "WARNING: message \"" + message.Substring(0, 100) + " not logged as it is too large (over " + SizeLimit + " bytes).";
				}

				long limit = SizeLimit / 2; // Safeguard: limit messages. If safeguard removed, the limit would be: SizeLimit - placeholder size - prefix length - 4 (2 * CR LF).

				if (messageByteCount > limit)
				{
					long overhead = messageByteCount - limit;
					int partToRemove = (int)overhead / 4; // In worst case, each char takes 4 bytes.

					if (partToRemove == 0)
					{
						partToRemove = 1;
					}

					while (messageByteCount > limit)
					{
						message = message.Substring(0, message.Length - partToRemove);
						messageByteCount = Encoding.UTF8.GetByteCount(message);
					}
				}

				int byteCount = Encoding.UTF8.GetByteCount(message);
				long positionOfPlaceHolder = GetPlaceHolderPosition();

				Stream fileStream = null;
				try
				{
					fileStream = new FileStream(LogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
					using (StreamWriter sw = new StreamWriter(fileStream))
					{
						fileStream = null;

						if (positionOfPlaceHolder == -1)
						{
							sw.BaseStream.Position = 0;

							sw.Write(logPrefix);
							sw.WriteLine(message);
							logPositionPlaceholderStart = byteCount + logPrefix.Length;
							sw.WriteLine(LogPositionPlaceholder);
						}
						else
						{
							sw.BaseStream.Position = positionOfPlaceHolder;

							if (positionOfPlaceHolder + byteCount + 4 + PlaceHolderSize > SizeLimit)
							{
								// Overwrite previous placeholder.
								byte[] placeholder = Encoding.UTF8.GetBytes("          ");

								sw.BaseStream.Write(placeholder, 0, placeholder.Length);
								sw.BaseStream.Position = 0;
							}

							sw.Write(logPrefix);
							sw.WriteLine(message);
							sw.Flush();
							logPositionPlaceholderStart = sw.BaseStream.Position;
							sw.WriteLine(LogPositionPlaceholder);
						}
					}
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Dispose();
					}
				}
			}
			catch
			{
				// Do nothing.
			}
			finally
			{
				loggerMutex.ReleaseMutex();
			}
		}

		private static long SetToStartOfLine(StreamReader streamReader, long startPosition)
		{
			Stream stream = streamReader.BaseStream;

			for(long position = startPosition - 1; position > 0; position--)
			{
				stream.Position = position;
				if(stream.ReadByte() == '\n')
				{
					return position + 1;
				}
			}

			return 0;
		}

		private static long GetPlaceHolderPosition()
		{
			long result = -1;

			Stream fileStream = null;

			try
			{
				fileStream = File.Open(LogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					fileStream = null;

					streamReader.DiscardBufferedData();

					long startOfLinePosition = SetToStartOfLine(streamReader, logPositionPlaceholderStart);

					streamReader.DiscardBufferedData();
					streamReader.BaseStream.Position = startOfLinePosition;
					string line;

					long postionInFile = startOfLinePosition;

					while ((line = streamReader.ReadLine()) != null)
					{
						if (line == LogPositionPlaceholder)
						{
							streamReader.DiscardBufferedData();
							result = postionInFile;
							break;
						}
						else
						{
							postionInFile = postionInFile + Encoding.UTF8.GetByteCount(line) + 2;
						}
					}

					// If this point is reached, it means the placeholder was still not found.
					if (result == -1 && startOfLinePosition > 0)
					{
						streamReader.DiscardBufferedData();
						streamReader.BaseStream.Position = 0;

						while ((line = streamReader.ReadLine()) != null)
						{
							if (line == LogPositionPlaceholder)
							{
								streamReader.DiscardBufferedData();
								result = streamReader.BaseStream.Position - PlaceHolderSize - 2;
								break;
							}
						}
					}
				}
			}
			finally
			{
				if(fileStream != null)
				{
					fileStream.Dispose();
				}
			}

			return result;
		}
	}
}

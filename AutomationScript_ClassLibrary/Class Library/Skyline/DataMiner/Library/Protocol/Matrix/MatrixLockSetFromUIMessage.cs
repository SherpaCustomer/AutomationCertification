﻿namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;

	/// <summary>
	/// Provides data for the event that is raised when a lock on a matrix input or output has changed.
	/// </summary>
	[Serializable]
	public class MatrixLockSetFromUIMessage
	{
		private readonly int index;
		private readonly MatrixIOType type;
		private readonly bool isLocked;

		internal MatrixLockSetFromUIMessage(int index, MatrixIOType type, bool isLocked)
		{
			this.index = index;
			this.type = type;
			this.isLocked = isLocked;
		}

		/// <summary>
		/// Gets the zero-based port number of the changed lock.
		/// </summary>
		/// <value>Zero-based port number.</value>
		public int Index
		{
			get { return index; }
		}

		/// <summary>
		/// Gets the <see cref="MatrixIOType"/> to determine if the changed lock is on an output or input port.
		/// </summary>
		/// <value><see cref="MatrixIOType"/> that determines if it is an output or input port.</value>
		public MatrixIOType Type
		{
			get { return type; }
		}

		/// <summary>
		/// Gets a value indicating whether this port is locked.
		/// </summary>
		/// <value><c>true</c> if the port is locked; otherwise, <c>false</c>.</value>
		public bool IsLocked
		{
			get { return isLocked; }
		}
	}
}

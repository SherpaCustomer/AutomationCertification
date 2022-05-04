﻿namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// This exception is used to indicate that two widgets have overlapping positions on the same dialog.
	/// </summary>
	[Serializable]
	public class OverlappingWidgetsException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class.
		/// </summary>
		public OverlappingWidgetsException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public OverlappingWidgetsException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public OverlappingWidgetsException(string message, Exception inner) : base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the OverlappingWidgetException class with the serialized data.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
		protected OverlappingWidgetsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}

using System;
using Skyline.DataMiner.Net.Messages;

namespace Skyline.DataMiner.Library.Common
{
	using System.Linq;

	/// <summary>
	/// Class representing any non-virtual connection.
	/// </summary>
	public class RealConnection : ConnectionSettings, IRealConnection
	{
		private readonly int id;
		private TimeSpan timeout;
		private int retries;
		private TimeSpan? elementTimeout;

		/// <summary>
		/// Initiates a new RealConnection class.
		/// </summary>
		/// <param name="info"></param>
		internal RealConnection(ElementPortInfo info)
		{
			this.id = info.PortID;
			this.retries = info.Retries;
			this.timeout = new TimeSpan(0, 0, 0, 0, info.TimeoutTime);
		}

		/// <summary>
		/// Default empty constructor.
		/// </summary>
		public RealConnection()
		{

		}

		/// <summary>
		/// Gets the zero based id of the connection.
		/// </summary>
		public int Id
		{
			get { return this.id; }
		}

		/// <summary>
		/// Get or Set the timeout value.
		/// </summary>
		public TimeSpan Timeout
		{
			get { return timeout; }
			set {

				if(value.TotalMilliseconds>=10 && value.TotalMilliseconds <= 120000)
				{
					timeout = value;
				}
				else
				{
					throw new IncorrectDataException("Timeout value should be between 10 and 120 s.");
				}
			}
		}

		/// <summary>
		/// Get or Set the amount of retries.
		/// </summary>
		public int Retries
		{
			get { return retries; }
			set {

				if(value>=0 && value<=10){
					retries = value;
				}
				else
				{
					throw new IncorrectDataException("Retries value should be between 0 and 10.");
				}
				
			}
		}

		/// <summary>
		/// Get or Set the timespan before the element will go into timeout after not responding.
		/// </summary>
		/// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
		public TimeSpan? ElementTimeout
		{
			get { return elementTimeout; }
			set {
				if(value==null || (value.Value.TotalSeconds >= 1 && value.Value.TotalSeconds <= 120))
				{
					elementTimeout = value;
				}
				else
				{
					throw new IncorrectDataException("ElementTimeout value should be between 1 and 120 sec.");
				}
			}
		}

		/// <summary>
		/// Returns whether a property has been set or not.
		/// </summary>
		internal override bool IsUpdated
		{
			get
			{
				return (ChangedPropertyList.Any());
			}
		}

		/// <summary>
		/// Creates an ElementPortInfo object based on the properties.
		/// </summary>
		/// <returns></returns>
		internal override ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
		{
			throw new NotSupportedException("RealConnection is not supported.");
		}

		/// <summary>
		/// Updates an ElementPortInfo object based on changes on properties.
		/// </summary>
		/// <param name="portInfo"></param>
		/// <param name="isCompatibilityIssueDetected"></param>
		internal override void UpdateElementPortInfo(ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
		{
			// throw new NotSupportedException("RealConnection is not supported.");
		}

		/// <summary>
		/// Clear the performed update flags of the properties of the object.
		/// </summary>
		internal override void ClearUpdates()
		{
			this.ChangedPropertyList.Clear();
		}
	}
}
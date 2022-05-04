namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents the advanced element information.
	/// </summary>
	internal class AdvancedSettings : ElementSettings, IAdvancedSettings
	{
		/// <summary>
		/// Value indicating whether the element is hidden.
		/// </summary>
		private bool isHidden;

		/// <summary>
		/// Value indicating whether the element is read-only.
		/// </summary>
		private bool isReadOnly;

		/// <summary>
		/// Indicates whether this is a simulated element.
		/// </summary>
		private bool isSimulation;

		/// <summary>
		/// The element timeout value.
		/// </summary>
		private TimeSpan timeout = new TimeSpan(0, 0, 30);

		/// <summary>
		/// Initializes a new instance of the <see cref="AdvancedSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the <see cref="DmsElement"/> instance this object is part of.</param>
		internal AdvancedSettings(DmsElement dmsElement)
		: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether the element is hidden.
		/// </summary>
		/// <value><c>true</c> if the element is hidden; otherwise, <c>false</c>.</value>
		/// <exception cref="NotSupportedException">A set operation is not supported on a derived element.</exception>
		public bool IsHidden
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isHidden;
			}

			set
			{
				DmsElement.LoadOnDemand();

				if (DmsElement.RedundancySettings.IsDerived)
				{
					throw new NotSupportedException("This operation is not supported on a derived element.");
				}

				if (isHidden != value)
				{
					ChangedPropertyList.Add("IsHidden");
					isHidden = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the element is read-only.
		/// </summary>
		/// <value><c>true</c> if the element is read-only; otherwise, <c>false</c>.</value>
		/// <exception cref="NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
		public bool IsReadOnly
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isReadOnly;
			}

			set
			{
				if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
				{
					throw new NotSupportedException("This operation is not supported on a DVE child or derived element.");
				}

				DmsElement.LoadOnDemand();

				if (isReadOnly != value)
				{
					ChangedPropertyList.Add("IsReadOnly");
					isReadOnly = value;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the element is running a simulation.
		/// </summary>
		/// <value><c>true</c> if the element is running a simulation; otherwise, <c>false</c>.</value>
		public bool IsSimulation
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isSimulation;
			}
		}

		/// <summary>
		/// Gets or sets the element timeout value.
		/// </summary>
		/// <value>The timeout value.</value>
		/// <exception cref="ArgumentOutOfRangeException">The value specified for a set operation is not in the range of [0,120] s.</exception>
		/// <exception cref="NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
		/// <remarks>Fractional seconds are ignored. For example, setting the timeout to a value of 3.5s results in setting it to 3s.</remarks>
		public TimeSpan Timeout
		{
			get
			{
				DmsElement.LoadOnDemand();
				return timeout;
			}

			set
			{
				if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
				{
					throw new NotSupportedException("Setting the timeout is not supported on a DVE child or derived element.");
				}

				DmsElement.LoadOnDemand();
				int timeoutInSeconds = (int)value.TotalSeconds;

				if (timeoutInSeconds < 0 || timeoutInSeconds > 120)
				{
					throw new ArgumentOutOfRangeException("value", "The timeout value must be in the range of [0,120] s.");
				}

				if ((int)timeout.TotalSeconds != (int)value.TotalSeconds)
				{
					ChangedPropertyList.Add("Timeout");
					timeout = value;
				}
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ADVANCED SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendFormat(CultureInfo.InvariantCulture, "Timeout: {0}{1}", Timeout, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Hidden: {0}{1}", IsHidden, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Simulation: {0}{1}", IsSimulation, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Read-only: {0}{1}", IsReadOnly, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			timeout = new TimeSpan(0, 0, 0, 0, elementInfo.ElementTimeoutTime);
			isHidden = elementInfo.Hidden;
			isReadOnly = elementInfo.IsReadOnly;
			isSimulation = elementInfo.IsSimulated;
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message which will be sent to SLNet.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
			foreach (string property in ChangedPropertyList)
			{
				switch (property)
				{
					case "IsHidden":
						message.IsHidden = isHidden;
						break;

					case "IsReadOnly":
						message.IsReadOnly = isReadOnly;
						break;

					case "Timeout":
						message.TimeoutTime = (int)timeout.TotalMilliseconds;
						break;

					default:
						continue;
				}
			}
		}
	}
}
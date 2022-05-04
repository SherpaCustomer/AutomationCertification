namespace Skyline.DataMiner.Library.Common
{
	using Net.Exceptions;
	using Net.Messages;

	using Skyline.DataMiner.Library.Common.Subscription.Waiters;
	using Skyline.DataMiner.Library.Common.Subscription.Waiters.Parameter;

	using System;
	using System.Globalization;
	using System.Linq;

	/// <summary>
	/// Represents a standalone parameter.
	/// </summary>
	/// <typeparam name="T">The type of the standalone parameter.</typeparam>
	/// <remarks>
	/// In case T equals int?, double? or DateTime?, extension methods are available. Refer to <see
	/// cref="ExtensionsIDmsStandaloneParameter"/> for more information.
	/// </remarks>
	internal class DmsStandaloneParameter<T> : DmsParameter<T>, IDmsStandaloneParameter<T>
	{
		/// <summary>
		/// The element this parameter is part of.
		/// </summary>
		private readonly IDmsElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsStandaloneParameter{T}"/> class.
		/// </summary>
		/// <param name="element">The element that the parameter belongs to.</param>
		/// <param name="id">The ID of the parameter.</param>
		/// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="id"/> is invalid.</exception>
		internal DmsStandaloneParameter(IDmsElement element, int id) : base(id)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			this.element = element;
		}

		/// <summary>
		/// Gets the element this parameter is part of.
		/// </summary>
		/// <value>The element this parameter is part of.</value>
		public IDmsElement Element
		{
			get { return element; }
		}

		/// <summary>
		/// Retrieves the alarm level.
		/// </summary>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The alarm level.</returns>
		public AlarmLevel GetAlarmLevel()
		{
			var response = SendGetParameterMessage();

			return (AlarmLevel)response.AlarmLevel;
		}

		/// <summary>
		/// Gets the displayed value.
		/// </summary>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The displayed value.</returns>
		public string GetDisplayValue()
		{
			var response = SendGetParameterMessage();

			return response.DisplayValue;
		}

		/// <summary>
		/// Gets the parameter value.
		/// </summary>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The parameter value.</returns>
		public T GetValue()
		{
			var response = SendGetParameterMessage();

			T value = ProcessResponse(response);

			return value;
		}

		/// <summary>
		/// Sets the value of this parameter.
		/// </summary>
		/// <param name="value">The value to set.</param>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		public void SetValue(T value)
		{
			HelperClass.CheckElementState(element);

			SetParameterMessage message = new SetParameterMessage
			{
				DataMinerID = element.DmsElementId.AgentId,
				ElId = element.DmsElementId.ElementId,
				ParameterId = Id,
				DisableInformationEventMessage = true
			};

			if (AddValueToSetParameterMessage(message, value))
			{
				element.Host.Dms.Communication.SendMessage(message);
			}
		}

		/// <summary>
		/// Sets the value of this parameter. Waits on specified expected changes.
		/// </summary>
		/// <param name="value">The value to set.</param>
		/// <param name="timeout">Maximum time to wait for each expected change.</param>
		/// <param name="expectedChanges">One or more expected changes.</param>
		/// <returns>Every expected change as it happens on the system.</returns>
		/// <exception cref="ElementNotFoundException">The element was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="TimeoutException">Expected change took too long.</exception>
		/// <exception cref="FormatException">One of the provided parameters is missing data.</exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="expectedChanges"/> or <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public void SetValue(T value, TimeSpan timeout, ExpectedChanges expectedChanges)
		{
			if (expectedChanges == null)
			{
				throw new ArgumentNullException("expectedChanges");
			}

			if (expectedChanges.ExpectedParamChanges != null)
			{
				using (ParamWaiter waiter = new ParamWaiter(element.Host.Dms.Communication, expectedChanges.ExpectedParamChanges))
				{
					SetValue(value);
					int waitCount = waiter.WaitNext(timeout).Count();
					System.Diagnostics.Debug.WriteLine("WaitNext: " + waitCount);
				}
			}

			if (expectedChanges.ExpectedCellChanges != null)
			{
				using (CellWaiter waiter = new CellWaiter(element.Host.Dms.Communication, expectedChanges.ExpectedCellChanges))
				{
					SetValue(value);
					int waitCount = waiter.WaitNext(timeout).Count();
					System.Diagnostics.Debug.WriteLine("WaitNext: " + waitCount);
				}
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Standalone Parameter:{0}", Id);
		}

		/// <summary>
		/// Sends a <see cref="GetParameterMessage"/> SLNet message.
		/// </summary>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The response message.</returns>
		private GetParameterResponseMessage SendGetParameterMessage()
		{
			HelperClass.CheckElementState(element);

			try
			{
				var message = new GetParameterMessage
				{
					DataMinerID = element.DmsElementId.AgentId,
					ElId = element.DmsElementId.ElementId,
					ParameterId = Id,
				};

				var response = (GetParameterResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

				return response;
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147220935)
				{
					// 0x80040239, SL_FAILED_NOT_FOUND, The object or file was not found.
					throw new ParameterNotFoundException(Id, element.DmsElementId, e);
				}
				else if (e.ErrorCode == -2147024891 && e.Message == "No such element.")
				{
					// 0x80070005: Access is denied.
					throw new ElementNotFoundException(element.DmsElementId, e);
				}
				else if (e.ErrorCode == -2147220916)
				{
					// 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
					throw new ElementNotFoundException(element.DmsElementId, e);
				}
				else
				{
					throw;
				}
			}
		}
	}
}
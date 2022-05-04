namespace Skyline.DataMiner.Library.Common
{
	using Net.Exceptions;
	using Net.Messages;
	using Net.Messages.Advanced;

	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Subscription.Waiters;
	using Skyline.DataMiner.Library.Common.Subscription.Waiters.Parameter;

	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Represents a table column.
	/// </summary>
	/// <typeparam name="T">The type of the values this column holds.</typeparam>
	internal class DmsColumn<T> : DmsParameter<T>, IDmsColumn<T>
	{
		/// <summary>
		/// The table this column belongs to.
		/// </summary>
		private readonly IDmsTable table;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsColumn{T}"/> class.
		/// </summary>
		/// <param name="table">The table this column belongs to.</param>
		/// <param name="id">The column parameter ID.</param>
		internal DmsColumn(IDmsTable table, int id) : base(id)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}

			this.table = table;
		}

		/// <summary>
		/// Gets the table this column is part of.
		/// </summary>
		/// <value>The table this column is part of.</value>
		public IDmsTable Table
		{
			get { return table; }
		}

		/// <summary>
		/// Retrieves the alarm level.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="primaryKey"/> is empty ("") or white space.
		/// </exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <returns>The alarm level.</returns>
		public AlarmLevel GetAlarmLevel(string primaryKey)
		{
			if (primaryKey == null)
			{
				throw new ArgumentNullException("primaryKey");
			}

			if (String.IsNullOrWhiteSpace(primaryKey))
			{
				throw new ArgumentException("The primary key must not be the empty string (\"\") or white space.", "primaryKey");
			}

			var response = SendGetParameterMessage(primaryKey);

			return (AlarmLevel)response.AlarmLevel;
		}

		/// <summary>
		/// Gets the displayed value.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="primaryKey"/> is empty ("") or white space.
		/// </exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <returns>The displayed value.</returns>
		/// <remarks>Typically used for parameters that provide a discrete entry mapping.</remarks>
		public string GetDisplayValue(string primaryKey)
		{
			if (primaryKey == null)
			{
				throw new ArgumentNullException("primaryKey");
			}

			if (String.IsNullOrWhiteSpace(primaryKey))
			{
				throw new ArgumentException("The primary key must not be the empty string (\"\") or white space.", "primaryKey");
			}

			var response = SendGetParameterMessage(primaryKey);

			return response.DisplayValue;
		}

		/// <summary>
		/// Gets the value of the cell that corresponds with the specified primary key.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="primaryKey"/> is empty ("") or white space.
		/// </exception>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The cell value.</returns>
		public T GetValue(string primaryKey)
		{
			if (primaryKey == null)
			{
				throw new ArgumentNullException("primaryKey");
			}

			if (String.IsNullOrWhiteSpace(primaryKey))
			{
				throw new ArgumentException("The primary key must not be the empty string (\"\") or white space.", "primaryKey");
			}

			IDmsElement element = table.Element;
			var response = SendGetParameterMessage(primaryKey);

			T value = ProcessResponse(response);

			return value;
		}

		/// <summary>
		/// Gets the primary keys of the rows that have one of the specified values for the specified column.
		/// </summary>
		/// <param name="values">The values to find.</param>
		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="values"/> contains a null reference.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>
		/// The primary keys of the rows that have the specified value for the specified column.
		/// </returns>
		public string[] Lookup(IEnumerable<string> values)
		{
			string[] allowedValues = HelperClass.ToStringArray(values, "values");

			return LookupSlNet(allowedValues);
		}

		/// <summary>
		/// Gets the primary keys of the rows that have the specified value for the specified column.
		/// </summary>
		/// <param name="value">The value to find.</param>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>
		/// The primary keys of the rows that have the specified value for the specified column.
		/// </returns>
		public string[] Lookup(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			return LookupSlNet(new [] { value });
		}

		/// <summary>
		/// Sets the value of a cell in a table.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <param name="value">The value to set.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="primaryKey"/> or <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		public void SetValue(string primaryKey, T value)
		{
			if (primaryKey == null)
			{
				throw new ArgumentNullException("primaryKey");
			}

			if (String.IsNullOrWhiteSpace(primaryKey))
			{
				throw new ArgumentException("The primary key must not be the empty string (\"\") or white space.", "primaryKey");
			}

			IDmsElement element = table.Element;

			HelperClass.CheckElementState(element);

			SetParameterMessage message = new SetParameterMessage
			{
				DataMinerID = element.DmsElementId.AgentId,
				ElId = element.DmsElementId.ElementId,
				ParameterId = Id,
				TableIndex = primaryKey,
				TableIndexPreference = SetParameterTableIndexPreference.ByPrimaryKey,
				DisableInformationEventMessage = true
			};

			if (AddValueToSetParameterMessage(message, value))
			{
				element.Host.Dms.Communication.SendMessage(message);
			}
		}

		/// <summary>
		/// Sets the value of a cell in a table and waits on specified expected changes.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <param name="value">The value to set.</param>
		/// <param name="timeout">The maximum time to wait on the expected change.</param>
		/// <param name="expectedChanges">
		/// One or more expected changes. Can be <see cref="CellValue"/> or <see cref="ParamValue"/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="primaryKey"/>, <paramref name="expectedChanges"/> or <paramref
		/// name="value"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="TimeoutException">Expected change took too long.</exception>
		/// <exception cref="FormatException">One of the provided parameters is missing data.</exception>
		public void SetValue(string primaryKey, T value, TimeSpan timeout, ExpectedChanges expectedChanges)
		{
			if (expectedChanges == null)
			{
				throw new ArgumentNullException("expectedChanges");
			}

			if (expectedChanges.ExpectedParamChanges != null)
			{
				using (ParamWaiter waiter = new ParamWaiter(this.Table.Element.Host.Dms.Communication, expectedChanges.ExpectedParamChanges))
				{
					SetValue(primaryKey, value);
					int waitCount = waiter.WaitNext(timeout).Count();
					System.Diagnostics.Debug.WriteLine("WaitNext: " + waitCount);
				}
			}

			if (expectedChanges.ExpectedCellChanges != null)
			{
				using (CellWaiter waiter = new CellWaiter(this.Table.Element.Host.Dms.Communication, expectedChanges.ExpectedCellChanges))
				{
					SetValue(primaryKey, value);
					int waitCount = waiter.WaitNext(timeout).Count();
					System.Diagnostics.Debug.WriteLine("WaitNext: " + waitCount);
				}
			}
		}

		/// <summary>
		/// Sends a <see cref="GetParameterMessage"/> SLNet message.
		/// </summary>
		/// <param name="primaryKey">The primary key in case a call value needs to be retrieved.</param>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The response message.</returns>
		internal GetParameterResponseMessage SendGetParameterMessage(string primaryKey)
		{
			HelperClass.CheckElementState(table.Element);

			IDmsElement element = table.Element;
			DmsElementId dmsElementId = element.DmsElementId;

			try
			{
				var message = new GetParameterMessage
				{
					DataMinerID = dmsElementId.AgentId,
					ElId = dmsElementId.ElementId,
					ParameterId = Id
				};

				if (primaryKey != null)
				{
					message.TableIndex = primaryKey;
				}

				var response = (GetParameterResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

				return response;
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147024891 && e.Message == "No such element.")
				{
					// 0x80070005: Access is denied.
					throw new ElementNotFoundException(element.DmsElementId, e);
				}
				else if (e.ErrorCode == -2147220935)
				{
					// 0x80040239, SL_FAILED_NOT_FOUND, The object or file was not found.
					throw new ParameterNotFoundException(Id, element.DmsElementId, e);
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

		/// <summary>
		/// Sends an SLNet message to get the primary keys of the rows that have one of the specified
		/// values as value.
		/// </summary>
		/// <param name="values">The allowed values.</param>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">
		/// The element was not found in the DataMiner System.
		/// </exception>
		/// <returns>The primary keys of the matching rows.</returns>
		private string[] LookupSlNet(string[] values)
		{
			IDmsElement element = table.Element;

			HelperClass.CheckElementState(element);

			try
			{
				string[] result;

				uint[] ids = new uint[3];
				ids[0] = (uint)element.AgentId;
				ids[1] = (uint)element.Id;
				ids[2] = (uint)Id;

				int type;

				type = (int)NotifyType.GetKeysForIndex;

				SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
				{
					DataMinerID = element.AgentId,
					ElementID = element.Id,
					What = type,
					Uia1 = new UIA(ids),
					Sa2 = new SA(values)
				};

				DMSMessage response = element.Host.Dms.Communication.SendSingleResponseMessage(message);
				SetDataMinerInfoResponseMessage responseMessage = (SetDataMinerInfoResponseMessage)response;

				if (responseMessage != null && responseMessage.Sa != null && responseMessage.Sa.Sa != null)
				{
					result = responseMessage.Sa.Sa;
				}
				else
				{
					return new string[0];
				}

				return result;
			}
			catch (DataMinerCOMException e)
			{
				if (e.ErrorCode == -2147220718)
				{
					// 0x80040312, Unknown destination DataMiner specified.
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
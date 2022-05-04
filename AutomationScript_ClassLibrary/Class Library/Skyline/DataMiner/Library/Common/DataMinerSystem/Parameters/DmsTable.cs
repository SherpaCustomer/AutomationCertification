namespace Skyline.DataMiner.Library.Common
{
    using Net.Exceptions;
    using Net.Messages;
    using Net.Messages.Advanced;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents a table.
    /// </summary>
    internal class DmsTable : IDmsTable
    {
        /// <summary>
        /// The element this table belongs to.
        /// </summary>
        private readonly IDmsElement element;

        /// <summary>
        /// The table parameter ID.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsTable"/> class.
        /// </summary>
        /// <param name="element">The element this table belongs to.</param>
        /// <param name="id">The table parameter ID.</param>
        internal DmsTable(IDmsElement element, int id)
        {
            this.element = element;
            this.id = id;
        }

        /// <summary>
        /// Gets the element this table is part of.
        /// </summary>
        /// <value>The element this table is part of.</value>
        public IDmsElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the table parameter ID.
        /// </summary>
        /// <value>The table parameter ID.</value>
        public int Id
        {
            get { return id; }
        }

		/// <summary>
		/// Adds the provided row to this table.
		/// </summary>
		/// <param name="data">The row data.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="IncorrectDataException">Invalid data was provided.</exception>
		public void AddRow(object[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            HelperClass.CheckElementState(element);

            try
            {
                SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
                {
                    DataMinerID = element.AgentId,
                    ElementID = element.Id,
                    What = 149,
                    Var1 = new uint[] { (uint)element.AgentId, (uint)element.Id, (uint)id },
                    Var2 = data
                };

                element.Host.Dms.Communication.SendSingleResponseMessage(message);
            }
            catch (DataMinerCOMException e)
            {
                if (e.ErrorCode == -2147220718)
                {
                    // 0x80040312, Unknown destination DataMiner specified.
                    throw new ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220916)
                {
                    // 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
                    throw new ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220959)
                {
                    // 0x80040221, SL_INVALID_DATA, "Invalid data".
                    string message = String.Format(CultureInfo.InvariantCulture, "Invalid data - element: '{0}', table ID: '{1}', data: [{2}]", element.DmsElementId.Value, Id, String.Join(",", data));
                    throw new IncorrectDataException(message);
                }
                else
                {
                    throw;
                }
            }
        }

		/// <summary>
		/// Removes the row with the specified primary key from the table.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="IncorrectDataException">Invalid data was provided.</exception>
		/// <remarks>If the table does not contain a row with the specified primary key, the table remains unchanged. No exception is thrown.</remarks>
		public void DeleteRow(string primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            DeleteRowsSLNet(new string[] { primaryKey });
        }

		/// <summary>
		/// Removes the rows with the specified primary keys from the table.
		/// </summary>
		/// <param name="primaryKeys">The primary keys of the rows to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKeys"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">One of the primary keys is <see langword="null"/>.</exception>
		/// <exception cref="IncorrectDataException">Invalid data was provided.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <remarks>
		/// No exception is thrown when one or more of the provided primary keys does not exist in the table.
		/// In this case, only the rows with the provided primary keys that exist in the table are deleted.
		/// </remarks>
		public void DeleteRows(IEnumerable<string> primaryKeys)
        {
            string[] keys = HelperClass.ToStringArray(primaryKeys, "primaryKeys");

            if (keys.Length > 0)
            {
                DeleteRowsSLNet(keys);
            }
        }

        /// <summary>
        /// Gets the specified column.
        /// </summary>
        /// <param name="parameterId">The parameter ID.</param>
        /// <typeparam name="T">The type of the column.</typeparam>
        /// <exception cref="ArgumentException"><paramref name="parameterId"/> is invalid.</exception>
        /// <exception cref="NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
        /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
        public IDmsColumn<T> GetColumn<T>(int parameterId)
        {
            if (parameterId < 1)
            {
                throw new ArgumentException("Invalid parameter ID.", "parameterId");
            }

            Type type = typeof(T);

            if (type != typeof(string) && type != typeof(int?) && type != typeof(double?) && type != typeof(DateTime?))
            {
                throw new NotSupportedException("Only one of the following types is supported: string, int?, double? or DateTime?.");
            }

            return new DmsColumn<T>(this, parameterId);
        }

		/// <summary>
		/// Gets the table data.
		/// </summary>
		/// <param name="keyColumnIndex">The 0-based index of the key column.</param>
		/// <exception cref="ArgumentException"><paramref name="keyColumnIndex"/> is invalid.</exception>
		/// <exception cref="ParameterNotFoundException">The table parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The table data.</returns>
		public IDictionary<string, object[]> GetData(int keyColumnIndex = 0)
		{
			if (keyColumnIndex < 0)
			{
				throw new ArgumentException("Invalid key column index.", "keyColumnIndex");
			}

			try
			{
				HelperClass.CheckElementState(element);

				GetPartialTableMessage message = new GetPartialTableMessage(element.DmsElementId.AgentId, element.DmsElementId.ElementId, id, new[] { "forceFullTable=true" });

				ParameterChangeEventMessage response = (ParameterChangeEventMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);
				if (response == null)
				{
					throw new ParameterNotFoundException(id, element.DmsElementId);
				}

				var result = BuildDictionary(response, keyColumnIndex);

				return result;
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
					throw new ParameterNotFoundException(id, element.DmsElementId, e);
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
		/// Gets the table data. Partial tables will be queried in the background per page, which has less impact on the performance.
		/// </summary>
		/// <param name="filters">Additional filters can be added to already filter out the result on a certain column value.</param>
		/// <exception cref="ParameterNotFoundException">The table parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>Every row of the table is yield returned as an object array.</returns>
		public IEnumerable<object[]> QueryData(IEnumerable<ColumnFilter> filters)
		{
			TableFilter tableFilter = new TableFilter(filters);

			while (tableFilter.PageId >= 0)
			{
				IDmsTableQueryResult result = QueryDataInternal(tableFilter);

				foreach (object[] row in result.PageRows)
				{
					yield return row;
				}

				if (tableFilter.IsIncludeAllPages || tableFilter.PageId >= result.NextPageId)
				{
					break;
				}

				tableFilter.PageId = result.NextPageId;
			}
		}

		/// <summary>
		/// Gets the display key that corresponds with the specified primary key.
		/// </summary>
		/// <param name="primaryKey">The primary key.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <returns>The display key that corresponds with the specified primary key.</returns>
		public string GetDisplayKey(string primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            HelperClass.CheckElementState(Element);

            string result = null;

            GetDynamicTableIndices message = BuildGetDynamicTableIndicesMessage(primaryKey, GetDynamicTableIndicesKeyFilterType.PrimaryKey);

            DynamicTableIndicesResponse responseMessage = (DynamicTableIndicesResponse)element.Host.Dms.Communication.SendSingleResponseMessage(message);

            if (responseMessage != null && responseMessage.Indices != null && responseMessage.Indices.Length > 0)
            {
                result = GetDisplayKey(primaryKey, responseMessage.Indices);
            }

            return result;
        }

		/// <summary>
		/// Gets the display keys.
		/// </summary>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <returns>The display keys.</returns>
		public string[] GetDisplayKeys()
        {
            string[] result;

            HelperClass.CheckElementState(Element);

            GetDynamicTableIndices message = new GetDynamicTableIndices
            {
                DataMinerID = element.AgentId,
                ElementID = element.Id,
                ParameterID = id
            };

            DMSMessage response = element.Host.Dms.Communication.SendSingleResponseMessage(message);

            DynamicTableIndicesResponse responseMessage = (DynamicTableIndicesResponse)response;

            if (responseMessage != null && responseMessage.Indices != null)
            {
                DynamicTableIndex[] indices = responseMessage.Indices;

                List<string> displayKeys = new List<string>(indices.Length);

                foreach (DynamicTableIndex index in indices)
                {
                    displayKeys.Add(index.DisplayValue);
                }

                result = displayKeys.ToArray();
            }
            else
            {
                result = new string[0];
            }

            return result;
        }

		/// <summary>
		/// Retrieves the primary key that corresponds with the specified display key.
		/// </summary>
		/// <param name="displayKey">The display key.</param>
		/// <exception cref="ArgumentNullException"><paramref name="displayKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <returns>The primary key that corresponds with the specified display key.</returns>
		public string GetPrimaryKey(string displayKey)
        {
            if (displayKey == null)
            {
                throw new ArgumentNullException("displayKey");
            }

            HelperClass.CheckElementState(Element);

            string result = null;

            GetDynamicTableIndices message = BuildGetDynamicTableIndicesMessage(displayKey, GetDynamicTableIndicesKeyFilterType.DisplayKey);

            DMSMessage response = element.Host.Dms.Communication.SendSingleResponseMessage(message);

            DynamicTableIndicesResponse responseMessage = (DynamicTableIndicesResponse)response;

            if (responseMessage != null && responseMessage.Indices != null && responseMessage.Indices.Length > 0)
            {
                result = GetPrimaryKey(displayKey, responseMessage.Indices);
            }

            return result;
        }

		/// <summary>
		/// Retrieves the primary keys.
		/// </summary>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <returns>The primary keys.</returns>
		public string[] GetPrimaryKeys()
        {
            string[] result;

            HelperClass.CheckElementState(Element);

            GetDynamicTableIndices message = new GetDynamicTableIndices
            {
                DataMinerID = element.AgentId,
                ElementID = element.Id,
                ParameterID = id
            };

            DMSMessage response = element.Host.Dms.Communication.SendSingleResponseMessage(message);

            DynamicTableIndicesResponse responseMessage = (DynamicTableIndicesResponse)response;

            if (responseMessage != null && responseMessage.Indices != null)
            {
                DynamicTableIndex[] indices = responseMessage.Indices;

                List<string> primaryKeys = new List<string>(indices.Length);

                foreach (DynamicTableIndex index in indices)
                {
                    primaryKeys.Add(index.IndexValue);
                }

                result = primaryKeys.ToArray();
            }
            else
            {
                result = new string[0];
            }

            return result;
        }

		/// <summary>
		/// Retrieves the row with the specified primary key.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The provided value is the empty string ("") or white space.</exception>
		/// <exception cref="ParameterNotFoundException">The table parameter was not found.</exception>
		/// <exception cref="KeyNotFoundInTableException">The specified <paramref name="primaryKey"/> does not exist in the table.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The row formatted as an object array.</returns>
		public object[] GetRow(string primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            if (String.IsNullOrWhiteSpace(primaryKey))
            {
                throw new ArgumentException("The provided primary key must not be empty or white space.", "primaryKey");
            }

            HelperClass.CheckElementState(Element);

            try
            {
                object[] details = new object[4];
                details[0] = element.DmsElementId.AgentId;
                details[1] = element.DmsElementId.ElementId;
                details[2] = id;
                details[3] = primaryKey;

                var message = new SetDataMinerInfoMessage
                {
                    DataMinerID = element.DmsElementId.AgentId,
                    ElementID = element.DmsElementId.ElementId,
                    What = 215,
                    Var1 = details
                };

                var response = (SetDataMinerInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

                object[] rowValues = (object[])response.RawData;

                if (rowValues != null && rowValues.Length > 0)
                {
                    if (IsRowContainingOnlyNullReferences(rowValues))
                    {
                        string exceptionMessage = String.Format(
                            CultureInfo.InvariantCulture,
                            "No row with primary key \"{0}\" was found in the table (table ID: '{1}', element: {2}).",
                            primaryKey,
                            Id,
                            element.DmsElementId.Value);

                        throw new KeyNotFoundInTableException(exceptionMessage);
                    }
                }
                else
                {
                    string exceptionMessage = String.Format(
                        CultureInfo.InvariantCulture,
                        "No row with primary key \"{0}\" was found in the table (table ID: '{1}', element: {2}).",
                        primaryKey,
                        Id,
                        element.DmsElementId.Value);

                    throw new KeyNotFoundInTableException(exceptionMessage);
                }

                return rowValues;
            }
            catch (DataMinerCOMException e)
            {
                if (e.ErrorCode == -2147220718)
                {
                    // 0x80040312, Unknown destination DataMiner specified.
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
		/// Retrieves the table rows.
		/// </summary>
		/// <exception cref="ParameterNotFoundException">The parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The table formatted as an object array of columns.</returns>
		public object[][] GetRows()
		{
			HelperClass.CheckElementState(element);

			try
			{
				GetPartialTableMessage message = new GetPartialTableMessage
				{
					DataMinerID = element.DmsElementId.AgentId,
					ElementID = element.DmsElementId.ElementId,
					ParameterID = id,
					Filters = new[] { "forceFullTable=true" }
				};

				ParameterChangeEventMessage response = (ParameterChangeEventMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);
				if (response == null)
				{
					throw new ParameterNotFoundException(id, element.DmsElementId);
				}

				if (response.NewValue == null || response.NewValue.ArrayValue == null)
				{
					return new object[][] { };
				}

				ParameterValue[] responseColumns = response.NewValue.ArrayValue;

				object[][] rows = responseColumns != null ? BuildRows(responseColumns) : new object[][] { };

				return rows;
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
		/// Determines whether a row with the specified primary key exists in the table.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="primaryKey"/> is the empty string ("") or white space.</exception>
		/// <exception cref="IncorrectDataException">The provided data is invalid.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns><c>true</c> if the table contains a row with the specified primary key; otherwise, <c>false</c>.</returns>
		public bool RowExists(string primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            if (String.IsNullOrWhiteSpace(primaryKey))
            {
                throw new ArgumentException("The provided primary key must not be the empty string (\"\") or white space.", "primaryKey");
            }

            HelperClass.CheckElementState(Element);

            try
            {
                bool rowExists = false;

                SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
                {
                    DataMinerID = element.AgentId,
                    ElementID = element.Id,
                    What = 163,
                    Var1 = new uint[] { (uint)element.AgentId, (uint)element.Id, (uint)Id },
                    Var2 = primaryKey
                };

                DMSMessage response = element.Host.Dms.Communication.SendSingleResponseMessage(message);
                SetDataMinerInfoResponseMessage responseMessage = (SetDataMinerInfoResponseMessage)response;

                int position = (int)responseMessage.RawData;

                if (position > 0)
                {
                    rowExists = true;
                }

                return rowExists;
            }
            catch (DataMinerCOMException e)
            {
                if (e.ErrorCode == -2147220718)
                {
                    // 0x80040312, Unknown destination DataMiner specified.
                    throw new ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220916)
                {
                    // 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
                    throw new ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220959)
                {
                    // 0x80040221, SL_INVALID_DATA, "Invalid data".
                    string message = String.Format(CultureInfo.InvariantCulture, "Invalid data - element: '{0}', table ID: '{1}', primary key: \"{2}\"", element.DmsElementId.Value, Id, primaryKey);
                    throw new IncorrectDataException(message);
                }
                else
                {
                    throw;
                }
            }
        }

		/// <summary>
		/// Updates the row with the provided data.
		/// </summary>
		/// <param name="primaryKey">The primary key of the row that must be updated.</param>
		/// <param name="data">The new row data.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKey"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The provided primary key is the empty string ("") or white space.</exception>
		/// <exception cref="ParameterNotFoundException">The table parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		public void SetRow(string primaryKey, object[] data)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (String.IsNullOrWhiteSpace(primaryKey))
            {
                throw new ArgumentException("The provided primary key must not be the empty string (\"\") or white space.", "primaryKey");
            }

            HelperClass.CheckElementState(Element);

            try
            {
                object[] ids = new object[4];
                ids[0] = element.AgentId;
                ids[1] = element.Id;
                ids[2] = id;
                ids[3] = primaryKey;

                SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
                {
                    DataMinerID = element.AgentId,
                    ElementID = element.Id,
                    What = 225,
                    Var1 = ids,
                    Var2 = data
                };

                element.Host.Dms.Communication.SendSingleResponseMessage(message);
            }
            catch (DataMinerCOMException e)
            {
                if (e.ErrorCode == -2147220718)
                {
                    // 0x80040312, Unknown destination DataMiner specified.
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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Table Parameter:{0}", id);
        }

		/// <summary>
		/// Builds the dictionary from the response message.
		/// </summary>
		/// <param name="response">The response message.</param>
		/// <param name="keyColumnIndex">The key column index.</param>
		/// <exception cref="ArgumentNullException"><paramref name="response"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyColumnIndex"/> is invalid.</exception>
		/// <returns>The dictionary holding the table data.</returns>
		internal static IDictionary<string, object[]> BuildDictionary(ParameterChangeEventMessage response, int keyColumnIndex)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			var result = new Dictionary<string, object[]>();

			if (response.NewValue == null || response.NewValue.ArrayValue == null)
			{
				return result;
			}

			ParameterValue[] columns = response.NewValue.ArrayValue;

			if (keyColumnIndex >= columns.Length)
			{
				throw new ArgumentException("Invalid key column index.", "keyColumnIndex");
			}

			// Dictionary used as a mapping from index to key.
			string[] keyMap = new string[columns[keyColumnIndex].ArrayValue.Length];

			int rowNumber = 0;

			foreach (ParameterValue keyCell in columns[keyColumnIndex].ArrayValue)
			{
				string primaryKey = Convert.ToString(keyCell.CellValue.InteropValue, CultureInfo.CurrentCulture);

				result[primaryKey] = new object[columns.Length];
				keyMap[rowNumber] = primaryKey;
				rowNumber++;
			}

			int columnNumber = 0;
			foreach (ParameterValue column in columns)
			{
				rowNumber = 0;

				foreach (ParameterValue cell in column.ArrayValue)
				{
					result[keyMap[rowNumber]][columnNumber] = cell.CellValue.ValueType == ParameterValueType.Empty ? null : cell.CellValue.InteropValue;
					rowNumber++;
				}

				columnNumber++;
			}

			return result;
		}

		/// <summary>
		/// Constructs the table rows as a jagged array from the response message.
		/// </summary>
		/// <param name="columns">The columns of the response.</param>
		/// <returns>The jagged array representing the table rows.</returns>
		private static object[][] BuildRows(ParameterValue[] columns)
        {
            object[][] rows;

            int columnCount = columns.Length;
            int rowCount = 0;

            if (columnCount > 0)
            {
                ParameterValue[] responseColumn = columns[0].ArrayValue;
                rowCount = responseColumn.Length;
            }

            if (columnCount > 0 && rowCount > 0)
            {
                rows = new object[rowCount][];

                for (int r = 0; r < rowCount; r++)
                {
                    rows[r] = new object[columnCount];
                }
            }
            else
            {
                rows = new object[][] { };
            }

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                ParameterValue[] responseColumn = columns[columnIndex].ArrayValue;

                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    rows[rowIndex][columnIndex] = responseColumn[rowIndex].IsEmpty ? null : responseColumn[rowIndex].ArrayValue[0].InteropValue;
                }
            }

            return rows;
        }

        /// <summary>
        /// Retrieves the display key that corresponds with the specified primary key from the indices.
        /// </summary>
        /// <param name="primaryKey">The primary key for which the corresponding display key should be obtained.</param>
        /// <param name="indices">The returned display key primary key pairs.</param>
        /// <returns>The display key that corresponds with the specified primary key.</returns>
        private static string GetDisplayKey(string primaryKey, DynamicTableIndex[] indices)
        {
            if (indices.Length == 1)
            {
                return indices[0].DisplayValue;
            }
            else
            {
                foreach (DynamicTableIndex index in indices)
                {
                    if (index != null && String.Equals(index.IndexValue, primaryKey, StringComparison.OrdinalIgnoreCase))
                    {
                        return index.DisplayValue;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Retrieves the primary key that corresponds with the specified display key from the indices.
        /// </summary>
        /// <param name="displayKey">The display key for which the corresponding primary key should be obtained.</param>
        /// <param name="indices">The returned display key primary key pairs.</param>
        /// <returns>The primary key that corresponds with the specified display key.</returns>
        private static string GetPrimaryKey(string displayKey, DynamicTableIndex[] indices)
        {
            if (indices.Length == 1)
            {
                return indices[0].IndexValue;
            }
            else
            {
                foreach (DynamicTableIndex index in indices)
                {
                    if (index != null && String.Equals(index.DisplayValue, displayKey, StringComparison.OrdinalIgnoreCase))
                    {
                        return index.IndexValue;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Determines whether the row only contains null references.
        /// </summary>
        /// <param name="rowValues">The row values.</param>
        /// <returns><c>true</c> if the row only consists of null references; otherwise, <c>false</c>.</returns>
        private static bool IsRowContainingOnlyNullReferences(object[] rowValues)
        {
            bool allNullReferences = true;

            for (int i = 0; i < rowValues.Length; i++)
            {
                if (rowValues[i] != null)
                {
                    allNullReferences = false;
                    break;
                }
            }

            return allNullReferences;
        }

        /// <summary>
        /// Builds a message to retrieve the corresponding key.
        /// </summary>
        /// <param name="displayKey">The display key.</param>
        /// <param name="keyType">The key type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="displayKey"/> is <see langword="null"/>.</exception>
        /// <returns>The message to retrieve the key.</returns>
        private GetDynamicTableIndices BuildGetDynamicTableIndicesMessage(string displayKey, GetDynamicTableIndicesKeyFilterType keyType)
        {
            if (displayKey == null)
            {
                throw new ArgumentNullException("displayKey");
            }

            GetDynamicTableIndices message = new GetDynamicTableIndices
            {
                DataMinerID = element.AgentId,
                ElementID = element.Id,
                ParameterID = id,
            };

            if (!displayKey.Contains("?") && !displayKey.Contains("*"))
            {
                message.KeyFilterType = keyType;
                message.KeyFilter = displayKey;
            }

            return message;
        }

		/// <summary>
		/// Performs an SLNet call to delete the rows with the specified primary keys.
		/// </summary>
		/// <param name="primaryKeys">The primary keys of the rows to delete.</param>
		/// <exception cref="IncorrectDataException" >Invalid data.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		private void DeleteRowsSLNet(string[] primaryKeys)
        {
            HelperClass.CheckElementState(Element);

            try
            {
                SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
                {
                    DataMinerID = element.AgentId,
                    ElementID = element.Id,
                    What = 156,
                    Var1 = new uint[] { (uint)element.AgentId, (uint)element.Id, (uint)id },
                    Var2 = primaryKeys
                };

                element.Host.Dms.Communication.SendSingleResponseMessage(message);
            }
            catch (DataMinerCOMException e)
            {
                if (e.ErrorCode == -2147220718)
                {
                    // 0x80040312, Unknown destination DataMiner specified.
                    throw new ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220916)
                {
                    // 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
                    throw new ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220959)
                {
                    // 0x80040221, SL_INVALID_DATA, "Invalid data".
                    string message = String.Format(CultureInfo.InvariantCulture, "Invalid data - element: '{0}', table ID: '{1}', primary key(s): [{2}]", element.DmsElementId.Value, Id, String.Join(",", primaryKeys));
                    throw new IncorrectDataException(message);
                }
                else
                {
                    throw;
                }
            }
        }

		/// <summary>
		/// Queries the table using the specified table filters.
		/// </summary>
		/// <param name="filters">The table filters.</param>
		/// <returns>The query result.</returns>
		/// <exception cref="ParameterNotFoundException">The table parameter was not found.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		private IDmsTableQueryResult QueryDataInternal(TableFilter filters)
		{
			try
			{
				List<string> filterValues = new List<string>();

				foreach (var filterItem in filters.Filter)
				{
					string filterValue = GetFilterValue(filterItem);

					if (!String.IsNullOrWhiteSpace(filterValue))
					{
						filterValues.Add(filterValue);
					}
				}

				if (filterValues.Any())
				{
					filters.IsIncludeAllPages = true;   // when there are filters defined then it means that the entire table needs to be constructed with filtered items and placed into new pages per request, this is too impacting so a force return in one result is called.
				}

				if (filters.PageId > 0 && !filters.IsIncludeAllPages)
				{
					filterValues.Add("page=" + filters.PageId);
				}

				if (filters.IsIncludeAllPages)
				{
					filterValues.Add("forceFullTable=true");
				}

				HelperClass.CheckElementState(element);

				GetPartialTableMessage message = new GetPartialTableMessage(element.DmsElementId.AgentId, element.DmsElementId.ElementId, id, filterValues.ToArray());

				ParameterChangeEventMessage response = (ParameterChangeEventMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);
				if (response == null)
				{
					throw new ParameterNotFoundException(id, element.DmsElementId);
				}

				IDmsTableQueryResult result = new DmsTableQueryResult(response);

				return result;
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
					throw new ParameterNotFoundException(id, element.DmsElementId, e);
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

		private static string GetFilterValue(ColumnFilter tableFilterItem)
		{
			if (tableFilterItem.Pid < 1 || System.String.IsNullOrWhiteSpace(tableFilterItem.Value))
			{
				return System.String.Empty;
			}

			string returnValue = "value=" + tableFilterItem.Pid;
			switch (tableFilterItem.ComparisonOperator)
			{
				case ComparisonOperator.GreaterThan:
					returnValue = returnValue + " > ";
					break;
				case ComparisonOperator.GreaterThanOrEqual:
					returnValue = returnValue + " >= ";
					break;
				case ComparisonOperator.LessThan:
					returnValue = returnValue + " < ";
					break;
				case ComparisonOperator.LessThanOrEqual:
					returnValue = returnValue + " <= ";
					break;
				case ComparisonOperator.NotEqual:
					returnValue = returnValue + " != ";
					break;
				default:
					returnValue = returnValue + " == ";
					break;
			}

			returnValue = returnValue + tableFilterItem.Value;
			return returnValue;
		}
	}
}
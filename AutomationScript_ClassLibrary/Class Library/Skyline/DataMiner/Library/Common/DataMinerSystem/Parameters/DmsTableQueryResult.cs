namespace Skyline.DataMiner.Library.Common
{
	using System.Collections.Generic;

	internal class DmsTableQueryResult : IDmsTableQueryResult
	{
		private readonly int totalPageCount;
		private readonly int currentPageId;
		private readonly int nextPageId;
		private readonly int totalRowCount;
		private readonly ICollection<object[]> pageRows;

		public DmsTableQueryResult(Skyline.DataMiner.Net.Messages.ParameterChangeEventMessage response)
		{
			if (response == null)
			{
				throw new System.ArgumentNullException("response");
			}

			if (response.NewValue == null || response.NewValue.ArrayValue == null)
			{
				totalPageCount = 0;
				currentPageId = 1;
				nextPageId = -1;
				totalRowCount = 0;
				pageRows = new List<object[]>();
				return;
			}

			var table = new Dictionary<int, object[]>();
			var columns = response.NewValue.ArrayValue;
			int columnNumber = 0;

			foreach (var column in columns)
			{
				int rowNumber = 0;

				foreach (var cell in column.ArrayValue)
				{
					object[] row;

					if (!table.TryGetValue(rowNumber, out row))
					{
						row = new object[columns.Length];
						table[rowNumber] = row;
					}

					row[columnNumber] = cell.CellValue.ValueType == Skyline.DataMiner.Net.Messages.ParameterValueType.Empty ? null : cell.CellValue.InteropValue;
					rowNumber++;
				}

				columnNumber++;
			}

			pageRows = table.Values;

			if (response.PartialDataInfo == null || response.PartialDataInfo.Pages == null || response.PartialDataInfo.CurrentTablePage == 0)
			{
				totalPageCount = 1;
				currentPageId = 1;
				nextPageId = -1;
				totalRowCount = pageRows.Count;
				return;
			}

			totalPageCount = response.PartialDataInfo.Pages.Length;
			currentPageId = response.PartialDataInfo.CurrentTablePage;
			totalRowCount = response.PartialDataInfo.TotalAmountRows;
			nextPageId = currentPageId >= totalPageCount ? -1 : (currentPageId + 1);
		}

		/// <summary>
		/// Gets the total number of pages that are present in the queried table.
		/// </summary>
		public int TotalPageCount
		{
			get
			{
				return totalPageCount;
			}
		}

		/// <summary>
		/// Gets the current page ID that has been returned.
		/// </summary>
		public int CurrentPageId
		{
			get
			{
				return currentPageId;
			}
		}

		/// <summary>
		/// Gets the next page ID to be requested. '-1' when there are no more pages to be retrieved.
		/// </summary>
		public int NextPageId
		{
			get
			{
				return nextPageId;
			}
		}

		/// <summary>
		/// Gets the total number of rows that are present in the table.
		/// </summary>
		public int TotalRowCount
		{
			get
			{
				return totalRowCount;
			}
		}

		/// <summary>
		/// Gets the rows that are present in this page.
		/// </summary>
		public ICollection<object[]> PageRows
		{
			get
			{
				return pageRows;
			}
		}
	}
}

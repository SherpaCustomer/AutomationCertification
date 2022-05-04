namespace Skyline.DataMiner.Library.Common
{
	using System.Collections.Generic;

	/// <summary>
	/// Filter to be applied when querying the table.
	/// </summary>
	internal class TableFilter
	{
		private readonly ICollection<ColumnFilter> filter;

		/// <summary>
		/// Initializes a new instance of the <see cref="TableFilter"/> class.
		/// </summary>
		/// <param name="filterItems">Filter to be applied when executing the query.</param>
		public TableFilter(IEnumerable<ColumnFilter> filterItems)
		{
			PageId = 0;
			IsIncludeAllPages = false;
			filter = new List<ColumnFilter>(filterItems);
		}

		/// <summary>
		/// Gets or sets the id of the page of the table to be returned. This has only effect on partial tables. This setting has no effect when querying normal tables, where all rows will be returned.
		/// </summary>
		public int PageId { get; set; }

		/// <summary>
		/// Gets or sets the indication if all pages of a partial table should be returned. Warning: when setting to 'true' on a partial table, without extra filtering, could result in a large object being returned, this could have a large impact on SLElement, SLNet, SLNetCom and SLScripting. This setting has no effect when querying normal tables, where all rows will be returned. 
		/// </summary>
		public bool IsIncludeAllPages { get; set; }

		/// <summary>
		/// Gets the collection of filters that will be applied when querying the table. Every item in the filter will be combined as a logical AND.
		/// </summary>
		public System.Collections.Generic.ICollection<ColumnFilter> Filter
		{
			get
			{
				return filter;
			}
		}
	}
}

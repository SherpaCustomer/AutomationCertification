
namespace Skyline.DataMiner.Library.Common.Subscription.Monitors.Parameter.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Cache of the current values of the rows in a table.
	/// </summary>
	internal class TableCache
	{
		public TableCache()
		{
			Rows = new Dictionary<string, object[]>();
		}

		public IDictionary<string, object[]> Rows { get; private set; }

		/// <summary>
		/// Updates the cache with new values and checks if there are changes.
		/// </summary>
		/// <param name="change">The table change to apply to this cache.</param>
		/// <returns>Returns true if there were effective changes, otherwise false.</returns>
		internal bool ApplyUpdate(TableValueChange change)
		{
			if (change == null) throw new ArgumentNullException("change");

			bool hasChanges = false;

			if (change.DeletedRows != null)
			{
				if (ApplyDeletedRows(change.DeletedRows)) hasChanges = true;
			}

			if (change.UpdatedRows != null)
			{
				if (ApplyUpdatedRows(change.UpdatedRows)) hasChanges = true;
			}

			return hasChanges;
		}

		private bool ApplyDeletedRows(string[] deletedRows)
		{
			bool hasChanges = false;
			
			foreach (var r in deletedRows)
			{
				if (Rows.Remove(r))
				{
					hasChanges = true;
				}
			}

			return hasChanges;
		}

		private bool ApplyUpdatedRows(IDictionary<string, object[]> updatedRows)
		{
			bool hasChanges = false;
			
			foreach (var r in updatedRows)
			{
				if (ApplyRowUpdate(r.Key, r.Value))
				{
					hasChanges = true;
				}
			}

			return hasChanges;
		}

		private bool ApplyRowUpdate(string key, object[] newValues)
		{
			bool hasChanges = false;
			
			object[] cachedRow;
			if (Rows.TryGetValue(key, out cachedRow))
			{
				if (cachedRow.Length < newValues.Length)
				{
					// should not be possible, but you never know...
					Array.Resize(ref cachedRow, newValues.Length);
				}

				for (int i = 0; i < newValues.Length; i++)
				{
					if (newValues[i] == null || Equals(newValues[i], cachedRow[i])) continue;

					cachedRow[i] = newValues[i];
					hasChanges = true;
				}
			}
			else
			{
				Rows.Add(key, newValues);
				hasChanges = true;
			}

			return hasChanges;
		}
	}
}

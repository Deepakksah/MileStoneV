using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration;

public static class DataSetUtility
{
	private enum DataSortOrder
	{
		InsertOrUpdate,
		Delete
	}

	private static Dictionary<string, IEnumerable<string>> TablesToExcludeFromUpdateLastModified { get; set; }

	static DataSetUtility()
	{
		TablesToExcludeFromUpdateLastModified = new Dictionary<string, IEnumerable<string>>();
		using ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		string[] value = new string[1] { configurationDataSet.DevicePrivacyMask.TableName };
		TablesToExcludeFromUpdateLastModified.Add(configurationDataSet.DataSetName, value);
	}

	public static void UpdateLastModified(DataSet originalDataSet, bool onlyUpdateUnmodifiedTimestamps)
	{
		UpdateLastModified(originalDataSet, onlyUpdateUnmodifiedTimestamps, DateTime.UtcNow);
	}

	public static void UpdateLastModified(DataSet dataSet, bool onlyUpdateUnmodifiedTimestamps, DateTime timeStamp)
	{
		if (dataSet == null)
		{
			throw new ArgumentNullException("dataSet");
		}
		bool flag = !onlyUpdateUnmodifiedTimestamps;
		DataSet changes = dataSet.GetChanges(DataRowState.Modified);
		if (changes == null)
		{
			return;
		}
		if (!TablesToExcludeFromUpdateLastModified.TryGetValue(dataSet.DataSetName, out var value))
		{
			value = Enumerable.Empty<string>();
		}
		string[] array = new string[2] { "LastModified", "last_modified" };
		foreach (string text in array)
		{
			foreach (DataTable table in changes.Tables)
			{
				if (value.Any((string s) => string.Equals(s, table.TableName, StringComparison.Ordinal)) || table.Rows.Count <= 0 || !table.Columns.Contains(text))
				{
					continue;
				}
				DataTable dataTable = dataSet.Tables[table.TableName];
				DataColumn[] primaryKey = table.PrimaryKey;
				if (primaryKey != null && primaryKey.Length != 0)
				{
					foreach (DataRow row in table.Rows)
					{
						if (row.RowState == DataRowState.Modified)
						{
							List<object> list = new List<object>();
							DataColumn[] array2 = primaryKey;
							foreach (DataColumn column in array2)
							{
								list.Add(row[column]);
							}
							DataRow dataRow2 = dataTable.Rows.Find(list.ToArray());
							if (flag || dataRow2[text, DataRowVersion.Original].Equals(dataRow2[text]))
							{
								dataRow2[text] = timeStamp;
							}
						}
					}
					continue;
				}
				foreach (DataRow row2 in dataTable.Rows)
				{
					if (row2.RowState == DataRowState.Modified && (flag || row2[text, DataRowVersion.Original].Equals(row2[text])))
					{
						row2[text] = timeStamp;
					}
				}
			}
		}
	}

	public static DataRow[] SortRowsForInsertOrUpdate(DataRow[] dataRows, string primaryKeyColumnName, string foreignKeyColumnName)
	{
		return SortRows(dataRows, DataSortOrder.InsertOrUpdate, DataRowVersion.Current, primaryKeyColumnName, foreignKeyColumnName);
	}

	public static DataRow[] SortRowsForDelete(DataRow[] dataRows, string primaryKeyColumnName, string foreignKeyColumnName)
	{
		return SortRows(dataRows, DataSortOrder.Delete, DataRowVersion.Original, primaryKeyColumnName, foreignKeyColumnName);
	}

	private static DataRow[] SortRows(DataRow[] dataRows, DataSortOrder sortOrder, DataRowVersion dataRowVersion, string primaryKeyColumnName, string foreignKeyColumnName)
	{
		List<DataRow> list = new List<DataRow>();
		Dictionary<object, DataRow> dictionary = new Dictionary<object, DataRow>();
		foreach (DataRow dataRow in dataRows)
		{
			dictionary.Add(dataRow[primaryKeyColumnName, dataRowVersion], dataRow);
		}
		while (dictionary.Count > 0)
		{
			DataRow dataRow2 = FindFirstRowWithoutParent(dictionary, dataRowVersion, primaryKeyColumnName, foreignKeyColumnName);
			list.Add(dataRow2);
			dictionary.Remove(dataRow2[primaryKeyColumnName, dataRowVersion]);
		}
		if (sortOrder == DataSortOrder.Delete)
		{
			list.Reverse();
		}
		return list.ToArray();
	}

	private static DataRow FindFirstRowWithoutParent(Dictionary<object, DataRow> rowsByPrimaryKey, DataRowVersion dataRowVersion, string primaryKeyColumnName, string foreignKeyColumnName)
	{
		return FindTopLevelRowRecursive(rowsByPrimaryKey.First().Value, rowsByPrimaryKey, dataRowVersion, primaryKeyColumnName, foreignKeyColumnName);
	}

	private static DataRow FindTopLevelRowRecursive(DataRow row, Dictionary<object, DataRow> rowsByPrimaryKey, DataRowVersion dataRowVersion, string primaryKeyColumnName, string foreignKeyColumnName)
	{
		if (row[foreignKeyColumnName, dataRowVersion] == DBNull.Value)
		{
			return row;
		}
		if (rowsByPrimaryKey.TryGetValue(row[foreignKeyColumnName, dataRowVersion], out var value))
		{
			return FindTopLevelRowRecursive(value, rowsByPrimaryKey, dataRowVersion, primaryKeyColumnName, foreignKeyColumnName);
		}
		return row;
	}
}

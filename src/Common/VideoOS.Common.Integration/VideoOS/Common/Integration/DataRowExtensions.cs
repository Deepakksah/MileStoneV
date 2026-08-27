using System;
using System.Data;
using System.Globalization;

namespace VideoOS.Common.Integration;

public static class DataRowExtensions
{
	public static T GetColumnValue<T>(this DataRow row, DataColumn column)
	{
		if (row == null)
		{
			throw new ArgumentNullException("row");
		}
		if (column == null)
		{
			throw new ArgumentNullException("column");
		}
		switch (row.RowState)
		{
		case DataRowState.Unchanged:
		case DataRowState.Added:
		case DataRowState.Modified:
			return row.GetColumnValue<T>(column, DataRowVersion.Current);
		case DataRowState.Deleted:
			return row.GetColumnValue<T>(column, DataRowVersion.Original);
		default:
			throw new InvalidOperationException("Row state is not added, modified or deleted.");
		}
	}

	private static bool IsNullable<T>()
	{
		return Nullable.GetUnderlyingType(typeof(T)) != null;
	}

	public static T GetColumnValue<T>(this DataRow row, DataColumn column, DataRowVersion version)
	{
		if (row == null)
		{
			throw new ArgumentNullException("row");
		}
		if (column == null)
		{
			throw new ArgumentNullException("column");
		}
		try
		{
			if (IsNullable<T>() && row.IsNull(column, version))
			{
				return default(T);
			}
			return (T)row[column, version];
		}
		catch (InvalidCastException innerException)
		{
			throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", column.ColumnName, column.Table.TableName), innerException);
		}
	}
}

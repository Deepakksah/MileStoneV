using VideoOS.Common.Integration;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public static class SqlColumnBinderExtensions
{
	public static string GetGeographyString(this DatabaseColumnBinder columnBinder, DatabaseDataReader reader)
	{
		return columnBinder.GetGeographyString(reader, allowNulls: false, CommonIds.GisPointEmpty);
	}

	public static string GetGeographyString(this DatabaseColumnBinder columnBinder, DatabaseDataReader reader, bool allowNulls, string defaultValue)
	{
		return columnBinder.GetString(reader, allowNulls, defaultValue);
	}
}

using VideoOS.SqlUpdate.Core.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Providers;

public class DatabaseSkuProvider : IDatabaseSkuProvider
{
	public VmsDbType GetSku(DatabaseConnection databaseConnection, string masterKey)
	{
		return VmsDbType.Unknown;
	}
}

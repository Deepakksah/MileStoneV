using VideoOS.SqlUpdate.Core.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Providers;

public interface IDatabaseSkuProvider
{
	VmsDbType GetSku(DatabaseConnection databaseConnection, string masterKey);
}

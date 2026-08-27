using VideoOS.Common.Integration;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.DataComponents;

public sealed class SqlLogFactory : ISqlLogFactory
{
	public ISqlLogProcessor CreateSqlLogProcessor()
	{
		return CreateSqlLogProcessor(Server.Instance.DataComponentFactory, Server.Instance.SqlConnectionManager, Server.Instance.AuditAndSystemLogger);
	}

	public static ISqlLogProcessor CreateSqlLogProcessor(IDataComponentFactory dataComponentFactory, ISqlConnectionManager sqlConnectionManager, ILocalizedLogger auditAndSystemLogger)
	{
		using ISiteConfigDataComponent siteConfigDataComponent = dataComponentFactory.CreateSiteConfigDataComponent();
		Site site = siteConfigDataComponent.QueryHomeSite();
		return new SqlLogProcessor(sqlConnectionManager, site.Id, site.Name, disablePermissionAuditLog: false, auditAndSystemLogger);
	}
}

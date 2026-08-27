using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Exceptions;

namespace VideoOS.Management.Server.Log;

public class VmoLogManager : ISqlLogProcessor, IDisposable
{
	private SqlLogProcessor _sqlLogProcessor;

	public LogEntryAuditLogger LogEntryAuditLogger => _sqlLogProcessor.LogEntryAuditLogger;

	public VmoLogManager(IGeminiRequestContext requestContext)
	{
		Site site;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			site = siteComponent.QueryHomeSite();
		}
		DatabaseSettings databaseSettings = requestContext.GetDatabaseSettings() as DatabaseSettings;
		bool getDisablePermissionLogsSetting = requestContext.GetService<PermissionAuditLogService>().GetDisablePermissionLogsSetting;
		_sqlLogProcessor = new SqlLogProcessor(databaseSettings.GetSqlConnectionManager(), Guid.Parse(site.Id), site.Name, getDisablePermissionLogsSetting, requestContext.GetService<AuditAndSystemLoggingService>().Logger);
	}

	public void Dispose()
	{
		_sqlLogProcessor.Dispose();
	}

	public void LogSession(Guid sessionId, string userCredentials, string remoteIPAddress)
	{
		try
		{
			_sqlLogProcessor.LogSession(sessionId, userCredentials, remoteIPAddress);
		}
		catch (VideoOS.Common.Integration.Exceptions.DatabaseConfigExtraRowsException ex)
		{
			throw new DatabaseConfigExtraRowsException(ex.Message, ex);
		}
		catch (VideoOS.Common.Integration.Exceptions.DatabaseConfigMissingRowException ex2)
		{
			throw new DatabaseConfigMissingRowException(ex2.Message, ex2);
		}
	}
}

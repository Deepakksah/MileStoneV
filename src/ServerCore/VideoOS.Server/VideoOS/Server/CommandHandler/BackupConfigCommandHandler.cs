using System;
using System.IO;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Logging;
using VideoOS.Common.Integration.Properties;
using VideoOS.Server.BackupRestoreComponent.EventServer.Backup;
using VideoOS.Utilities.Database;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server.CommandHandler;

public sealed class BackupConfigCommandHandler : ICommandHandler
{
	private Server _server;

	private BackupConfigManager _backupManager;

	private IResponseSink _responseSink;

	public BackupConfigCommandHandler(Server server)
	{
		_server = server;
		IDatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.ServerService);
		ISqlConnectionManager sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo, server.Logger);
		BackupConfigManagerExternalCommunication backupConfigManagerExternalCommunication = new BackupConfigManagerExternalCommunication(sqlConnectionManager);
		_backupManager = new BackupConfigManager(backupConfigManagerExternalCommunication);
		_backupManager.AddComponent(new BackupConfigCorporateComponent(sqlConnectionManager));
		_backupManager.AddComponent(new BackupConfigIdentityServerComponent(new SqlConnectionManager(new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.IDP), server.Logger)));
		_backupManager.AddComponent(new BackupConfigEventServerComponent(new ConfigurationDataSetLoader(_server.Data)));
		_backupManager.BackupProgress += HandleBackupProgress;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is BackupConfigCommand backupConfigCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		_responseSink = responseSink;
		if (backupConfigCommand.Action == BackupConfigAction.StartBackup)
		{
			ServerInfoCollector serverInfoCollector = new ServerInfoCollector();
			_server.AuditAndSystemLogger.Log(LogEntryId.AuditConfigurationExported, Guid.Empty, serverInfoCollector.ManagementServerName(), serverInfoCollector.ManagementServerName(), ResourceType.ManagementServer, SourceType.Server, callContextInfo.LocationIPAddress, callContextInfo.UserDomainAccountName, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditConfigurationExported);
			BackupConfigResultData backupConfigResultData = _backupManager.Backup(Path.Combine(ServerConfig.Instance.BackupShareFolderPath, ServerConfig.Instance.BackupShareFolderName));
			switch (backupConfigResultData.Result)
			{
			case BackupConfigManager.BackupConfigResult.Success:
				responseSink.AddResponse(new BackupConfigCompletedResponse(backupConfigResultData.FileNamesToReturn));
				break;
			case BackupConfigManager.BackupConfigResult.Failed:
				responseSink.AddResponse(new BackupConfigFailedResponse(Convert(backupConfigResultData.Error), backupConfigResultData.EnglishErrorDescription, backupConfigResultData.ErrorParameters, backupConfigResultData.ServerException));
				break;
			case BackupConfigManager.BackupConfigResult.Aborted:
				responseSink.AddResponse(new BackupConfigAbortedResponse());
				break;
			}
		}
		else if (!_backupManager.AbortBackup())
		{
			responseSink.AddResponse(new BackupConfigFailedResponse(BackupConfigError.BackupAlreadyInactive, "AbortBackup method failed because there were no active backup.", null, null));
		}
	}

	private void HandleBackupProgress(object sender, BackupProgressEventArgs e)
	{
		_responseSink.AddResponse(new BackupConfigStatusResponse(Convert(e.ActionStatus), e.ComponentName, e.CurrentActionProgress));
	}

	private static BackupConfigActionStatus Convert(BackupConfigManager.BackupConfigActionStatus value)
	{
		return value switch
		{
			BackupConfigManager.BackupConfigActionStatus.Undefined => BackupConfigActionStatus.Undefined, 
			BackupConfigManager.BackupConfigActionStatus.NotStarted => BackupConfigActionStatus.NotStarted, 
			BackupConfigManager.BackupConfigActionStatus.Validating => BackupConfigActionStatus.Validating, 
			BackupConfigManager.BackupConfigActionStatus.CreatingBackup => BackupConfigActionStatus.CreatingBackup, 
			BackupConfigManager.BackupConfigActionStatus.CompressingBackupFiles => BackupConfigActionStatus.CompressingBackupFiles, 
			_ => throw new ArgumentException("value not supported.", "value"), 
		};
	}

	private static BackupConfigError Convert(BackupConfigManager.BackupConfigError value)
	{
		return value switch
		{
			BackupConfigManager.BackupConfigError.Undefined => BackupConfigError.Undefined, 
			BackupConfigManager.BackupConfigError.BackupAlreadyActive => BackupConfigError.BackupAlreadyActive, 
			BackupConfigManager.BackupConfigError.BackupAlreadyInactive => BackupConfigError.BackupAlreadyInactive, 
			BackupConfigManager.BackupConfigError.ExceptionOccurred => BackupConfigError.ExceptionOccurred, 
			BackupConfigManager.BackupConfigError.SqlServerRunningAsLocal => BackupConfigError.SqlServerRunningAsLocal, 
			BackupConfigManager.BackupConfigError.NoBackupFilesWereMade => BackupConfigError.NoBackupFilesWereMade, 
			BackupConfigManager.BackupConfigError.MoveFilesFailed => BackupConfigError.MoveFilesFailed, 
			BackupConfigManager.BackupConfigError.MissingBackupFolder => BackupConfigError.MissingBackupFolder, 
			BackupConfigManager.BackupConfigError.LowOnDiskSpace => BackupConfigError.LowOnDiskSpace, 
			BackupConfigManager.BackupConfigError.SqlBackupError => BackupConfigError.SqlBackupError, 
			BackupConfigManager.BackupConfigError.SqlAuthenticationError => BackupConfigError.SqlAuthenticationError, 
			BackupConfigManager.BackupConfigError.FileIOError => BackupConfigError.FileIOError, 
			BackupConfigManager.BackupConfigError.SqlNetworkError => BackupConfigError.SqlNetworkError, 
			_ => throw new ArgumentException("value not supported.", "value"), 
		};
	}
}

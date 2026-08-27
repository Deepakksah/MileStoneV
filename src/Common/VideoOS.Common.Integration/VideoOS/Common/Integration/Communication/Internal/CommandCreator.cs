using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Communication.Internal;

public abstract class CommandCreator
{
	public static DatabaseOperationCommand CreateDatabaseOperationCommand(DatabaseOperation databaseOperationType, Guid recorderId, Guid storageId, Guid archiveId, string path, bool newPath, string archiveDefinition, Guid deviceId, Guid[] deviceIds, Guid[] trackIds, bool recursive)
	{
		return new DatabaseOperationCommand(databaseOperationType, recorderId, storageId, archiveId, path, newPath, archiveDefinition, deviceId, deviceIds, trackIds, recursive);
	}

	public static ExecuteRuleActionsCommand CreateExecuteRuleActionsCommand(Guid sourceId, RuleActionExecutionRequest[] requests, DateTime senderTimeUtc)
	{
		return new ExecuteRuleActionsCommand(sourceId, requests, senderTimeUtc);
	}

	public static ServerFileSystemOperationCommand CreateServerFileSystemOperationCommand(Guid serverId, ServerFileSystemServerType serverType, ServerFileSystemOperation operation, string path)
	{
		return new ServerFileSystemOperationCommand(serverId, serverType, operation, path);
	}
}

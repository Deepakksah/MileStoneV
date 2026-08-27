using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server.CommandHandler;

public class ActivateArchivingCommandHandler : ICommandHandler
{
	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ActivateArchivingCommand activateArchivingCommand))
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
		Guid guid;
		try
		{
			guid = ConfigurationDataManager.FindRecordingServerServiceIdByArchiveStorageId(activateArchivingCommand.ArchiveStorageId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception exception)
		{
			responseSink.AddResponse(new ExceptionResponse(exception));
			return;
		}
		Server.Instance.Logger.LogInformation("Activate archiving on archive. Archive id: {ArchiveId}; Recorder service id: {ServiceId}", activateArchivingCommand.ArchiveStorageId, guid);
		Server.Instance.RecorderCommManager.SendActivateArchivingCommand(guid, activateArchivingCommand, callContextInfo);
		responseSink.AddResponse(new SuccessResponse());
	}
}

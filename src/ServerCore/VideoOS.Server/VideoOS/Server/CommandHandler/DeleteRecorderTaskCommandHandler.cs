using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class DeleteRecorderTaskCommandHandler : ICommandHandler
{
	private static class DeleteConfigurationTaskError
	{
		public const int ConfigurationDatabaseTimedout = 60530;

		public const int DeleteRecorderUnknownError = 60531;
	}

	private IServerEnvironment serverEnvironment;

	private ILogger Logger => serverEnvironment.Server.Logger;

	public DeleteRecorderTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		this.serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is DeleteRecorderTaskCommand { CallContextInfo: var callContextInfo } deleteRecorderTaskCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (callContextInfo == null)
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "command.CallContextInfo");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ProcessCommand(deleteRecorderTaskCommand, responseSink, callContextInfo);
	}

	private void ProcessCommand(DeleteRecorderTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int num = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, num++));
		DeleteConfigurationItemCommand deleteConfigurationItemCommand = new DeleteConfigurationItemCommand(new string[1] { cmd.RecordingServerId.ToString() }, new string[0], ConfigurationManager.RecordersDataTable.ToString());
		deleteConfigurationItemCommand.UserAcceptedToDeleteEvidenceLockedData = cmd.UserAcceptedToDeleteMarkedData;
		IResponseCollector responseCollector = serverEnvironment.LocalCommandProcessor.ProcessCommandAsync(deleteConfigurationItemCommand, callContextInfo);
		ResponseBase responseBase;
		while (true)
		{
			try
			{
				responseBase = responseCollector.GetNextResponse(TimeSpan.FromSeconds(60.0));
			}
			catch (TimeoutException)
			{
				responseBase = null;
			}
			num++;
			if (!(responseBase is HeartBeatResponse))
			{
				break;
			}
			if (num < 100)
			{
				responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
			}
		}
		Logger.LogInformation("Delete recorder task - response received. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.GetType().Name);
		if (responseBase is SuccessResponse)
		{
			DeleteRecorderTaskResult deleteRecorderTaskResult = new DeleteRecorderTaskResult();
			deleteRecorderTaskResult.RecorderId = cmd.RecordingServerId;
			deleteRecorderTaskResult.RecorderName = cmd.RecordingServerName;
			responseSink.AddResponse(new TaskSuccessResponse(deleteRecorderTaskResult.ToXml()));
			return;
		}
		if (responseBase is TimeoutResponse)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 60530;
			taskError.ErrorText = VmoResources.VMO60530;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
			return;
		}
		if (responseBase is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to delete recorder. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to delete recorder. [Error: {Exception}]", exceptionResponse.Exception);
			}
		}
		else
		{
			Logger.LogWarning("Failed to delete recorder. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.ToString());
		}
		TaskError taskError2 = new TaskError();
		taskError2.ErrorCode = 60531;
		taskError2.ErrorText = VmoResources.VMO60531;
		responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2));
	}
}

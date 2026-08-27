using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class DeleteHardwareTaskCommandHandler : ICommandHandler
{
	private IServerEnvironment serverEnvironment;

	private ILogger Logger => serverEnvironment.Server.Logger;

	public DeleteHardwareTaskCommandHandler(IServerEnvironment serverEnvironment)
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
		if (!(command is DeleteHardwareTaskCommand { CallContextInfo: var callContextInfo } deleteHardwareTaskCommand))
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
		ProcessCommand(deleteHardwareTaskCommand, responseSink, callContextInfo);
	}

	private void ProcessCommand(DeleteHardwareTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int num = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
		DeleteConfigurationItemCommand command = new DeleteConfigurationItemCommand(new string[1] { cmd.HardwareId.ToString() }, new string[1] { cmd.RecordingServerId.ToString() }, ConfigurationManager.HardwareDataTable.ToString());
		IResponseCollector responseCollector = serverEnvironment.LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
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
		Logger.LogInformation("Delete hardware task - response received. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.GetType().Name);
		if (responseBase is SuccessResponse)
		{
			DeleteHardwareTaskResult deleteHardwareTaskResult = new DeleteHardwareTaskResult();
			deleteHardwareTaskResult.HardwareId = cmd.HardwareId;
			deleteHardwareTaskResult.RecordingServerId = cmd.RecordingServerId;
			deleteHardwareTaskResult.HardwareName = cmd.HardwareName;
			responseSink.AddResponse(new TaskSuccessResponse(deleteHardwareTaskResult.ToXml()));
			return;
		}
		if (responseBase is TimeoutResponse)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 60634;
			taskError.ErrorText = VmoResources.VMO60650;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
			return;
		}
		if (responseBase is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to delete hardware. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to delete hardware. [Error: {Exception}]", exceptionResponse.Exception);
			}
		}
		else
		{
			Logger.LogWarning("Failed to delete hardware. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.ToString());
		}
		TaskError taskError2 = new TaskError();
		taskError2.ErrorCode = 60630;
		taskError2.ErrorText = VmoResources.VMO60651;
		responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2));
	}
}

using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class FirmwareUpgradeTaskCommandHandler : ICommandHandler
{
	private class UploadToRsProgress : IProgress<int>
	{
		private IResponseSink _responseSink;

		private int _lastProgress;

		private DateTime _taskStartTime;

		private int _percentageOfTotalProgress;

		private int _startingProgress;

		public UploadToRsProgress(IResponseSink sink, DateTime startTime, int percentageOfTotalProgress, int startingProgress)
		{
			_responseSink = sink;
			_taskStartTime = startTime;
			_percentageOfTotalProgress = percentageOfTotalProgress;
			_startingProgress = startingProgress;
		}

		public void Report(int value)
		{
			if (value != _lastProgress)
			{
				_responseSink.AddResponse(new TaskProgressResponse(_taskStartTime, _startingProgress + _percentageOfTotalProgress * value / 100));
				_lastProgress = value;
			}
		}
	}

	private readonly IServerEnvironment _serverEnvironment;

	private IFileUploadSynchronizer UploadSynchronizer => _serverEnvironment.FileUploadSynchronizer;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	private IMessageManager MessageManager => _serverEnvironment.MessageManager;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	public FirmwareUpgradeTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is FirmwareUpgradeTaskCommand { CallContextInfo: var callContextInfo } firmwareUpgradeTaskCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (callContextInfo == null)
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		TaskResponseBase response = ProcessCommand(firmwareUpgradeTaskCommand, responseSink, callContextInfo);
		responseSink.AddResponse(response);
	}

	private TaskResponseBase ProcessCommand(FirmwareUpgradeTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		if (!RecorderCommManager.IsRecorderNotDisconnected(cmd.RecordingServerId))
		{
			Logger.LogError("Firmware Upgrade task - Recorder not available. Recorder Id: {ServerId}", cmd.RecordingServerId);
			return CreateTaskFailedResponse(66108, VmoResources.VMO66108, cmd.HardwareName);
		}
		if (RecorderCommManager.IsRecorderIncompatible(cmd.RecordingServerId))
		{
			Logger.LogError("Firmware Upgrade task - Recorder is running unsupported version. Recorder Id: {ServerId}", cmd.RecordingServerId);
			return CreateTaskFailedResponse(66108, VmoResources.VMO66108, cmd.HardwareName);
		}
		int percentComplete = 10;
		if (!ExecuteUploadFileToRecorder(cmd, callContextInfo, responseSink, utcNow))
		{
			return CreateTaskFailedResponse(66109, VmoResources.VMO66109, cmd.HardwareName);
		}
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		FirmwareUpgradeCommand command = new FirmwareUpgradeCommand(cmd.HardwareId, cmd.FileId);
		try
		{
			using IMessageManagerResponseCollector responseCollector = MessageManager.SendCommand(cmd.RecordingServerId, command, callContextInfo);
			try
			{
				return ProcessResponses(cmd, responseCollector, responseSink, utcNow);
			}
			catch (TimeoutException)
			{
				Logger.LogError("Firmware Upgrade task - timeout while waiting for a response.");
				return CreateTaskFailedResponse(60144, VmoResources.VMO60144, cmd.HardwareName);
			}
		}
		catch (RecorderNotFoundException ex2)
		{
			Logger.LogError("Recorder not found while updating firmware: {ExceptionMessage}", ex2.Message);
			return CreateTaskFailedResponse(66108, VmoResources.VMO66108, cmd.HardwareName);
		}
		catch (NotSupportedByLegacyRecorderException ex3)
		{
			Logger.LogError("Legacy Recorder used while updating firmware: {ExceptionMessage}", ex3.Message);
			return CreateTaskFailedResponse(66108, VmoResources.VMO66108, cmd.HardwareName);
		}
	}

	private bool ExecuteUploadFileToRecorder(FirmwareUpgradeTaskCommand command, object state, IResponseSink responseSink, DateTime startTime)
	{
		if (command == null)
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		ICallContextInfo callContextInfo = state as ICallContextInfo;
		if (callContextInfo == null)
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		if (string.IsNullOrEmpty(command.FileId))
		{
			_serverEnvironment.Server.Logger.LogWarning("Invalid file ID");
			return false;
		}
		if (command.RecordingServerId == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("Invalid recorder ID");
			return false;
		}
		if (!_serverEnvironment.RecorderCommManager.IsRecorderAttachedAndConnected(command.RecordingServerId))
		{
			_serverEnvironment.Server.Logger.LogWarning("Recorder unavailable");
			responseSink.AddResponse(new RecorderUnavailableResponse(command.RecordingServerId));
			return false;
		}
		if (UploadSynchronizer.Upload(command.RecordingServerId, command.FileId, TimeSpan.FromMinutes(10.0), () => FileExistsOnRecorder(callContextInfo, command), () => UploadFileToRecorder(responseSink, callContextInfo, startTime, command)))
		{
			responseSink.AddResponse(new FileUploadResponse(command.FileId));
			return true;
		}
		return false;
	}

	private TaskResponseBase ProcessResponses(FirmwareUpgradeTaskCommand cmd, IMessageManagerResponseCollector responseCollector, IResponseSink responseSink, DateTime startTime)
	{
		ResponseBase responseBase;
		ProgressResponse progressResponse;
		do
		{
			try
			{
				responseBase = responseCollector.GetNextResponse(TimeSpan.FromSeconds(95.0));
			}
			catch (TimeoutException)
			{
				responseBase = new TimeoutResponse();
			}
			progressResponse = responseBase as ProgressResponse;
			if (progressResponse != null)
			{
				int percentComplete = (int)(30f + 0.7f * (float)progressResponse.Progress);
				responseSink.AddResponse(new TaskProgressResponse(startTime, percentComplete));
			}
		}
		while (progressResponse != null);
		if (!(responseBase is ErrorResponse errorResponse))
		{
			if (!(responseBase is WarningResponse warningResponse))
			{
				if (!(responseBase is FirmwareUpgradeSuccessResponse successResponse))
				{
					if (responseBase is TimeoutResponse)
					{
						return CreateTaskFailedResponse(60144, string.Empty);
					}
					return HandleUnexpectedError(cmd, responseSink, startTime);
				}
				return HandleSuccessResponse(cmd, responseSink, startTime, successResponse);
			}
			return HandleWarningResponse(cmd, responseSink, startTime, warningResponse);
		}
		return HandleErrorResponse(cmd, responseSink, startTime, errorResponse);
	}

	private TaskFailedResponse HandleErrorResponse(FirmwareUpgradeTaskCommand cmd, IResponseSink responseSink, DateTime startTime, ErrorResponse errorResponse)
	{
		Logger.LogError("Firmware Upgrade command for Hardware Id: {HardwareId} failed. Error: {ErrorMessage}", cmd.HardwareId, errorResponse.Message);
		responseSink.AddResponse(new TaskProgressResponse(startTime, 100));
		return GetFailedResponse(cmd, errorResponse);
	}

	private TaskResponseBase HandleUnexpectedError(FirmwareUpgradeTaskCommand cmd, IResponseSink responseSink, DateTime startTime)
	{
		Logger.LogWarning("Firmware Upgrade command failed for unknown reason.");
		responseSink.AddResponse(new TaskProgressResponse(startTime, 100));
		return CreateTaskFailedResponse(66108, VmoResources.VMO66108, cmd.HardwareName);
	}

	private TaskResponseBase HandleWarningResponse(FirmwareUpgradeTaskCommand cmd, IResponseSink responseSink, DateTime startTime, WarningResponse warningResponse)
	{
		Logger.LogInformation("Firmware Upgrade Succeeded with a warning. HardwareId: {HardwareId}", cmd.HardwareId);
		responseSink.AddResponse(new TaskProgressResponse(startTime, 100));
		FirmwareUpgradeTaskResult firmwareUpgradeTaskResult = new FirmwareUpgradeTaskResult
		{
			NewFirmwareVersion = warningResponse.Message,
			WarningCode = 66107,
			WarningMessage = VmoResources.VMO66107,
			HardwareId = cmd.HardwareId,
			HardwareName = cmd.HardwareName,
			RecordingServerId = cmd.RecordingServerId
		};
		return new TaskSuccessResponse(firmwareUpgradeTaskResult.ToXml());
	}

	private TaskResponseBase HandleSuccessResponse(FirmwareUpgradeTaskCommand cmd, IResponseSink responseSink, DateTime startTime, FirmwareUpgradeSuccessResponse successResponse)
	{
		Logger.LogInformation("Firmware Upgrade Succeeded. HardwareId: {HardwareId}", cmd.HardwareId);
		responseSink.AddResponse(new TaskProgressResponse(startTime, 100));
		FirmwareUpgradeTaskResult firmwareUpgradeTaskResult = new FirmwareUpgradeTaskResult
		{
			NewFirmwareVersion = successResponse.NewFirmwareVersion,
			HardwareId = cmd.HardwareId,
			HardwareName = cmd.HardwareName,
			RecordingServerId = cmd.RecordingServerId
		};
		return new TaskSuccessResponse(firmwareUpgradeTaskResult.ToXml());
	}

	private static TaskFailedResponse GetFailedResponse(FirmwareUpgradeTaskCommand cmd, ErrorResponse errorResponse)
	{
		return errorResponse.ErrorCode switch
		{
			21 => CreateTaskFailedResponse(66101, VmoResources.VMO66101, cmd.HardwareName), 
			22 => CreateTaskFailedResponse(66102, VmoResources.VMO66102, cmd.HardwareName), 
			23 => CreateTaskFailedResponse(66103, VmoResources.VMO66103, cmd.HardwareName), 
			24 => CreateTaskFailedResponse(66104, VmoResources.VMO66104, cmd.HardwareName), 
			25 => CreateTaskFailedResponse(66105, VmoResources.VMO66105, cmd.HardwareName), 
			26 => CreateTaskFailedResponse(66106, VmoResources.VMO66106, cmd.HardwareName), 
			_ => CreateTaskFailedResponse(66108, VmoResources.VMO66108, cmd.HardwareName), 
		};
	}

	private static TaskFailedResponse CreateTaskFailedResponse(int errorCode, string errorText, params string[] arguments)
	{
		TaskError taskError = new TaskError
		{
			ErrorCode = errorCode,
			ErrorText = errorText
		};
		for (int i = 0; i < arguments.Length; i++)
		{
			taskError.Arguments.Add(i, arguments[i]);
		}
		return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
	}

	private bool FileExistsOnRecorder(ICallContextInfo callContextInfo, FirmwareUpgradeTaskCommand command)
	{
		CheckFilePresentOnRecorderCommand command2 = new CheckFilePresentOnRecorderCommand(command.FileId);
		IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(command.RecordingServerId, command2, callContextInfo);
		ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(10.0));
		if (nextResponse is BoolResponse boolResponse)
		{
			return boolResponse.Value;
		}
		return false;
	}

	private bool UploadFileToRecorder(IResponseSink responseSink, ICallContextInfo callContextInfo, DateTime startTime, FirmwareUpgradeTaskCommand command)
	{
		UploadToRsProgress uploadProgress = new UploadToRsProgress(responseSink, startTime, 10, 20);
		IServerFileStorageEntry storage = _serverEnvironment.ServerFileStorage.GetStorage(command.FileId);
		FileUploadCommand command2 = new FileUploadCommand(storage, uploadProgress);
		try
		{
			_serverEnvironment.ServerLogManager.AuditLogger.AuditFileUploadToRecorder(command.FileId, GetRecorderName(command.RecordingServerId), callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
			using IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(command.RecordingServerId, command2, callContextInfo);
			ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromMinutes(10.0));
			if (nextResponse is FileUploadResponse)
			{
				LogFileUploaded(command.FileId, command.RecordingServerId, success: true);
				return true;
			}
			LogFileUploaded(command.FileId, command.RecordingServerId, success: false);
			return false;
		}
		catch (RecorderNotFoundException)
		{
			_serverEnvironment.Server.Logger.LogError("Recorder unavailable");
			responseSink.AddResponse(new RecorderUnavailableResponse(command.RecordingServerId));
			return false;
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			_serverEnvironment.Server.Logger.LogError("Legacy recorder unsupported");
			responseSink.AddResponse(new RecorderNotSupportedResponse(command.RecordingServerId));
			return false;
		}
		catch (TimeoutException)
		{
			Logger.LogError("Firmware Upgrade task - timeout while waiting for a response for file upload to Recording Server.");
			return false;
		}
	}

	private string GetRecorderName(Guid recorderId)
	{
		return _serverEnvironment.ServerConfigurationLookup.FindRecorderDisplayName(recorderId);
	}

	private void LogFileUploaded(string fileName, Guid recorderId, bool success)
	{
		_serverEnvironment.ServerLogManager.AddLogEntry(success ? LogEntryId.FileTransferredToRecorder : LogEntryId.FileTransferToRecorderFailed, fileName, GetRecorderName(recorderId));
	}
}

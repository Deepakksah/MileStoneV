using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class FileUploadToRecorderCommandHandler : ICommandHandler
{
	private class UploadProgress : IProgress<int>
	{
		private IResponseSink _responseSink;

		private int _lastProgress;

		public UploadProgress(IResponseSink sink)
		{
			_responseSink = sink;
		}

		public void Report(int value)
		{
			if (value != _lastProgress)
			{
				_responseSink.AddResponse(new ProgressResponse(value));
				_lastProgress = value;
			}
		}
	}

	private readonly IServerEnvironment _serverEnvironment;

	public FileUploadToRecorderCommandHandler(IServerEnvironment serverEnvironment)
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
		if (!(command is FileUploadToRecorderCommand fileUploadToRecorderCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		if (string.IsNullOrEmpty(fileUploadToRecorderCommand.FileId))
		{
			_serverEnvironment.Server.Logger.LogWarning("Invalid file ID");
			responseSink.AddResponse(new FailResponse());
			return;
		}
		if (fileUploadToRecorderCommand.RecorderId == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("Invalid recorder ID");
			responseSink.AddResponse(new FailResponse());
			return;
		}
		if (!_serverEnvironment.RecorderCommManager.IsRecorderAttachedAndConnected(fileUploadToRecorderCommand.RecorderId))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(fileUploadToRecorderCommand.RecorderId));
			return;
		}
		if (FileIsAlreadyPresentOnRecorder(fileUploadToRecorderCommand.RecorderId, fileUploadToRecorderCommand.FileId, callContextInfo))
		{
			responseSink.AddResponse(new FileUploadResponse(fileUploadToRecorderCommand.FileId));
			return;
		}
		UploadProgress uploadProgress = new UploadProgress(responseSink);
		IServerFileStorageEntry storage = _serverEnvironment.ServerFileStorage.GetStorage(fileUploadToRecorderCommand.FileId);
		FileUploadCommand command2 = new FileUploadCommand(storage, uploadProgress);
		try
		{
			CreateFileUploadToRecorderAuditLog(fileUploadToRecorderCommand.FileId, fileUploadToRecorderCommand.RecorderId, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
			IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(fileUploadToRecorderCommand.RecorderId, command2, callContextInfo);
			ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(60.0));
			responseSink.AddResponse(nextResponse);
			if (nextResponse is FileUploadResponse fileUploadResponse)
			{
				LogFileUploaded(fileUploadResponse.StorageId.ToString(), fileUploadToRecorderCommand.RecorderId);
			}
			else
			{
				LogFileUploadFailed(fileUploadToRecorderCommand.FileId.ToString(), fileUploadToRecorderCommand.RecorderId);
			}
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(fileUploadToRecorderCommand.RecorderId));
			LogFileUploadFailed(fileUploadToRecorderCommand.FileId.ToString(), fileUploadToRecorderCommand.RecorderId);
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(fileUploadToRecorderCommand.RecorderId));
			LogFileUploadFailed(fileUploadToRecorderCommand.FileId.ToString(), fileUploadToRecorderCommand.RecorderId);
		}
		catch (Exception)
		{
			responseSink.AddResponse(new ErrorResponse(5, "Error transferring file."));
			LogFileUploadFailed(fileUploadToRecorderCommand.FileId.ToString(), fileUploadToRecorderCommand.RecorderId);
			throw;
		}
	}

	private bool FileIsAlreadyPresentOnRecorder(Guid targetRecorderId, string fileId, ICallContextInfo context)
	{
		CheckFilePresentOnRecorderCommand command = new CheckFilePresentOnRecorderCommand(fileId);
		IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(targetRecorderId, command, context);
		ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(10.0));
		if (nextResponse is BoolResponse boolResponse)
		{
			return boolResponse.Value;
		}
		return false;
	}

	private string GetRecorderName(Guid recorderId)
	{
		return _serverEnvironment.ServerConfigurationLookup.FindRecorderDisplayName(recorderId);
	}

	private void CreateFileUploadToRecorderAuditLog(string fileId, Guid recorderId, string userCredentials, string locationIpAddress)
	{
		LogEntryAudit logEntryAudit = LogEntryAuditFactory.AuditFileTransferToRecorderStarted(fileId, GetRecorderName(recorderId), userCredentials, locationIpAddress);
		ServerLogManager.Instance.AddAuditLogEntry(logEntryAudit);
	}

	private void LogFileUploaded(string filenameName, Guid recorderId)
	{
		_serverEnvironment.ServerLogManager.AddLogEntry(LogEntryId.FileTransferredToRecorder, filenameName, GetRecorderName(recorderId));
	}

	private void LogFileUploadFailed(string filenameName, Guid recorderId)
	{
		_serverEnvironment.ServerLogManager.AddLogEntry(LogEntryId.FileTransferToRecorderFailed, filenameName, GetRecorderName(recorderId));
	}
}

using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Extensions;

namespace VideoOS.Server.CommandHandler;

public sealed class UploadFileChunkCommandHandler : ICommandHandler
{
	private IServer Server => ServerEnvironment.Server;

	private IServerEnvironment ServerEnvironment { get; }

	private ILogAuditLogger AuditLogger { get; }

	private IServerLogManager ServerLogManager { get; }

	public UploadFileChunkCommandHandler(IServerEnvironment serverEnvironment, ILogAuditLogger auditLogger)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		if (auditLogger == null)
		{
			throw new ArgumentNullException("auditLogger");
		}
		ServerEnvironment = serverEnvironment;
		AuditLogger = auditLogger;
		ServerLogManager = serverEnvironment.ServerLogManager;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UploadFileChunkCommand uploadFileChunkCommand))
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
		if (uploadFileChunkCommand.TransferId == Guid.Empty)
		{
			throw new ArgumentException("A transfer ID was not passed to the command.");
		}
		if (uploadFileChunkCommand.TotalSizeInBytes <= 0)
		{
			throw new ArgumentException("Total size must be a positive value.");
		}
		if (uploadFileChunkCommand.OffsetInBytes < 0)
		{
			throw new ArgumentException("Offset must be 0 or larger.");
		}
		byte[] array = uploadFileChunkCommand.ChunkBuffer.ToArray();
		if (uploadFileChunkCommand.OffsetInBytes + array.Length > uploadFileChunkCommand.TotalSizeInBytes)
		{
			throw new ArgumentException("Chunk size was larger than expected.");
		}
		StreamWithStorage streamWithStorage = null;
		try
		{
			if (uploadFileChunkCommand.OffsetInBytes == 0)
			{
				UploadFileChunkAuditLog(uploadFileChunkCommand.PermanentId, uploadFileChunkCommand.TotalSizeInBytes, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
				IServerFileStorageEntry storage = ServerEnvironment.ServerFileStorage.GetStorage(uploadFileChunkCommand.PermanentId);
				if (storage != null)
				{
					responseSink.AddResponse(new FileUploadResponse(storage.Id));
					return;
				}
			}
			streamWithStorage = Server.ServerStorageManager.GetStreamWithId(uploadFileChunkCommand.TransferId);
			streamWithStorage.FileStream.Write(array, 0, array.Length);
			int num = uploadFileChunkCommand.OffsetInBytes + array.Length;
			if (uploadFileChunkCommand.TotalSizeInBytes != num)
			{
				responseSink.AddResponse(new SuccessResponse());
				return;
			}
			byte[] sHA256Hash = streamWithStorage.FileStream.GetSHA256Hash();
			Server.ServerStorageManager.ReturnStream(uploadFileChunkCommand.TransferId);
			if (!uploadFileChunkCommand.FileChecksum.SequenceEqual(sHA256Hash))
			{
				IServerFileStorageEntry storage2 = streamWithStorage.Storage;
				ServerEnvironment.ServerFileStorage.ReturnStorage(storage2);
				Server.Logger.LogError("(UploadFileChunkCommand) Checksum did not match");
				responseSink.AddResponse(new ErrorResponse(0, "File checksum mismatch."));
				LogFileUploadFailed(uploadFileChunkCommand.PermanentId, uploadFileChunkCommand.TotalSizeInBytes);
			}
			else
			{
				LogFileUploaded(uploadFileChunkCommand.PermanentId, uploadFileChunkCommand.TotalSizeInBytes);
				streamWithStorage.Storage.Persist(uploadFileChunkCommand.PermanentId);
				responseSink.AddResponse(new FileUploadResponse(streamWithStorage.Storage.Id));
			}
		}
		catch (Exception ex)
		{
			if (!HandleException(ex, uploadFileChunkCommand, responseSink, streamWithStorage))
			{
				throw;
			}
		}
	}

	private bool HandleException(Exception ex, UploadFileChunkCommand command, IResponseSink responseSink, StreamWithStorage streamWithId)
	{
		LogFileUploadFailed(command.TransferId.ToString(), command.TotalSizeInBytes);
		Server.ServerStorageManager.ReturnStream(command.TransferId);
		if (streamWithId != null)
		{
			ServerEnvironment.ServerFileStorage.ReturnStorage(streamWithId.Storage);
		}
		if (!(ex is ObjectDisposedException))
		{
			if (!(ex is StorageManagerCannotCreateStorageException))
			{
				if (!(ex is StorageManagerCannotOpenStorageException))
				{
					if (!(ex is StorageQuotaExceededException))
					{
						Server.Logger.LogError("(UploadFileChunkCommand) Unknown error. Exception: {ExceptionMessage}", ex.Message);
						responseSink.AddResponse(new ErrorResponse(5, "Unknown error."));
						return false;
					}
					Server.Logger.LogError("(UploadFileChunkCommand) Storage quota exceeded on the server. Exception: {ExceptionMessage}", ex.Message);
					responseSink.AddResponse(new ErrorResponse(4, "Storage quota was exceeded."));
				}
				else
				{
					Server.Logger.LogError("(UploadFileChunkCommand) Failed to open storage on the server. Exception: {ExceptionMessage}", ex.Message);
					responseSink.AddResponse(new ErrorResponse(3, "Cannot open file storage."));
				}
			}
			else
			{
				Server.Logger.LogError("(UploadFileChunkCommand) Failed to create storage on the server. Exception: {ExceptionMessage}", ex.Message);
				responseSink.AddResponse(new ErrorResponse(2, "Cannot create file storage."));
			}
		}
		else
		{
			Server.Logger.LogError("(UploadFileChunkCommand) Failed to create storage on the server Object disposed. Exception: {ExceptionMessage}", ex.Message);
			responseSink.AddResponse(new ErrorResponse(1, "File storage was disposed."));
		}
		return true;
	}

	private void UploadFileChunkAuditLog(string targetFileName, long size, string userCredentials, string locationIpAddress)
	{
		LogEntryAudit logEntryAudit = LogEntryAuditFactory.AuditFileUploadStarted(targetFileName, size.ToString(CultureInfo.InvariantCulture), OemResources.Instance.Manager.ApplicationNameShort, userCredentials, locationIpAddress);
		AuditLogger.AddAuditLogEntry(logEntryAudit);
	}

	private void LogFileUploaded(string filenameName, long fileSize)
	{
		ServerLogManager.AddLogEntry(LogEntryId.FileUploadedToServer, filenameName, fileSize.ToString(CultureInfo.InvariantCulture));
	}

	private void LogFileUploadFailed(string filenameName, long fileSize)
	{
		ServerLogManager.AddLogEntry(LogEntryId.FileUploadToServerFailed, filenameName, fileSize.ToString(CultureInfo.InvariantCulture));
	}
}

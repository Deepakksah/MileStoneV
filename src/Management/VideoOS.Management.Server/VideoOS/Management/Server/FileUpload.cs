using System;
using System.Globalization;
using System.ServiceModel;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public static class FileUpload
{
	public class FileUploadResult
	{
		public string StorageId { get; set; }
	}

	public static FileUploadResult UploadFileChunk(Guid transferId, byte[] chunkData, int offset, long fileSize, byte[] fileChecksum)
	{
		using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
		ResponseBase responseBase = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.UploadFileChunk(transferId, chunkData, offset, fileSize, fileChecksum), null, delegate(Exception ex, Action ac)
		{
			GeminiTrace.Error(TraceType.General, "Transferring chunk failed: UploadFileChunk(transferId:'{0}')", transferId.ToString());
			return UploadFileChunkErrorHandler(ex);
		});
		if (responseBase is FileUploadResponse fileUploadResponse)
		{
			return new FileUploadResult
			{
				StorageId = fileUploadResponse.StorageId
			};
		}
		if (responseBase is SuccessResponse)
		{
			return new FileUploadResult();
		}
		throw new FileUploadUnknownException();
	}

	private static bool UploadFileChunkErrorHandler(Exception ex)
	{
		if (ex is FaultException)
		{
			if (ex is FaultException<ErrorResponseFault> ex2)
			{
				switch (ex2.Detail.ErrorNumber)
				{
				case 0:
					throw new FileUploadInvalidChecksumException();
				case 1:
					throw new FileUploadFileDisposedException();
				case 2:
					throw new FileUploadCannotCreateFileException();
				case 3:
					throw new FileUploadCannotOpenFileException();
				case 4:
					throw new FileUploadQuotaExceededException();
				case 5:
					throw new FileUploadUnknownException();
				}
			}
			GeminiTrace.Error(TraceType.General, "Chunk transfer failed");
			GeminiTrace.TraceException(ex);
			throw new FileUploadUnknownException(ex.Message, ex);
		}
		if (ex.GetType() == typeof(CommunicationException) || ex.GetType() == typeof(EndpointNotFoundException))
		{
			throw new ManagementServerServiceUnavailableException(ex.Message, ex);
		}
		GeminiTrace.Error(TraceType.General, "Chunk transfer failed");
		GeminiTrace.TraceException(ex);
		throw new FileUploadUnknownException(ex.Message, ex);
	}
}

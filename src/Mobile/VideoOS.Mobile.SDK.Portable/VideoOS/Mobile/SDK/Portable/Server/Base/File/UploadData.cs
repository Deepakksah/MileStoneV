using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.File;

public class UploadData : IDisposable, IDisposableQuiet
{
	private class RequestStreamParams
	{
		public HttpWebRequest Request { get; set; }

		public byte[] Data { get; set; }

		public EventWaitHandle AllDone { get; set; }
	}

	private bool _disposed;

	private Stream _stream;

	private readonly VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection _connection;

	private readonly string _webAddress;

	private readonly TimeSpan _maxTransmitionDelay = new TimeSpan(0, 2, 0);

	private readonly Guid _streamId;

	private readonly Guid _exportId;

	private int _chunkNumber;

	private bool _canceled;

	private bool _sendingLastChunk;

	public event Action UploadFailed;

	public event Action<double> ProgressChanged;

	public event Action UploadSuccessful;

	public UploadData(RequestFootageStreamResponse response, VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection connection)
	{
		_disposed = false;
		_connection = connection;
		IConnectionEndPoint connection2 = _connection;
		_webAddress = ((connection2.ChannelType == ChannelTypes.HTTP) ? "http" : "https") + "://" + connection2.IpAddress + ":" + connection2.Port + "/XProtectMobile/Stream/" + response.StreamId.ToString() + "/";
		_streamId = response.StreamId;
		_exportId = response.ExportId;
	}

	protected void Dispose(bool disposing, bool quiet)
	{
		if (!_disposed)
		{
			_disposed = true;
		}
	}

	public void StartUpload(Stream stream, ConnectionType connectionType)
	{
		_stream = stream;
		int chunkSize = 8192;
		switch (connectionType)
		{
		case ConnectionType.EDGE:
			chunkSize = 16384;
			break;
		case ConnectionType.ThreeG:
			chunkSize = 32768;
			break;
		case ConnectionType.HSDPA:
			chunkSize = 65536;
			break;
		case ConnectionType.HSPAPlus:
			chunkSize = 131072;
			break;
		case ConnectionType.LTE:
			chunkSize = 262144;
			break;
		case ConnectionType.Wireless:
			chunkSize = 524288;
			break;
		}
		if (stream == null || stream.Length <= 0)
		{
			return;
		}
		byte[] buffer = new byte[chunkSize];
		int totalBytesSend = 0;
		int bytesRead = 0;
		Task.Factory.StartNew(delegate
		{
			_chunkNumber = 0;
			while (!_canceled && (bytesRead = _stream.Read(buffer, _chunkNumber * chunkSize, chunkSize)) > 0)
			{
				try
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_webAddress);
					httpWebRequest.Method = "POST";
					httpWebRequest.ContentType = "application/octet-stream";
					RequestStreamParams requestStreamParams = new RequestStreamParams
					{
						Request = httpWebRequest,
						Data = buffer.Take(bytesRead).ToArray(),
						AllDone = new ManualResetEvent(initialState: false)
					};
					if (totalBytesSend + bytesRead >= stream.Length)
					{
						_sendingLastChunk = true;
					}
					httpWebRequest.BeginGetRequestStream(OnGetRequestStream, requestStreamParams);
					if (requestStreamParams.AllDone != null)
					{
						if (requestStreamParams.AllDone.WaitOne(_maxTransmitionDelay))
						{
							totalBytesSend += bytesRead;
							if (this.ProgressChanged != null)
							{
								this.ProgressChanged((double)totalBytesSend / (double)_stream.Length * 100.0);
							}
						}
						else
						{
							OnError(null);
						}
						requestStreamParams.AllDone.Dispose();
					}
				}
				catch (Exception ex)
				{
					Logger.Instance.Log(LogContent.Error, "UploadData.StartUpload.Thread", ex.Message);
					OnError(ex);
				}
			}
		});
	}

	public bool CancelUpload()
	{
		_canceled = true;
		if (_connection.ServerFeatures.SupportsInvestigations)
		{
			return _connection.Investigations.DeleteInvestigation(_exportId, TimeSpan.FromSeconds(30.0)).ErrorCode == ErrorCodes.Ok;
		}
		return false;
	}

	private void OnGetRequestStream(IAsyncResult asyncResultRequest)
	{
		try
		{
			RequestStreamParams requestStreamParams = (RequestStreamParams)asyncResultRequest.AsyncState;
			HttpWebRequest request = requestStreamParams.Request;
			byte[] data = requestStreamParams.Data;
			_ = requestStreamParams.AllDone;
			Stream stream = request.EndGetRequestStream(asyncResultRequest);
			MainHeader tObject = new MainHeader
			{
				VideoID = _streamId,
				FrameCount = (uint)_chunkNumber,
				FrameSizeBytes = (uint)data.Length,
				TimeStampUtcMs = (ulong)DateTime.UtcNow.ToLong(),
				HeaderSizeBytes = 36,
				HeaderExtensionFlags = 0
			};
			byte[] array = BinaryFrameHeaderHelper.Instance.WriteStruct(tObject, 36, BinaryFrameInvertor.Invert);
			stream.Write(array, 0, array.Count());
			stream.Write(data, 0, data.Count());
			stream.Dispose();
			request.BeginGetResponse(OnGetResponse, requestStreamParams);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "UploadData.OnGetRequestStream", ex.Message);
			OnError(ex);
		}
	}

	private void OnGetResponse(IAsyncResult asyncResultResponse)
	{
		EventWaitHandle eventWaitHandle = null;
		try
		{
			RequestStreamParams obj = (RequestStreamParams)asyncResultResponse.AsyncState;
			HttpWebRequest request = obj.Request;
			eventWaitHandle = obj.AllDone;
			HttpWebResponse obj2 = (HttpWebResponse)request.EndGetResponse(asyncResultResponse);
			if (obj2.StatusCode == HttpStatusCode.OK)
			{
				OnSuccess(eventWaitHandle);
			}
			else
			{
				OnError(null);
			}
			obj2.Dispose();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "UploadData.OnGetResponse", ex.Message);
			OnError(ex);
		}
	}

	private void OnError(Exception exception)
	{
		if (this.UploadFailed != null && !_canceled)
		{
			this.UploadFailed();
		}
		EndTheTransmision(null);
	}

	private void OnSuccess(EventWaitHandle waitHandle)
	{
		try
		{
			if (_sendingLastChunk && this.UploadSuccessful != null)
			{
				this.UploadSuccessful();
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "UploadData.OnSuccess", ex.Message);
		}
		EndTheTransmision(waitHandle);
	}

	private void EndTheTransmision(EventWaitHandle waitHandle)
	{
		try
		{
			waitHandle?.Set();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "UploadData.EndTheTransmision", ex.Message);
		}
	}

	public void Dispose()
	{
		DisposeInternal(disposing: true, quiet: false);
	}

	private void DisposeInternal(bool disposing, bool quiet)
	{
		Dispose(disposing, quiet);
		GC.SuppressFinalize(this);
	}

	public void DisposeQuiet()
	{
		DisposeInternal(disposing: true, quiet: true);
	}
}

using System;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpStream : IVideo, IVideoSub, IDisposable, IControl
{
	private readonly IRequestStreamResponse _response;

	private readonly string _extension;

	private readonly string _webAddress;

	private readonly ByteBuffer _buffer;

	private readonly byte[] _bytesVideoId;

	private bool _disposed;

	private IClientContext _iClientContext;

	public const int OneMegaByte = 1048576;

	public const int InitialBufferSize = 1048576;

	public const int MaximalBufferSize = 10485760;

	public virtual bool IsSecured => false;

	public OnVideoFrame NewFrame { get; set; }

	public HttpStream(IRequestStreamResponse response, string extension, IConnectionEndPoint endPoint)
	{
		_disposed = false;
		_response = response;
		_extension = extension;
		_iClientContext = ClientCtxFactory.Create(IsSecured);
		_webAddress = _iClientContext.Protocol + endPoint.IpAddress + ":" + endPoint.Port + "/" + _extension + "/" + _response.VideoId.ToString() + "/";
		_buffer = new ByteBuffer(1048576);
		_bytesVideoId = response.VideoId.ToByteArray();
		if (response.ByteOrder == StreamParamsHelper.ByteOrder.Network)
		{
			_bytesVideoId = EndiansInvertor.Invert(response.VideoId).ToByteArray();
		}
	}

	~HttpStream()
	{
		Dispose(disposing: true);
	}

	public void GetNextFrame()
	{
		Run();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_iClientContext = null;
			}
			_disposed = true;
		}
	}

	private void Run()
	{
		if (_disposed)
		{
			return;
		}
		try
		{
			IRequest request = _iClientContext.CreateRequest(_webAddress);
			request.Method = "GET";
			request.GetResponse(GetResponseCallback);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideo.Run", ex.Message);
			OnVideoFrameCallback(null);
		}
	}

	private void GetResponseCallback(IResponse iResponse)
	{
		int extractedCount = 0;
		OnVideoFrame onVideoFrame = delegate(VideoFrame frame)
		{
			OnVideoFrameCallback(frame);
			extractedCount++;
		};
		try
		{
			_buffer.Clear();
			ProcessStreamResponse(iResponse, onVideoFrame);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideo.GetResponseCallback", ex.Message);
			onVideoFrame(null);
		}
		CheckAreFramesExtracted(extractedCount);
	}

	private void ProcessStreamResponse(IResponse streamResponse, OnVideoFrame onVideoFrame)
	{
		if (streamResponse != null)
		{
			ProcessMultipleFrames(streamResponse, _buffer, onVideoFrame);
			FinalizeBuffer(_buffer, onVideoFrame);
		}
	}

	private void ProcessMultipleFrames(IResponse streamResponse, ByteBuffer buffer, OnVideoFrame onVideoFrame)
	{
		int num = 0;
		while ((num = streamResponse.Read(buffer.data, buffer.offset + buffer.lenWritten, buffer.data.Length - (buffer.offset + buffer.lenWritten))) != 0 && !_disposed)
		{
			buffer.lenWritten += num;
			FrameSeparator.ProcessBuffer(buffer, _bytesVideoId, onVideoFrame, _response.ByteOrder == StreamParamsHelper.ByteOrder.Network, OnRequireBiggerBuffer);
			buffer.Compact();
		}
	}

	private void FinalizeBuffer(ByteBuffer buffer, OnVideoFrame onVideoFrame)
	{
		buffer.AddData(_bytesVideoId, 0, _bytesVideoId.Length);
		FrameSeparator.ProcessBuffer(buffer, _bytesVideoId, onVideoFrame, _response.ByteOrder == StreamParamsHelper.ByteOrder.Network);
	}

	private void OnRequireBiggerBuffer(ByteBuffer buffer, uint requiredSize)
	{
		int size = buffer.size;
		int num = (int)requiredSize / size + 1;
		int newSize = System.Math.Min(size * num, 10485760);
		buffer.Resize(newSize);
	}

	private void CheckAreFramesExtracted(int extractedCount)
	{
		if (extractedCount == 0)
		{
			OnVideoFrameCallback(null);
		}
	}

	private void OnVideoFrameCallback(VideoFrame frame)
	{
		if (!_disposed && NewFrame != null)
		{
			NewFrame(frame);
		}
	}

	public void Start()
	{
		Run();
	}

	public void Stop()
	{
	}
}

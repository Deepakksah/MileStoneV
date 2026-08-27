using System;
using System.IO;
using System.Net;
using System.Threading;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpVideoPushBase<T> : IPushStreamSub<T>, IPushVideoFail, IPushVideoSuccess, IPushVideoCounter, IDisposable where T : PushStreamData, new()
{
	private class RequestStreamParams
	{
		public HttpWebRequest Request { get; set; }

		public T Frame { get; set; }

		public EventWaitHandle AllDone { get; set; }
	}

	private readonly RequestStreamResponseUpload _response;

	private readonly string _extension;

	private readonly string _webAddress;

	private readonly BinaryVideoPush<T> _binaryVideoPush;

	private long _totalFailCount;

	private int _consecutiveFailCount;

	private readonly object _counterLocker = new object();

	private readonly TimeSpan _maxTransmitionDelay = new TimeSpan(0, 2, 0);

	private IFpsMeterMeasure _fpsMeterMeasure = new FpsMeterMeasureFake();

	public ParceDeviceState GetDeviceState { get; set; }

	public long TotalFailCount => _totalFailCount;

	public OnFrameCompletion FailCallback { get; set; }

	public OnFrameCompletion SuccessCallback { get; set; }

	public IFpsMeter FpsMeter
	{
		get
		{
			IFpsMeter fpsMeter = _fpsMeterMeasure as IFpsMeter;
			if (fpsMeter == null)
			{
				fpsMeter = (IFpsMeter)(_fpsMeterMeasure = new FpsMeter(new TimeSpan(0, 0, 5)));
			}
			return fpsMeter;
		}
	}

	public HttpVideoPushBase(RequestStreamResponseUpload response, string extension, IConnectionEndPoint endPoint, string protocol)
	{
		_response = response;
		_extension = extension;
		_webAddress = protocol + endPoint.IpAddress + ":" + endPoint.Port + "/" + _extension + "/" + _response.StreamId.ToString() + "/";
		_binaryVideoPush = new BinaryVideoPush<T>(_response.VideoId);
	}

	public void SendFrame(T frame, bool blocking = false)
	{
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_webAddress);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "image/jpeg";
			RequestStreamParams requestStreamParams = new RequestStreamParams
			{
				Request = httpWebRequest,
				Frame = frame,
				AllDone = (blocking ? new ManualResetEvent(initialState: false) : null)
			};
			httpWebRequest.BeginGetRequestStream(OnGetRequestStream, requestStreamParams);
			if (requestStreamParams.AllDone != null)
			{
				requestStreamParams.AllDone.WaitOne(_maxTransmitionDelay);
				requestStreamParams.AllDone.Dispose();
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideoPushBase.Run", ex.Message);
			OnError(ex, null);
		}
	}

	private void OnGetRequestStream(IAsyncResult asyncResultRequest)
	{
		EventWaitHandle waitHandle = null;
		try
		{
			RequestStreamParams requestStreamParams = (RequestStreamParams)asyncResultRequest.AsyncState;
			HttpWebRequest request = requestStreamParams.Request;
			T frame = requestStreamParams.Frame;
			waitHandle = requestStreamParams.AllDone;
			Stream stream = request.EndGetRequestStream(asyncResultRequest);
			_binaryVideoPush.Send(frame, stream, null);
			stream.Dispose();
			request.BeginGetResponse(OnGetResponse, requestStreamParams);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideoPushBase.OnGetRequestStream", ex.Message);
			OnError(ex, waitHandle);
		}
	}

	private void OnGetResponse(IAsyncResult asyncResultResponse)
	{
		EventWaitHandle waitHandle = null;
		try
		{
			RequestStreamParams obj = (RequestStreamParams)asyncResultResponse.AsyncState;
			HttpWebRequest request = obj.Request;
			waitHandle = obj.AllDone;
			HttpWebResponse httpWebResponse = (HttpWebResponse)request.EndGetResponse(asyncResultResponse);
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				GetDeviceState?.Invoke(httpWebResponse.GetResponseStream());
				OnSuccess(waitHandle);
			}
			else
			{
				OnError(null, waitHandle);
			}
			httpWebResponse.Dispose();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideoPushBase.OnGetResponse", ex.Message);
			OnError(ex, waitHandle);
		}
	}

	private void OnError(Exception exception, EventWaitHandle waitHandle)
	{
		lock (_counterLocker)
		{
			_totalFailCount++;
			_consecutiveFailCount++;
		}
		try
		{
			if (FailCallback != null)
			{
				FailCallback(_consecutiveFailCount);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideoPushBase.OnError.FailCallback", ex.Message);
		}
		EndTheTransmision(waitHandle);
	}

	private void OnSuccess(EventWaitHandle waitHandle)
	{
		lock (_counterLocker)
		{
			_consecutiveFailCount = 0;
		}
		try
		{
			if (SuccessCallback != null)
			{
				SuccessCallback(_consecutiveFailCount);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideoPushBase.OnSuccess.SuccessCallback", ex.Message);
		}
		EndTheTransmision(waitHandle);
		_fpsMeterMeasure.OnFrame();
	}

	private void EndTheTransmision(EventWaitHandle waitHandle)
	{
		try
		{
			waitHandle?.Set();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpVideoPushBase.EndTheTransmision", ex.Message);
		}
	}

	public void Dispose()
	{
	}
}

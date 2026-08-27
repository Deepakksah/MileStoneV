using System;
using System.IO;
using System.Net;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpClientObject : IRequest, IResponse
{
	private readonly HttpWebRequest _webRequest;

	private Action<IResponse> _asyncCallback;

	private HttpWebResponse _iResponse;

	private Stream _responseStream;

	public string Method
	{
		get
		{
			return _webRequest.Method;
		}
		set
		{
			_webRequest.Method = value;
		}
	}

	public HttpClientObject(string webAddress)
	{
		_webRequest = HttpClient.HttpWebRequestFactory.Create(webAddress);
	}

	public void GetResponse(Action<IResponse> asyncCallback)
	{
		_asyncCallback = asyncCallback;
		_webRequest.BeginGetResponse(GetResponseCallback, _webRequest);
	}

	private void GetResponseCallback(IAsyncResult asyncResult)
	{
		HttpWebRequest httpWebRequest = (HttpWebRequest)asyncResult.AsyncState;
		try
		{
			_iResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
			_responseStream = _iResponse.GetResponseStream();
			_asyncCallback(this);
		}
		catch (Exception exception)
		{
			OnFail(exception, "HttpClientObject.GetResponseCallback");
		}
		Disposing.DisposeMember(ref _responseStream);
		Disposing.DisposeMember(ref _iResponse);
	}

	private void OnFail(Exception exception, string origin)
	{
		Logger.Instance.Log(LogContent.Error, origin, exception.Message);
		if (_asyncCallback != null)
		{
			_asyncCallback(null);
		}
	}

	public int Read(byte[] buffer, int nOffset, int nCount)
	{
		int result = 0;
		if (_responseStream != null)
		{
			result = _responseStream.Read(buffer, nOffset, nCount);
		}
		return result;
	}
}

using System;
using System.Globalization;
using VideoOS.RecorderWebservice.Shared.Status2;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public class RecorderStatusServiceProxy : IRecorderStatusService2, IDisposable
{
	private RecorderStatusService2 _recorderStatusSvcProxy;

	private bool disposedValue;

	public RecorderStatusServiceProxy(string webServerUrl)
	{
		_recorderStatusSvcProxy = new RecorderStatusService2();
		webServerUrl = webServerUrl.TrimEnd('/');
		_recorderStatusSvcProxy.Url = string.Format(CultureInfo.InvariantCulture, "{0}/RecorderStatusService/RecorderStatusService2.asmx", webServerUrl);
	}

	public Status GetCurrentDeviceStatus(string token, Guid[] deviceIds)
	{
		return _recorderStatusSvcProxy.GetCurrentDeviceStatus(token, deviceIds);
	}

	public AttachAndConnectionState GetRecorderStatus(string token)
	{
		return _recorderStatusSvcProxy.GetRecorderStatus(token);
	}

	public VideoDeviceStatistics[] GetVideoDeviceStatistics(string token, Guid[] deviceIds)
	{
		return _recorderStatusSvcProxy.GetVideoDeviceStatistics(token, deviceIds);
	}

	public void Abort()
	{
		_recorderStatusSvcProxy.Abort();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && _recorderStatusSvcProxy != null)
			{
				_recorderStatusSvcProxy.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}

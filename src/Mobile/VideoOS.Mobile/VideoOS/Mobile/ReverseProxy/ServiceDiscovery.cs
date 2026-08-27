using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.ReverseProxy;

public class ServiceDiscovery : IServiceDiscovery, IControl
{
	private const int CycleMs = 15000;

	private readonly CycleTaskMember _cycleIdpRetrieverTask;

	private Uri _idpAddress;

	private readonly IDiscoveryServiceConfiguration _configuration;

	private readonly TimeoutUpdateChecker _timeoutUpdateChecker;

	private readonly Func<IEnumerable<Uri>, TimeSpan, Uri> _tryFindService;

	public ServiceDiscovery(IDiscoveryServiceConfiguration configuration, Func<IEnumerable<Uri>, TimeSpan, Uri> tryFindService)
	{
		_configuration = configuration;
		_timeoutUpdateChecker = new TimeoutUpdateChecker(_configuration.UpdateFrequencyMs);
		_cycleIdpRetrieverTask = new CycleTaskMember((Func<TaskFuncResult>)RetrieveIdpAddress, (Action)null, (Action)null, 15000);
		_tryFindService = tryFindService;
	}

	private TaskFuncResult RetrieveIdpAddress()
	{
		_timeoutUpdateChecker.Update((Func<bool>)RetrieveUri);
		return TaskFuncResult.SleepForaWhile;
	}

	private bool RetrieveUri()
	{
		bool result = false;
		List<Uri> arg = new List<Uri>(_configuration.Uris);
		Uri uri = _tryFindService(arg, _configuration.WaitForResponseTimeoutMs);
		if (uri != null)
		{
			_idpAddress = new UriBuilder(uri.Scheme, uri.Host, uri.Port).Uri;
			result = true;
		}
		Log.Trace("RetrieveUri", _idpAddress?.ToString());
		return result;
	}

	public void Start()
	{
		_cycleIdpRetrieverTask.StartTask();
	}

	public void Stop()
	{
		_cycleIdpRetrieverTask.SignalTaskToStop();
	}

	public Uri GetServiceUri()
	{
		return _idpAddress;
	}
}

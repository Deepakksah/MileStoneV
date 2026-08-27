using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class RestrictedMediaService
{
	private static RestrictedMediaService _instance;

	private static bool _telemetryDataSent;

	public static RestrictedMediaService Instance
	{
		[DebuggerStepThrough]
		get
		{
			if (_instance == null)
			{
				_instance = new RestrictedMediaService();
			}
			if (!_telemetryDataSent)
			{
				new TrackPageViewData(typeof(RestrictedMediaService)).Send();
				_telemetryDataSent = true;
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual RestrictedMediaResult RestrictedMediaCreate(ServerId serverId, IEnumerable<FQID> devices, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMedia[] RestrictedMediaQueryAll(ServerId serverId)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaResult RestrictedMediaUpdate(FQID restrictedMediaFQID, IEnumerable<FQID> devicesToAdd, IEnumerable<FQID> devicesToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaResult RestrictedMediaDelete(FQID restrictedMediaFQID)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(ServerId serverId, IEnumerable<FQID> devices, DateTime timeStart)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaResult RestrictedMediaLiveModeExit(ServerId serverId, IEnumerable<FQID> devices, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaLive[] RestrictedMediaLiveQueryAll(ServerId serverId)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaLiveResult RestrictedMediaLiveUpdate(ServerId serverId, IEnumerable<FQID> devices, DateTime timeStart)
	{
		throw new NotImplementedException();
	}

	public virtual RestrictedMediaLiveResult RestrictedMediaLiveDelete(ServerId serverId, IEnumerable<FQID> devices)
	{
		throw new NotImplementedException();
	}
}

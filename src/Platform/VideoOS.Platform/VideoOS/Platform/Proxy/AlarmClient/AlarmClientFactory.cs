using System;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Proxy.AlarmClient;

public class AlarmClientFactory
{
	public AlarmClientFactory()
	{
	}

	public static IAlarmClient Create(ServerId serverId, string eventServerURI, Version eventServerVersion, bool useToken)
	{
		if (useToken)
		{
			new TrackPageViewData(typeof(AlarmClientToken)).Send();
			return new AlarmClientToken(serverId, eventServerURI, eventServerVersion);
		}
		new TrackPageViewData(typeof(AlarmClientCredentials)).Send();
		return new AlarmClientCredentials(serverId, eventServerURI, eventServerVersion);
	}
}

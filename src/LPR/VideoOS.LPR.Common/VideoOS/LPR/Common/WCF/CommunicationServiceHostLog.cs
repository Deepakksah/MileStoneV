using System;
using VideoOS.Platform;
using VideoOS.ServiceHost.Common;

namespace VideoOS.LPR.Common.WCF;

public sealed class CommunicationServiceHostLog : ICommunicationServiceHostLog
{
	public void Debug(string service, string message)
	{
		EnvironmentManager.Instance.Log(error: false, "LPRPluginWebService", message);
	}

	public void Info(object service, string message)
	{
		EnvironmentManager.Instance.Log(error: false, "LPRPluginWebService", message);
	}

	public void Error(object service, string message, Exception logError)
	{
		EnvironmentManager.Instance.Log(error: true, "LPRPluginWebService", message + logError?.Message);
	}
}

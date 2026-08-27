using System;
using System.Collections.Generic;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Settings;
using VideoOS.Server.Webservice.Shared.DataCollectorService;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public class LiveDataAgent
{
	public static List<PerformanceCounterValue> GetPerformanceCounterValues(string hostName, string categoryName, string[] counterNames)
	{
		if (counterNames == null)
		{
			throw new ArgumentNullException("counterNames");
		}
		List<PerformanceCounterValue> list = new List<PerformanceCounterValue>();
		using ILiveDataCollectionManager liveDataCollectionManager = LiveDataCollectionManagerFactory.CreateInstance(hostName, ConfigSettings.Secure, LiveAdapterData.DataCollectorTokenCache);
		foreach (string counterName in counterNames)
		{
			PerformanceCounterValue[] performanceCounterValues = liveDataCollectionManager.GetPerformanceCounterValues(categoryName, counterName, string.Empty, ConfigSettings.DataCollectorSampleFrequenceInSec);
			list.AddRange(performanceCounterValues);
		}
		return list;
	}

	public static float GetPerformanceCounterInstanceValue(string hostName, string categoryName, string counterName, string instanceName)
	{
		using ILiveDataCollectionManager liveDataCollectionManager = LiveDataCollectionManagerFactory.CreateInstance(hostName, ConfigSettings.Secure, LiveAdapterData.DataCollectorTokenCache);
		return liveDataCollectionManager.GetPerformanceCounterValues(categoryName, counterName, instanceName, ConfigSettings.DataCollectorSampleFrequenceInSec)[0].CalculatedValue;
	}
}

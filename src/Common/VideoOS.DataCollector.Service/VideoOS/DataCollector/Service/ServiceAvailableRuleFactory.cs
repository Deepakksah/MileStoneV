using System;

namespace VideoOS.DataCollector.Service;

public static class ServiceAvailableRuleFactory
{
	private const string RECORDING_SERVICE_TYPE = "2B2D3601-5857-44A3-880F-BE7957B5B27E";

	public static ServiceAvailableRule CreateServiceAvailableRule(int dataCollectorId, string name, string serviceName, string serviceType, string id, IStateHandlerFactory stateHandlerFactory)
	{
		if (IsRecordingServer(serviceType))
		{
			return new RecordingServiceAvailableRule(dataCollectorId, name, serviceName, serviceType, id, stateHandlerFactory);
		}
		return new ServiceAvailableRule(dataCollectorId, name, serviceName, serviceType, id, stateHandlerFactory);
	}

	private static bool IsRecordingServer(string serviceType)
	{
		return string.Compare(serviceType, "2B2D3601-5857-44A3-880F-BE7957B5B27E", StringComparison.Ordinal) == 0;
	}
}

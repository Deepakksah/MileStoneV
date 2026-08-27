using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Services;

namespace VideoOS.Common.Integration;

public static class PerformanceRuleServiceTypes
{
	public static IList<Guid> RuleServiceTypes => new Guid[9]
	{
		ServiceTypes.ServiceType_ManagementService,
		ServiceTypes.ServiceType_RecordingService,
		ServiceTypes.ServiceType_FailoverService,
		ServiceTypes.ServiceType_EventServerService,
		ServiceTypes.ServiceType_LicensePlateRecognitionService,
		ServiceTypes.ServiceType_LogService,
		ServiceTypes.ServiceType_MobileService,
		ServiceTypes.ServiceType_GatewayService,
		ServiceTypes.ServiceType_AlarmsService
	};

	public static IList<Guid> RuleCameraTypes => new Guid[1] { WellKnownDevicesGroupIds.AllCameras };
}

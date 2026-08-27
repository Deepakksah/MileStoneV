using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.BusinessObjects;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class FaultOrWarningDeviceConverter
{
	public static VideoOS.WebService.Common.ServerCommandService.FaultDevice[] ToScsFaultDevice(IEnumerable<VideoOS.Management.Server.BusinessObjects.FaultDevice> faults)
	{
		return faults.Select((VideoOS.Management.Server.BusinessObjects.FaultDevice i) => new VideoOS.WebService.Common.ServerCommandService.FaultDevice
		{
			DeviceId = new Guid(i.DeviceId),
			ErrorCode = i.ErrorCode,
			Message = i.Message
		}).ToArray();
	}

	public static VideoOS.WebService.Common.ServerCommandService.WarningDevice[] ToScsWarningDevice(IEnumerable<VideoOS.Management.Server.BusinessObjects.WarningDevice> warnings)
	{
		return warnings.Select((VideoOS.Management.Server.BusinessObjects.WarningDevice i) => new VideoOS.WebService.Common.ServerCommandService.WarningDevice
		{
			DeviceId = new Guid(i.DeviceId),
			ErrorCode = i.ErrorCode,
			Message = i.Message
		}).ToArray();
	}
}

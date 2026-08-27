using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IFaultDevice
{
	Guid DeviceId { get; set; }

	int ErrorCode { get; set; }

	string Message { get; set; }
}

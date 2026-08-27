using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IWarningDevice
{
	Guid DeviceId { get; set; }

	int ErrorCode { get; set; }

	string Message { get; set; }
}

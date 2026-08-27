using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLive
{
	Guid DeviceId { get; set; }

	string UserName { get; set; }

	DateTime LastModified { get; set; }

	DateTime StartTime { get; set; }
}

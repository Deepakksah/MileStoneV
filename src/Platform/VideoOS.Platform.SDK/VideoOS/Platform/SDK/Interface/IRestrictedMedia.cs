using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMedia
{
	Guid Id { get; set; }

	Guid[] DeviceIds { get; set; }

	string Header { get; set; }

	string Description { get; set; }

	string UserName { get; set; }

	DateTime LastModified { get; set; }

	DateTime StartTime { get; set; }

	DateTime EndTime { get; set; }
}

using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedData
{
	DateTime Created { get; set; }

	string Description { get; set; }

	Guid[] DeviceIds { get; set; }

	DateTime EndTime { get; set; }

	string Header { get; set; }

	Guid Id { get; set; }

	DateTime LastModified { get; set; }

	string Reference { get; set; }

	DateTime RetentionExpire { get; set; }

	IRetentionOption RetentionOption { get; set; }

	DateTime StartTime { get; set; }

	DateTime TagTime { get; set; }

	bool UseRetention { get; set; }

	string User { get; set; }

	long RetentionSize { get; set; }
}

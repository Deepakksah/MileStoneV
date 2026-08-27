using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IHardwareInfo
{
	Guid HardwareId { get; set; }

	Guid[] DeviceIds { get; set; }

	Guid RecorderId { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	string Uri { get; set; }

	bool Interconnected { get; set; }

	DateTime LastModified { get; set; }

	bool Disabled { get; set; }
}

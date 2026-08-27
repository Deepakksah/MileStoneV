using System;

namespace VideoOS.Platform.SDK.Config;

public interface IDevice
{
	bool Disabled { get; set; }

	IRecorder Recorder { get; set; }

	string Name { get; set; }

	Guid Guid { get; set; }

	string DBExportName { get; }

	PositioningInformation PositioningInformation { get; }
}

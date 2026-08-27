using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IDeviceInfo
{
	Guid RecorderId { get; set; }

	Guid DeviceId { get; set; }

	Guid HardwareId { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	string Shortcut { get; set; }

	int DeviceIndex { get; set; }

	string GisPoint { get; set; }

	string ShortName { get; set; }

	int Icon { get; set; }

	double CoverageDirection { get; set; }

	double CoverageDepth { get; set; }

	double CoverageFieldOfView { get; set; }
}

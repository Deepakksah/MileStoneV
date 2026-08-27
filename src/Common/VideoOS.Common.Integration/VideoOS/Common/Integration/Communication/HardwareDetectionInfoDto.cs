using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class HardwareDetectionInfoDto
{
	public Uri Uri { get; set; }

	public string ScanRangeXml { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public int DriverNumber { get; set; }

	public string AutoDetectResult { get; set; }
}

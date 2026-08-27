using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class RecorderInformationModel
{
	public string DisplayName { get; set; }

	public string HostName { get; set; }

	public Uri WebserverUri { get; set; }

	public string ScanRanges { get; set; }

	public string TimeZone { get; set; }

	public string DevicePackInfoXml { get; set; }

	public ushort FailoverServerPort { get; set; }

	public ushort DatabasePort { get; set; }

	public Guid FailoverServerId { get; set; }

	public string Version { get; set; }
}

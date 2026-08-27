using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class SetHardwareNetworkSettingsModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	public string HardwareAddress { get; set; }

	public int? HardwarePort { get; set; }

	public string DefaultGateway { get; set; }

	public string SubnetMask { get; set; }
}

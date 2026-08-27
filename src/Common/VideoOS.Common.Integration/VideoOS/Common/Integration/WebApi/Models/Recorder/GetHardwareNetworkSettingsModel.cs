using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class GetHardwareNetworkSettingsModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }
}

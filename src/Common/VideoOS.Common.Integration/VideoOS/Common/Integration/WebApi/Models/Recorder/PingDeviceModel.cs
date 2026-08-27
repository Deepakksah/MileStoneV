using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class PingDeviceModel : CommandModelBase
{
	public string DeviceAddress { get; set; }

	[RequiredNonemptyGuid]
	public Guid RecorderId { get; set; }

	public TimeSpan Timeout { get; set; }
}

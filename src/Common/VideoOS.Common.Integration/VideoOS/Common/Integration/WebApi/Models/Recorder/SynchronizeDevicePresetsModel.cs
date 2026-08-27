using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class SynchronizeDevicePresetsModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }

	[RequiredNonemptyGuid]
	public Guid RecorderId { get; set; }
}

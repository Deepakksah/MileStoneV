using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class GetAbsolutePtzPositionModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }
}

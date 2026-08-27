using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class PtzStopSessionModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }

	public string UserToken { get; set; }
}

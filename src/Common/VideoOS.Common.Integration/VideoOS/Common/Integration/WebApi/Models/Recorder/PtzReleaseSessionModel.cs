using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class PtzReleaseSessionModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }

	public string UserToken { get; set; }
}

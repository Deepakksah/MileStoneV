using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class PrepareMoveHardwareModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	public int TimeoutSeconds { get; set; }
}

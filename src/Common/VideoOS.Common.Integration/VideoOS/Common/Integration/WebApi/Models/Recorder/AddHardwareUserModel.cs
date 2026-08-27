using System;
using VideoOS.Common.Integration.WebApi.Models.Server;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class AddHardwareUserModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	public HardwareUserModel HardwareUser { get; set; }
}

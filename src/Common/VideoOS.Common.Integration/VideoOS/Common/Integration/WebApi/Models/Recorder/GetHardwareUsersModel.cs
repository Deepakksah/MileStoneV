using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class GetHardwareUsersModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }
}

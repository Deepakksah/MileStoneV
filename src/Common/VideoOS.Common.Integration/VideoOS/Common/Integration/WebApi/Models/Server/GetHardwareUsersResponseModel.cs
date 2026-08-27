using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class GetHardwareUsersResponseModel : ResponseModel
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	public HardwareUserModel[] HardwareUsers { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class GetAbsolutePtzPositionResponseModel : ResponseModel
{
	[Required]
	public double Pan { get; set; }

	[Required]
	public double Tilt { get; set; }

	[Required]
	public double Zoom { get; set; }

	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }
}

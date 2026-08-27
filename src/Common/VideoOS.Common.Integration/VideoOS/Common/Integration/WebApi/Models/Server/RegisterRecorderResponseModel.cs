using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class RegisterRecorderResponseModel : ResponseModelBase
{
	[Required]
	public bool Compatible { get; set; }

	[Required]
	public TimeSpan ServiceDiscoveryTimeout { get; set; }
}

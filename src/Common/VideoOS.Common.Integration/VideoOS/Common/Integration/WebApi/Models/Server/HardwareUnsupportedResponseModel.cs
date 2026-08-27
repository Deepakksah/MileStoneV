using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class HardwareUnsupportedResponseModel : ResponseModel
{
	[Required]
	public string ProductName { get; set; }

	[Required]
	public string ProductVersion { get; set; }

	[Required]
	public int ErrorCode { get; set; }
}

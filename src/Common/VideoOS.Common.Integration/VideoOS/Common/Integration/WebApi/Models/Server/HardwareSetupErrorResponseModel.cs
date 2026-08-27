using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class HardwareSetupErrorResponseModel : ResponseModel
{
	[Required]
	public int ErrorCode { get; set; }

	[Required]
	public string ErrorMessageInternal { get; set; }
}

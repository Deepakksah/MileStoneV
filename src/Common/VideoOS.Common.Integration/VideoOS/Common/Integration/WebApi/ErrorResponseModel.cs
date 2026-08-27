using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi;

public class ErrorResponseModel : ResponseModel
{
	public int ErrorCode { get; set; }

	[Required]
	public string Message { get; set; }
}

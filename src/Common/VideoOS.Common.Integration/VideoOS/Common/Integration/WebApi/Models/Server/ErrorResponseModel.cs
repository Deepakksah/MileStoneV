using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ErrorResponseModel : ResponseModel
{
	public int ErrorCode { get; set; }

	public int SubErrorCode { get; set; }

	[Required]
	public string Message { get; set; }
}

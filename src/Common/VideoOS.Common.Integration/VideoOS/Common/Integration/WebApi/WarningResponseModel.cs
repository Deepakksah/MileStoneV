using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi;

public class WarningResponseModel : ResponseModel
{
	[Required]
	public string Message { get; set; }
}

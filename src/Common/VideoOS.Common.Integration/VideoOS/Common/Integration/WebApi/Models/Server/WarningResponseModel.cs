using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class WarningResponseModel : ResponseModel
{
	[Required]
	public string Message { get; set; }
}

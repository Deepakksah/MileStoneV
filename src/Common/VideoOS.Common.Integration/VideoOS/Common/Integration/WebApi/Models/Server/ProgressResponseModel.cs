using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ProgressResponseModel : ResponseModel
{
	[Required]
	public int Progress { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ReplacementTokenModel
{
	[Required]
	public string ReplacementToken { get; set; }
}

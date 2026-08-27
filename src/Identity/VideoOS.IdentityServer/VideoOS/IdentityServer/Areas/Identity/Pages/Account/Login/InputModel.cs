using System.ComponentModel.DataAnnotations;
using VideoOS.IdentityServer.Resources;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Login;

public class InputModel
{
	[Required]
	[DataType(/*Could not decode attribute arguments.*/)]
	[Display(Name = "UserName", ResourceType = typeof(IdentityResource))]
	public string UserName { get; set; }

	[Required]
	[DataType(/*Could not decode attribute arguments.*/)]
	[Display(Name = "Password", ResourceType = typeof(IdentityResource))]
	public string Password { get; set; }

	public string ReturnUrl { get; set; }
}

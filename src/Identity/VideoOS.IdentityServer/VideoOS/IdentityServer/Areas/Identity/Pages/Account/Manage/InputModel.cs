using System.ComponentModel.DataAnnotations;
using VideoOS.IdentityServer.Attributes;
using VideoOS.IdentityServer.Resources;
using VideoOS.IdentityServer.Validators;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Manage;

public class InputModel
{
	[Required(ErrorMessageResourceName = "ErrorEmptyPassword", ErrorMessageResourceType = typeof(IdentityResource))]
	[DataType(/*Could not decode attribute arguments.*/)]
	[Display(Name = "CurrentPassword", ResourceType = typeof(IdentityResource))]
	public string CurrentPassword { get; set; }

	[Required(ErrorMessageResourceName = "ErrorEmptyPassword", ErrorMessageResourceType = typeof(IdentityResource))]
	[DataType(/*Could not decode attribute arguments.*/)]
	[Display(Name = "NewPassword", ResourceType = typeof(IdentityResource))]
	[NotEqual("CurrentPassword", ErrorMessageResourceName = "ErrorTheSamePasswords", ErrorMessageResourceType = typeof(IdentityResource))]
	[PasswordRequirements(ErrorMessageResourceName = "ErrorStrongPassword", ErrorMessageResourceType = typeof(IdentityResource))]
	public string NewPassword { get; set; }

	[Required(ErrorMessageResourceName = "ErrorEmptyPassword", ErrorMessageResourceType = typeof(IdentityResource))]
	[DataType(/*Could not decode attribute arguments.*/)]
	[Display(Name = "ConfirmPassword", ResourceType = typeof(IdentityResource))]
	[Compare("NewPassword", ErrorMessageResourceName = "ErrorDontMatchPasswords", ErrorMessageResourceType = typeof(IdentityResource))]
	[PasswordRequirements(ErrorMessageResourceName = "ErrorStrongPassword", ErrorMessageResourceType = typeof(IdentityResource))]
	[NotEqual("CurrentPassword", ErrorMessageResourceName = "ErrorTheSamePasswords", ErrorMessageResourceType = typeof(IdentityResource))]
	public string ConfirmPassword { get; set; }
}

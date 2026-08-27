using System;

namespace VideoOS.IdentityServer.Common;

public class UpdateUserModel : PatchDocumentBase
{
	public string Description { get; set; }

	public string Password { get; set; }

	public bool? CanChangePassword { get; set; }

	public bool? ForcePasswordChange { get; set; }

	public DateTimeOffset? LockoutEnd { get; set; }

	public bool? Active { get; set; }
}

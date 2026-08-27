using System;

namespace VideoOS.IdentityServer.Common;

public class CreateUserModel
{
	public string Id { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string Description { get; set; }

	public bool CanChangePassword { get; set; }

	public bool ForcePasswordChange { get; set; }

	public DateTimeOffset? LockoutEnd { get; set; }

	public bool Active { get; set; } = true;
}

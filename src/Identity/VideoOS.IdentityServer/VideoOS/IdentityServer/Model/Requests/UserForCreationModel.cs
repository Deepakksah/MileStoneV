using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.IdentityServer.Attributes;

namespace VideoOS.IdentityServer.Model.Requests;

public class UserForCreationModel
{
	[IsGuid]
	public string Id { get; set; }

	[Required]
	[DisallowCharacter(new char[] { '?', '/', '[', ']', '\\' })]
	public string UserName { get; set; }

	public string Password { get; set; }

	public string Description { get; set; }

	public bool CanChangePassword { get; set; } = true;

	public bool ForcePasswordChange { get; set; }

	public DateTimeOffset? LockoutEnd { get; set; }

	public bool Active { get; set; } = true;
}

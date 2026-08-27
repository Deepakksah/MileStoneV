using System;
using System.Collections.Generic;

namespace VideoOS.IdentityServer.Model.Responses;

public class UserDto
{
	public string Id { get; set; }

	public string UserName { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	public string Description { get; set; }

	public bool CanChangePassword { get; set; }

	public bool ForcePasswordChange { get; set; }

	public DateTimeOffset? LockoutEnd { get; set; }

	public bool Active { get; set; }

	public ICollection<UserClaimDto> Claims { get; set; } = new List<UserClaimDto>();

	public ICollection<UserLoginDto> Logins { get; set; } = new List<UserLoginDto>();

	public DateTime? DeactivatedAtUtc { get; set; }
}

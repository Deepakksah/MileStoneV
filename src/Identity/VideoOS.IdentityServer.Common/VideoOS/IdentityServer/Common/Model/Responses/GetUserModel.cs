using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class GetUserModel
{
	public string Id { get; set; }

	public string UserName { get; set; }

	public string Description { get; set; }

	public bool CanChangePassword { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	public bool ForcePasswordChange { get; set; }

	public DateTimeOffset? LockoutEnd { get; set; }

	public bool Active { get; set; }

	public IEnumerable<GetUserClaimModel> Claims { get; set; }

	public IEnumerable<GetUserProviderModel> Logins { get; set; }

	public bool IsExternal => Logins?.Any() ?? false;
}

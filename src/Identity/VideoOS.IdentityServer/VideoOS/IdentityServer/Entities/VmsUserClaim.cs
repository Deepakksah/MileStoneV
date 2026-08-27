using System;
using Microsoft.AspNetCore.Identity;

namespace VideoOS.IdentityServer.Entities;

public class VmsUserClaim : IdentityUserClaim<string>, IModifiedAware
{
	public string LoginProvider { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ModifiedDate { get; set; }
}

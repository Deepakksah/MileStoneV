using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Data;

public class VmsUserStore : UserStore<VmsUser, IdentityRole<string>, DbContext, string, VmsUserClaim, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
{
	public VmsUserStore(IdentityDbContextWrapper context, IdentityErrorDescriber describer = null)
		: base((DbContext)context, describer)
	{
	}
}

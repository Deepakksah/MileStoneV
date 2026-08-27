using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface IUserProvisioner
{
	Task<VmsUser> CreateOrUpdateUser(UserLoginInfo info, List<Claim> claims);
}

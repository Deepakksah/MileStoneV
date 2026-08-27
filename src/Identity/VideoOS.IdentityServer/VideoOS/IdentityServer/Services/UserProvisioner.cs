using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class UserProvisioner(VmsUserManager userManager, ILoginProviderRepository providerRepository) : IUserProvisioner
{
	public async Task<VmsUser> CreateOrUpdateUser(UserLoginInfo info, List<Claim> claims)
	{
		List<Claim> externalClaims = ExternalClaims(claims);
		VmsUser vmsUser = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey, "Claims");
		if (vmsUser == null)
		{
			IdentityResult identityResult = await AutoProvisionUserAsync(info, externalClaims);
			if (!identityResult.Succeeded)
			{
				throw new InvalidOperationException(string.Join(", ", identityResult.Errors.Select((IdentityError e) => e.Description)));
			}
			vmsUser = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
		}
		else
		{
			vmsUser = await userManager.UpdateUserClaims(vmsUser, externalClaims, info);
		}
		return vmsUser;
	}

	private async Task<IdentityResult> AutoProvisionUserAsync(UserLoginInfo info, List<Claim> externalClaims)
	{
		string text = await GetUniqueUserName(info.LoginProvider, externalClaims);
		string text2 = Guid.NewGuid().ToString();
		VmsUser vmsUser = VmsUser.Create(string.IsNullOrEmpty(text) ? text2 : text, externalClaims.FirstOrDefault((Claim x) => x.Type == "name")?.Value, canChangePassword: false, forcePasswordChange: false, null, active: true);
		vmsUser.Id = text2;
		return await userManager.CreateUserAsync(vmsUser, null, info, externalClaims);
	}

	private async Task<string> GetUniqueUserName(string loginProviderId, ICollection<Claim> filtered)
	{
		string firstFoundClaimValue = string.Empty;
		string configuredClaimType = (await providerRepository.GetLoginProvider(new Guid(loginProviderId)))?.UserNameClaimType;
		string userName;
		if (!string.IsNullOrEmpty(configuredClaimType))
		{
			userName = filtered.FirstOrDefault((Claim f) => f.Type == configuredClaimType)?.Value;
			if (!string.IsNullOrEmpty(userName))
			{
				if (await userManager.FindByNameAsync(userName) == null)
				{
					return userName;
				}
				if (string.IsNullOrEmpty(firstFoundClaimValue))
				{
					firstFoundClaimValue = userName;
				}
			}
		}
		userName = filtered.FirstOrDefault((Claim x) => x.Type == "preferred_username")?.Value;
		if (!string.IsNullOrEmpty(userName))
		{
			if (await userManager.FindByNameAsync(userName) == null)
			{
				return userName;
			}
			if (string.IsNullOrEmpty(firstFoundClaimValue))
			{
				firstFoundClaimValue = userName;
			}
		}
		userName = filtered.FirstOrDefault((Claim x) => x.Type == "name")?.Value;
		if (!string.IsNullOrEmpty(userName))
		{
			if (await userManager.FindByNameAsync(userName) == null)
			{
				return userName;
			}
			if (string.IsNullOrEmpty(firstFoundClaimValue))
			{
				firstFoundClaimValue = userName;
			}
		}
		userName = filtered.FirstOrDefault((Claim x) => x.Type == "email")?.Value;
		if (!string.IsNullOrEmpty(userName))
		{
			if (await userManager.FindByNameAsync(userName) == null)
			{
				return userName;
			}
			if (string.IsNullOrEmpty(firstFoundClaimValue))
			{
				firstFoundClaimValue = userName;
			}
		}
		int num = 1;
		while (true)
		{
			userName = firstFoundClaimValue + "#" + num;
			if (await userManager.FindByNameAsync(userName) == null)
			{
				break;
			}
			num++;
		}
		return userName;
	}

	private static List<Claim> ExternalClaims(List<Claim> claims)
	{
		List<Claim> claims2 = FilterClaims(claims);
		return EnsureNameClaim(claims2);
	}

	private static List<Claim> FilterClaims(List<Claim> claimsList)
	{
		List<Claim> list = new List<Claim>();
		foreach (Claim claims in claimsList)
		{
			if (claims.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
			{
				list.Add(new Claim("name", claims.Value));
			}
			else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claims.Type))
			{
				list.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claims.Type], claims.Value));
			}
			else if (claims.Type != "sub" && claims.Type != "sid")
			{
				list.Add(claims);
			}
		}
		return list;
	}

	private static List<Claim> EnsureNameClaim(List<Claim> claims)
	{
		if (claims.All((Claim x) => x.Type != "name"))
		{
			string text = claims.FirstOrDefault((Claim x) => x.Type == "given_name")?.Value;
			string text2 = claims.FirstOrDefault((Claim x) => x.Type == "family_name")?.Value;
			if (text != null && text2 != null)
			{
				claims.Add(new Claim("name", text + " " + text2));
			}
			else if (text != null)
			{
				claims.Add(new Claim("name", text));
			}
			else if (text2 != null)
			{
				claims.Add(new Claim("name", text2));
			}
		}
		return claims;
	}
}

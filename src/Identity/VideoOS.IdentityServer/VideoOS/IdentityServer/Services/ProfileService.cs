using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Services;

public class ProfileService : IProfileService
{
	private readonly VmsUserManager _vmsUserManager;

	protected readonly ILogger Logger;

	public ProfileService(VmsUserManager vmsUserManager, ILogger<ProfileService> logger)
	{
		_vmsUserManager = vmsUserManager;
		Logger = logger;
	}

	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		string subjectId = context.Subject.GetSubjectId();
		Logger.LogDebug("Get profile data for subject ID {subjectId} from client {client} with claim types {claimTypes} via {caller}", subjectId, context.Client.ClientName ?? context.Client.ClientId, context.RequestedClaimTypes, context.Caller);
		if (WindowsAuthentication(context.Subject))
		{
			context.AddRequestedClaims(context.Subject.Claims);
			return;
		}
		VmsUser vmsUser = await _vmsUserManager.FindByIdAsync(subjectId, "Claims");
		DateTimeOffset dateTimeOffset = vmsUser.ModifiedDate;
		ClaimsIdentity claimsIdentity = new ClaimsIdentity();
		claimsIdentity.AddClaims(new List<Claim>
		{
			new Claim("name", vmsUser.UserName),
			new Claim("updated_at", dateTimeOffset.ToUnixTimeSeconds().ToString())
		});
		if (context.Caller == "UserInfoEndpoint" || context.Caller == "ClaimsProviderIdentityToken")
		{
			Claim idpClaim = context.Subject.Claims.FirstOrDefault((Claim c) => c.Type.Equals("idp"));
			if (HasRequestedProviderScope(context))
			{
				context.IssuedClaims.AddRange(CollectClaimsBasedOnProvider(vmsUser, idpClaim).Claims);
			}
			else
			{
				claimsIdentity.AddClaims(CollectClaimsBasedOnProvider(vmsUser, idpClaim).Claims);
			}
		}
		context.AddRequestedClaims(claimsIdentity.Claims.ToList());
	}

	private ClaimsIdentity CollectClaimsBasedOnProvider(VmsUser user, Claim idpClaim)
	{
		ClaimsIdentity claimsIdentity = new ClaimsIdentity();
		if (idpClaim != null)
		{
			claimsIdentity.AddClaims(from vmsUserClaim in user.Claims
				where vmsUserClaim.LoginProvider != null && vmsUserClaim.LoginProvider.Equals(idpClaim.Value, StringComparison.InvariantCultureIgnoreCase) && !vmsUserClaim.ClaimType.Equals("name")
				select vmsUserClaim.ToClaim());
		}
		return claimsIdentity;
	}

	private bool HasRequestedProviderScope(ProfileDataRequestContext context)
	{
		return context.RequestedResources.ParsedScopes.Select((ParsedScopeValue v) => v.ParsedName).Contains("provider");
	}

	public async Task IsActiveAsync(IsActiveContext context)
	{
		if (WindowsAuthentication(context.Subject))
		{
			context.IsActive = true;
			return;
		}
		string subjectId = context.Subject.GetSubjectId();
		VmsUser vmsUser = await _vmsUserManager.FindByIdAsync(subjectId);
		context.IsActive = vmsUser != null && vmsUser.Active && !_vmsUserManager.IsUserLockedOut(vmsUser);
	}

	private static bool WindowsAuthentication(ClaimsPrincipal claimsPrincipal)
	{
		Claim claim = claimsPrincipal.FindFirst("amr");
		if (claim != null)
		{
			return claim.Value == "wia";
		}
		return false;
	}
}

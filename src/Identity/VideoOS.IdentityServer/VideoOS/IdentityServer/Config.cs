using System;
using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;

namespace VideoOS.IdentityServer;

public class Config
{
	public static IEnumerable<Duende.IdentityServer.EntityFramework.Entities.Client> GetClients()
	{
		return new Duende.IdentityServer.EntityFramework.Entities.Client[5]
		{
			new Duende.IdentityServer.EntityFramework.Entities.Client
			{
				Id = -1,
				Created = FirstCreatedTime(),
				ClientId = "winauthclient",
				ClientName = "Registration client",
				Enabled = true,
				RequireClientSecret = false,
				AccessTokenType = 0,
				AllowedGrantTypes = new List<ClientGrantType>
				{
					new ClientGrantType
					{
						Id = -1,
						GrantType = "windows_auth",
						ClientId = -1
					}
				},
				AllowOfflineAccess = false,
				AllowedScopes = new List<ClientScope>
				{
					new ClientScope
					{
						Id = -1,
						Scope = "write:client",
						ClientId = -1
					}
				},
				IncludeJwtId = true
			},
			new Duende.IdentityServer.EntityFramework.Entities.Client
			{
				Id = -2,
				Created = FirstCreatedTime(),
				ClientId = "GrantValidatorClient",
				ClientName = "GrantValidatorClient",
				Enabled = true,
				AllowedGrantTypes = new List<ClientGrantType>
				{
					new ClientGrantType
					{
						Id = -2,
						GrantType = "windows_credentials",
						ClientId = -2
					},
					new ClientGrantType
					{
						Id = -3,
						GrantType = "password",
						ClientId = -2
					}
				},
				AccessTokenType = 0,
				RequireConsent = false,
				RequireClientSecret = false,
				AllowOfflineAccess = false,
				AllowedScopes = new List<ClientScope>
				{
					new ClientScope
					{
						Id = -2,
						Scope = "managementserver",
						ClientId = -2
					}
				},
				IncludeJwtId = true
			},
			new Duende.IdentityServer.EntityFramework.Entities.Client
			{
				Id = -3,
				Created = FirstCreatedTime(),
				ClientId = "VmsAdminClient",
				ClientName = "VmsAdminClient",
				Enabled = true,
				RequireConsent = false,
				RedirectUris = new List<ClientRedirectUri>
				{
					new ClientRedirectUri
					{
						Id = -1,
						RedirectUri = "http://127.0.0.1",
						ClientId = -3
					}
				},
				AllowedGrantTypes = new List<ClientGrantType>
				{
					new ClientGrantType
					{
						Id = -4,
						GrantType = "authorization_code",
						ClientId = -3
					},
					new ClientGrantType
					{
						Id = -5,
						GrantType = "refresh_token",
						ClientId = -3
					},
					new ClientGrantType
					{
						Id = -6,
						GrantType = "password",
						ClientId = -3
					},
					new ClientGrantType
					{
						Id = -7,
						GrantType = "windows_credentials",
						ClientId = -3
					},
					new ClientGrantType
					{
						Id = -16,
						GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
						ClientId = -3
					}
				},
				AccessTokenType = 0,
				RequirePkce = true,
				AllowedScopes = new List<ClientScope>
				{
					new ClientScope
					{
						Id = -3,
						Scope = "managementserver",
						ClientId = -3
					},
					new ClientScope
					{
						Id = -4,
						Scope = "openid",
						ClientId = -3
					},
					new ClientScope
					{
						Id = -5,
						Scope = "profile",
						ClientId = -3
					},
					new ClientScope
					{
						Id = -6,
						Scope = "email",
						ClientId = -3
					},
					new ClientScope
					{
						Id = -7,
						Scope = "offline_access",
						ClientId = -3
					},
					new ClientScope
					{
						Id = -13,
						Scope = "incidentmanagerserver",
						ClientId = -3
					},
					new ClientScope
					{
						Id = -20,
						Scope = "provider",
						ClientId = -3
					}
				},
				RequireClientSecret = false,
				AllowOfflineAccess = true,
				RefreshTokenUsage = 1,
				UpdateAccessTokenClaimsOnRefresh = true,
				RefreshTokenExpiration = 0,
				AlwaysIncludeUserClaimsInIdToken = true,
				IncludeJwtId = true
			},
			new Duende.IdentityServer.EntityFramework.Entities.Client
			{
				Id = -4,
				Created = FirstCreatedTime(),
				ClientId = "VmsClient",
				ClientName = "VmsClient",
				Enabled = true,
				RequireConsent = false,
				RedirectUris = new List<ClientRedirectUri>
				{
					new ClientRedirectUri
					{
						Id = -2,
						RedirectUri = "http://127.0.0.1",
						ClientId = -4
					}
				},
				AllowedGrantTypes = new List<ClientGrantType>
				{
					new ClientGrantType
					{
						Id = -8,
						GrantType = "authorization_code",
						ClientId = -4
					},
					new ClientGrantType
					{
						Id = -9,
						GrantType = "refresh_token",
						ClientId = -4
					},
					new ClientGrantType
					{
						Id = -10,
						GrantType = "password",
						ClientId = -4
					},
					new ClientGrantType
					{
						Id = -11,
						GrantType = "windows_credentials",
						ClientId = -4
					},
					new ClientGrantType
					{
						Id = -17,
						GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
						ClientId = -4
					}
				},
				AccessTokenType = 0,
				RequirePkce = true,
				AllowedScopes = new List<ClientScope>
				{
					new ClientScope
					{
						Id = -8,
						Scope = "managementserver",
						ClientId = -4
					},
					new ClientScope
					{
						Id = -9,
						Scope = "openid",
						ClientId = -4
					},
					new ClientScope
					{
						Id = -10,
						Scope = "profile",
						ClientId = -4
					},
					new ClientScope
					{
						Id = -11,
						Scope = "email",
						ClientId = -4
					},
					new ClientScope
					{
						Id = -12,
						Scope = "offline_access",
						ClientId = -4
					},
					new ClientScope
					{
						Id = -14,
						Scope = "incidentmanagerserver",
						ClientId = -4
					},
					new ClientScope
					{
						Id = -21,
						Scope = "provider",
						ClientId = -4
					}
				},
				RequireClientSecret = false,
				AllowOfflineAccess = true,
				RefreshTokenUsage = 0,
				UpdateAccessTokenClaimsOnRefresh = true,
				RefreshTokenExpiration = 0,
				AbsoluteRefreshTokenLifetime = 0,
				AlwaysIncludeUserClaimsInIdToken = true,
				IncludeJwtId = true
			},
			new Duende.IdentityServer.EntityFramework.Entities.Client
			{
				Id = -5,
				Created = FirstCreatedTime(),
				ClientId = "VmsMobileClient",
				ClientName = "VmsMobileClient",
				Enabled = true,
				RequireConsent = false,
				RedirectUris = new List<ClientRedirectUri>
				{
					new ClientRedirectUri
					{
						Id = -3,
						RedirectUri = "com.milestonesys.xprotect://login.external",
						ClientId = -5
					}
				},
				AllowedGrantTypes = new List<ClientGrantType>
				{
					new ClientGrantType
					{
						Id = -12,
						GrantType = "authorization_code",
						ClientId = -5
					},
					new ClientGrantType
					{
						Id = -13,
						GrantType = "refresh_token",
						ClientId = -5
					},
					new ClientGrantType
					{
						Id = -14,
						GrantType = "password",
						ClientId = -5
					},
					new ClientGrantType
					{
						Id = -15,
						GrantType = "windows_credentials",
						ClientId = -5
					}
				},
				AccessTokenType = 0,
				RequirePkce = true,
				AllowedScopes = new List<ClientScope>
				{
					new ClientScope
					{
						Id = -15,
						Scope = "managementserver",
						ClientId = -5
					},
					new ClientScope
					{
						Id = -16,
						Scope = "openid",
						ClientId = -5
					},
					new ClientScope
					{
						Id = -17,
						Scope = "profile",
						ClientId = -5
					},
					new ClientScope
					{
						Id = -18,
						Scope = "email",
						ClientId = -5
					},
					new ClientScope
					{
						Id = -19,
						Scope = "offline_access",
						ClientId = -5
					}
				},
				RequireClientSecret = false,
				AllowOfflineAccess = true,
				RefreshTokenUsage = 0,
				UpdateAccessTokenClaimsOnRefresh = true,
				RefreshTokenExpiration = 0,
				AbsoluteRefreshTokenLifetime = 0,
				SlidingRefreshTokenLifetime = (int)TimeSpan.FromMinutes(180.0).TotalSeconds,
				AlwaysIncludeUserClaimsInIdToken = true,
				IncludeJwtId = true
			}
		};
	}

	public static IEnumerable<Duende.IdentityServer.EntityFramework.Entities.IdentityResource> GetIdentityScopes()
	{
		Duende.IdentityServer.EntityFramework.Entities.IdentityResource identityResource = new IdentityResources.OpenId().ToEntity();
		identityResource.Id = -1;
		identityResource.Created = FirstCreatedTime();
		identityResource.UserClaims.First().Id = -1;
		int userClaimId = 0;
		userClaimId = AssignNegativeId(identityResource.UserClaims, userClaimId, -1);
		yield return identityResource;
		Duende.IdentityServer.EntityFramework.Entities.IdentityResource identityResource2 = new IdentityResources.Profile().ToEntity();
		identityResource2.Id = -2;
		identityResource2.Created = FirstCreatedTime();
		userClaimId = AssignNegativeId(identityResource2.UserClaims, userClaimId, -2);
		yield return identityResource2;
		Duende.IdentityServer.EntityFramework.Entities.IdentityResource identityResource3 = new IdentityResources.Email().ToEntity();
		identityResource3.Id = -3;
		identityResource3.Created = FirstCreatedTime();
		AssignNegativeId(identityResource3.UserClaims, userClaimId, -3);
		yield return identityResource3;
		Duende.IdentityServer.EntityFramework.Entities.IdentityResource identityResource4 = new Duende.IdentityServer.EntityFramework.Entities.IdentityResource
		{
			Name = "provider",
			DisplayName = "User provider profile",
			Description = "Your users profile information belonging to the current login provider",
			Emphasize = true
		};
		identityResource4.Id = -4;
		identityResource4.Created = FirstCreatedTime();
		yield return identityResource4;
	}

	private static int AssignNegativeId(IEnumerable<IdentityResourceClaim> claims, int userClaimId, int identityResourceId)
	{
		foreach (IdentityResourceClaim claim in claims)
		{
			userClaimId--;
			claim.Id = userClaimId;
			claim.IdentityResourceId = identityResourceId;
		}
		return userClaimId;
	}

	public static IEnumerable<Duende.IdentityServer.EntityFramework.Entities.ApiScope> GetApiScopes()
	{
		return new List<Duende.IdentityServer.EntityFramework.Entities.ApiScope>
		{
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -1,
				Name = "write:client",
				DisplayName = "Client registration scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -2,
				Name = "management",
				DisplayName = "Management scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -3,
				Name = "recorder",
				DisplayName = "Recorder scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -4,
				Name = "idp_management",
				DisplayName = "IDP Management Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -5,
				Name = "datacollector",
				DisplayName = "Datacollector Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -6,
				Name = "managementserver",
				DisplayName = "Management server Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -7,
				Name = "failoverresponseserver",
				DisplayName = "Failover response server Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -8,
				Name = "mediadb",
				DisplayName = "Media Db Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -9,
				Name = "idp_log_event",
				DisplayName = "Log Event Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -10,
				Name = "vps",
				DisplayName = "Video Processing Server Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -11,
				Name = "incidentmanagerserver",
				DisplayName = "Incident Manager server scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -12,
				Name = "failover",
				DisplayName = "Failover server Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -13,
				Name = "scim",
				DisplayName = "Scim Scope",
				Created = FirstCreatedTime()
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiScope
			{
				Id = -14,
				Name = "serverlogs",
				DisplayName = "Server Logs Scope",
				Created = FirstCreatedTime()
			}
		};
	}

	public static IEnumerable<Duende.IdentityServer.EntityFramework.Entities.ApiResource> GetApis()
	{
		return new Duende.IdentityServer.EntityFramework.Entities.ApiResource[14]
		{
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -1,
				Created = FirstCreatedTime(),
				Name = "registration",
				Enabled = true,
				DisplayName = "Client registration",
				Description = "Api for client registration",
				UserClaims = new List<ApiResourceClaim>
				{
					new ApiResourceClaim
					{
						Id = -1,
						Type = "name",
						ApiResourceId = -1
					}
				},
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -1,
						Scope = "write:client",
						ApiResourceId = -1
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -2,
				Created = FirstCreatedTime(),
				Name = "management",
				Enabled = true,
				DisplayName = "Management server",
				Description = "Services for management server",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -2,
						ApiResourceId = -2,
						Scope = "management"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -3,
				Created = FirstCreatedTime(),
				Name = "recorder",
				Enabled = true,
				DisplayName = "Recorder server",
				Description = "Services for recorder server",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -3,
						ApiResourceId = -3,
						Scope = "recorder"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -4,
				Created = FirstCreatedTime(),
				Name = "idp_management",
				Enabled = true,
				DisplayName = "IDP Management",
				Description = "Management of IDP",
				UserClaims = new List<ApiResourceClaim>
				{
					new ApiResourceClaim
					{
						Id = -2,
						ApiResourceId = -4,
						Type = "name"
					}
				},
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -4,
						Scope = "idp_management"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -5,
				Created = FirstCreatedTime(),
				Name = "datacollector",
				Enabled = true,
				DisplayName = "Datacollector",
				Description = "Api for datacollector",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -5,
						ApiResourceId = -5,
						Scope = "datacollector"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -6,
				Created = FirstCreatedTime(),
				Name = "managementserver",
				Enabled = true,
				DisplayName = "Management Server",
				Description = "Api for management server",
				UserClaims = new List<ApiResourceClaim>
				{
					new ApiResourceClaim
					{
						Id = -3,
						ApiResourceId = -6,
						Type = "updated_at"
					},
					new ApiResourceClaim
					{
						Id = -4,
						ApiResourceId = -6,
						Type = "name"
					}
				},
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -6,
						ApiResourceId = -6,
						Scope = "managementserver"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -7,
				Created = FirstCreatedTime(),
				Name = "failoverresponseserver",
				Enabled = true,
				DisplayName = "Failover Response Server",
				Description = "Api for failover response server",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -7,
						ApiResourceId = -7,
						Scope = "failoverresponseserver"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -8,
				Created = FirstCreatedTime(),
				Name = "mediadb",
				Enabled = true,
				DisplayName = "MediaDb",
				Description = "Api for mediadb",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -8,
						ApiResourceId = -8,
						Scope = "mediadb"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -9,
				Created = FirstCreatedTime(),
				Name = "idp_log_event",
				Enabled = true,
				DisplayName = "IDP Logs",
				Description = "Api for IDP logs",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -9,
						ApiResourceId = -9,
						Scope = "idp_log_event"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -10,
				Created = FirstCreatedTime(),
				Name = "vps",
				Enabled = true,
				DisplayName = "Video Processing Server",
				Description = "Api for video processing server",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -10,
						ApiResourceId = -10,
						Scope = "vps"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -11,
				Created = FirstCreatedTime(),
				Name = "incidentmanagerserver",
				Enabled = true,
				DisplayName = "Incident Manager Server",
				Description = "Api for the Incident Manager server",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -11,
						ApiResourceId = -11,
						Scope = "incidentmanagerserver"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -12,
				Created = FirstCreatedTime(),
				Name = "failover",
				Enabled = true,
				DisplayName = "Failover Server",
				Description = "Api for failover server",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -12,
						ApiResourceId = -12,
						Scope = "failover"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -13,
				Created = FirstCreatedTime(),
				Name = "scim",
				Enabled = true,
				DisplayName = "Scim",
				Description = "Scim Api",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -13,
						ApiResourceId = -13,
						Scope = "scim"
					}
				}
			},
			new Duende.IdentityServer.EntityFramework.Entities.ApiResource
			{
				Id = -14,
				Created = FirstCreatedTime(),
				Name = "serverlogs",
				Enabled = true,
				DisplayName = "Server Logs",
				Description = "Sever Logs Api",
				Scopes = new List<ApiResourceScope>
				{
					new ApiResourceScope
					{
						Id = -14,
						ApiResourceId = -14,
						Scope = "serverlogs"
					}
				}
			}
		};
	}

	private static DateTime FirstCreatedTime()
	{
		return new DateTime(2022, 11, 2).ToUniversalTime();
	}
}

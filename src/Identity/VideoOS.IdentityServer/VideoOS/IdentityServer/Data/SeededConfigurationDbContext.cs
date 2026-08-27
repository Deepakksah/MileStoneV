using System.Collections.Generic;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.DomainEventInfrastructure;

namespace VideoOS.IdentityServer.Data;

public class SeededConfigurationDbContext : ConfigurationDbContext<SeededConfigurationDbContext>
{
	public SeededConfigurationDbContext(DbContextOptions<SeededConfigurationDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		Seed(modelBuilder);
		modelBuilder.ApplyConfiguration(new EventLogsConfiguration(includeInMigrations: false));
	}

	private void Seed(ModelBuilder modelBuilder)
	{
		SeedClients(modelBuilder);
		SeedResources(modelBuilder);
		SeedIdentityResources(modelBuilder);
		SeedApiScopes(modelBuilder);
	}

	private void SeedClients(ModelBuilder modelBuilder)
	{
		IEnumerable<Client> clients = Config.GetClients();
		foreach (Client item in clients)
		{
			if (item.AllowedScopes != null)
			{
				foreach (ClientScope allowedScope in item.AllowedScopes)
				{
					allowedScope.ClientId = item.Id;
					modelBuilder.Entity<ClientScope>().HasData(allowedScope);
				}
			}
			item.AllowedScopes = null;
			if (item.AllowedGrantTypes != null)
			{
				foreach (ClientGrantType allowedGrantType in item.AllowedGrantTypes)
				{
					allowedGrantType.ClientId = item.Id;
					modelBuilder.Entity<ClientGrantType>().HasData(allowedGrantType);
				}
			}
			item.AllowedGrantTypes = null;
			if (item.RedirectUris != null)
			{
				foreach (ClientRedirectUri redirectUri in item.RedirectUris)
				{
					redirectUri.ClientId = item.Id;
					modelBuilder.Entity<ClientRedirectUri>().HasData(redirectUri);
				}
			}
			item.RedirectUris = null;
			if (item.PostLogoutRedirectUris != null)
			{
				foreach (ClientPostLogoutRedirectUri postLogoutRedirectUri in item.PostLogoutRedirectUris)
				{
					postLogoutRedirectUri.ClientId = item.Id;
					modelBuilder.Entity<ClientPostLogoutRedirectUri>().HasData(postLogoutRedirectUri);
				}
			}
			item.PostLogoutRedirectUris = null;
			if (item.IdentityProviderRestrictions != null)
			{
				foreach (ClientIdPRestriction identityProviderRestriction in item.IdentityProviderRestrictions)
				{
					identityProviderRestriction.ClientId = item.Id;
					modelBuilder.Entity<ClientIdPRestriction>().HasData(identityProviderRestriction);
				}
			}
			item.IdentityProviderRestrictions = null;
			if (item.Properties != null)
			{
				foreach (ClientProperty property in item.Properties)
				{
					property.ClientId = item.Id;
					modelBuilder.Entity<ClientProperty>().HasData(property);
				}
			}
			item.Properties = null;
			if (item.ClientSecrets != null)
			{
				foreach (ClientSecret clientSecret in item.ClientSecrets)
				{
					clientSecret.ClientId = item.Id;
					modelBuilder.Entity<ClientSecret>().HasData(clientSecret);
				}
			}
			item.ClientSecrets = null;
			if (item.Claims != null)
			{
				foreach (ClientClaim claim in item.Claims)
				{
					claim.ClientId = item.Id;
					modelBuilder.Entity<ClientClaim>().HasData(claim);
				}
			}
			item.Claims = null;
			if (item.AllowedCorsOrigins != null)
			{
				foreach (ClientCorsOrigin allowedCorsOrigin in item.AllowedCorsOrigins)
				{
					allowedCorsOrigin.ClientId = item.Id;
					modelBuilder.Entity<ClientCorsOrigin>().HasData(allowedCorsOrigin);
				}
			}
			item.AllowedCorsOrigins = null;
			modelBuilder.Entity<Client>().HasData(item);
		}
	}

	private void SeedResources(ModelBuilder modelBuilder)
	{
		IEnumerable<ApiResource> apis = Config.GetApis();
		foreach (ApiResource item in apis)
		{
			if (item.Secrets != null)
			{
				foreach (ApiResourceSecret secret in item.Secrets)
				{
					secret.ApiResourceId = item.Id;
					modelBuilder.Entity<ApiResourceSecret>().HasData(secret);
				}
			}
			item.Secrets = null;
			if (item.Scopes != null)
			{
				foreach (ApiResourceScope scope in item.Scopes)
				{
					scope.ApiResourceId = item.Id;
					modelBuilder.Entity<ApiResourceScope>().HasData(scope);
				}
			}
			item.Scopes = null;
			if (item.UserClaims != null)
			{
				foreach (ApiResourceClaim userClaim in item.UserClaims)
				{
					userClaim.ApiResourceId = item.Id;
					modelBuilder.Entity<ApiResourceClaim>().HasData(userClaim);
				}
			}
			item.UserClaims = null;
			modelBuilder.Entity<ApiResource>().HasData(item);
		}
	}

	private void SeedIdentityResources(ModelBuilder modelBuilder)
	{
		IEnumerable<IdentityResource> identityScopes = Config.GetIdentityScopes();
		foreach (IdentityResource item in identityScopes)
		{
			modelBuilder.Entity<IdentityResource>().HasData(item);
			if (item.UserClaims != null)
			{
				foreach (IdentityResourceClaim userClaim in item.UserClaims)
				{
					userClaim.IdentityResourceId = item.Id;
					modelBuilder.Entity<IdentityResourceClaim>().HasData(userClaim);
				}
			}
			item.UserClaims = null;
		}
	}

	private void SeedApiScopes(ModelBuilder modelBuilder)
	{
		IEnumerable<ApiScope> apiScopes = Config.GetApiScopes();
		foreach (ApiScope item in apiScopes)
		{
			modelBuilder.Entity<ApiScope>().HasData(item);
			if (item.UserClaims != null)
			{
				foreach (ApiScopeClaim userClaim in item.UserClaims)
				{
					userClaim.ScopeId = item.Id;
					modelBuilder.Entity<ApiScopeClaim>().HasData(userClaim);
				}
			}
			item.UserClaims = null;
		}
	}
}

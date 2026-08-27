using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services.KeyManagement;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Data.Migrations.XPCO;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Oem;

namespace VideoOS.IdentityServer.Services;

public class MigratorHostedService : IHostedService
{
	private readonly IServiceProvider _serviceProvider;

	private readonly ILogger _logger;

	private readonly IOemResources _oemResources;

	private readonly PasswordHasherForOldUserMigration _passwordHasher;

	public MigratorHostedService(IServiceProvider serviceProvider, ILogger<MigratorHostedService> logger, IOemResources oemResources, PasswordHasherForOldUserMigration passwordHasher)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
		_oemResources = oemResources;
		_passwordHasher = passwordHasher;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Database migration started...");
		Stopwatch watch = Stopwatch.StartNew();
		using IServiceScope serviceScope = _serviceProvider.CreateScope();
		serviceScope.ServiceProvider.GetRequiredService<SeededConfigurationDbContext>();
		ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await applicationDbContext.Database.MigrateAsync(cancellationToken);
		await serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync(cancellationToken);
		await serviceScope.ServiceProvider.GetRequiredService<DataProtectionKeysDbContext>().Database.MigrateAsync(cancellationToken);
		IdentityDbContextWrapper identityDbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContextWrapper>();
		await identityDbContext.Database.MigrateAsync(cancellationToken);
		SeededConfigurationDbContext configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<SeededConfigurationDbContext>();
		await configurationDbContext.Database.MigrateAsync(cancellationToken);
		await MigrateCorporateBasicUserData(identityDbContext);
		await MigrateAdminIdentitiesData(identityDbContext);
		await SeedMobileOemClient(configurationDbContext);
		ISigningKeyProtector requiredService = serviceScope.ServiceProvider.GetRequiredService<ISigningKeyProtector>();
		ISigningKeyStore requiredService2 = serviceScope.ServiceProvider.GetRequiredService<ISigningKeyStore>();
		await MigrateTokenSigningKeys(applicationDbContext, requiredService2, requiredService);
		await CleanUpMigrateSidClaim(identityDbContext);
		watch.Stop();
		serviceScope.ServiceProvider.GetService<IPostMigratorHostedServiceHandler>()?.Run(serviceScope.ServiceProvider);
		_logger.LogInformation($"Database migration completed in {watch.ElapsedMilliseconds} ms...");
	}

	private async Task MigrateTokenSigningKeys(ApplicationDbContext applicationDbContext, ISigningKeyStore keyStore, ISigningKeyProtector keyProtector)
	{
		CorporateMigrationHelper migration = new CorporateMigrationHelper(applicationDbContext);
		if (!(await migration.TableExists(TableNames.SecurityKeys)))
		{
			return;
		}
		List<SecurityKeyDataModel> securityKeyDataModels = await migration.GetMissingSecurityKeys();
		if (!securityKeyDataModels.Any())
		{
			await migration.DropSecurityKeys();
			return;
		}
		_logger.LogInformation("Migrating security keys to Duende format.");
		IExecutionStrategy strategy = applicationDbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async delegate
		{
			await using IDbContextTransaction transaction = await applicationDbContext.Database.BeginTransactionAsync();
			foreach (SecurityKeyDataModel item in securityKeyDataModels)
			{
				SigningCredentials credentials = RsaKeyGenerator.GetCredentials(item);
				SerializedKey serializedKey = keyProtector.Protect(new RsaKeyContainer((RsaSecurityKey)credentials.Key, "RS256", DateTime.UtcNow));
				serializedKey.Id = serializedKey.Id.ToUpperInvariant();
				await keyStore.StoreKeyAsync(serializedKey);
			}
			await migration.DropSecurityKeys();
			await applicationDbContext.SaveChangesAsync();
			await transaction.CommitAsync();
		});
	}

	private static async Task CleanUpMigrateSidClaim(IdentityDbContextWrapper identityDbContext)
	{
		IQueryable<VmsUserClaim> entities = identityDbContext.UserClaims.Where((VmsUserClaim c) => c.ClaimType.Equals("MigrateSid"));
		identityDbContext.UserClaims.RemoveRange(entities);
		await identityDbContext.SaveChangesAsync();
	}

	private Task SeedMobileOemClient(SeededConfigurationDbContext configurationDbContext)
	{
		Duende.IdentityServer.EntityFramework.Entities.Client client = configurationDbContext?.Clients?.Include((Duende.IdentityServer.EntityFramework.Entities.Client r) => r.RedirectUris).FirstOrDefault((Duende.IdentityServer.EntityFramework.Entities.Client client2) => client2.ClientId.Equals("VmsMobileClient"));
		string returnUrl = _oemResources.IdpReturnUrl;
		if (client != null && client.RedirectUris != null && client.RedirectUris.Count == 1 && !string.IsNullOrWhiteSpace(returnUrl) && Uri.IsWellFormedUriString(returnUrl, UriKind.RelativeOrAbsolute) && client.RedirectUris.SingleOrDefault((ClientRedirectUri uri) => uri.RedirectUri == returnUrl.ToString()) == null)
		{
			_logger.LogInformation("New redirectUri - " + returnUrl + " set for Mobile Client");
			client.RedirectUris.Single().RedirectUri = returnUrl.ToString();
			return configurationDbContext.SaveChangesAsync();
		}
		return Task.CompletedTask;
	}

	private async Task MigrateAdminIdentitiesData(IdentityDbContextWrapper identityDbContext)
	{
		_logger.LogInformation("Migrating admin identities.");
		CorporateMigrationHelper migration = new CorporateMigrationHelper(identityDbContext);
		if (!(await migration.TableExists("TempCorporateAdminIdentities")))
		{
			return;
		}
		List<CorporateMigrationHelper.MigratedAdminIdentity> missingAdminIdentities = await migration.GetMissingAdminIdentities();
		if (!missingAdminIdentities.Any())
		{
			await migration.DropTempAdminIdentities();
			return;
		}
		IExecutionStrategy strategy = identityDbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async delegate
		{
			await using IDbContextTransaction transaction = await identityDbContext.Database.BeginTransactionAsync();
			await migration.AddAdminIdentity(missingAdminIdentities.Select((CorporateMigrationHelper.MigratedAdminIdentity i) => i.Identity).ToList());
			await migration.DropTempAdminIdentities();
			await identityDbContext.SaveChangesAsync();
			await transaction.CommitAsync();
		});
	}

	private async Task MigrateCorporateBasicUserData(IdentityDbContextWrapper identityDbContext)
	{
		_logger.LogInformation("Migrating basic users.");
		CorporateMigrationHelper migration = new CorporateMigrationHelper(identityDbContext);
		if (!(await migration.TableExists("TempCorporateBasicUsers")))
		{
			return;
		}
		List<CorporateMigrationHelper.MigratedBasicUser> missingBasicUsers = await migration.GetMissingBasicUsers();
		if (!missingBasicUsers.Any())
		{
			await migration.DropTempCorporateBasicUsers();
			return;
		}
		IExecutionStrategy strategy = identityDbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async delegate
		{
			await using IDbContextTransaction transaction = await identityDbContext.Database.BeginTransactionAsync();
			List<VmsUser> newUsers = new List<VmsUser>();
			newUsers.AddRange(missingBasicUsers.Select(delegate(CorporateMigrationHelper.MigratedBasicUser n)
			{
				VmsUser vmsUser = VmsUser.Create(n.UserName, n.Description, n.CanChangePassword, forcePasswordChange: false, null, active: true);
				vmsUser.Id = n.Id;
				vmsUser.CreatedDate = n.CreatedDate;
				vmsUser.ModifiedDate = n.ModifiedDate;
				return vmsUser;
			}));
			List<VmsUserClaim> newClaims = new List<VmsUserClaim>();
			using IServiceScope serviceScope = _serviceProvider.CreateScope();
			UserManager<VmsUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<VmsUser>>();
			missingBasicUsers.ForEach(async delegate(CorporateMigrationHelper.MigratedBasicUser n)
			{
				VmsUser identityUser = newUsers.First((VmsUser f) => f.UserName.Equals(n.UserName));
				await userManager.UpdateNormalizedUserNameAsync(identityUser);
				List<VmsUserClaim> list = new List<VmsUserClaim>();
				VmsUserClaim item = new VmsUserClaim
				{
					ClaimType = "MigrateSid",
					ClaimValue = n.Sid,
					UserId = n.Id
				};
				list.Add(item);
				if (n.PasswordHashString != null)
				{
					identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, n.PasswordHashString);
					VmsUserClaim item2 = new VmsUserClaim
					{
						ClaimType = "MigrateSalt",
						ClaimValue = n.Salt,
						UserId = identityUser.Id
					};
					VmsUserClaim item3 = new VmsUserClaim
					{
						ClaimType = "MigrateEncryptionMethod",
						ClaimValue = n.EncryptionMethod,
						UserId = identityUser.Id
					};
					list.Add(item2);
					list.Add(item3);
				}
				newClaims.AddRange(list.ToArray());
			});
			await identityDbContext.Users.AddRangeAsync(newUsers);
			await identityDbContext.UserClaims.AddRangeAsync(newClaims.ToArray());
			await migration.DropTempCorporateBasicUsers();
			await identityDbContext.SaveChangesAsync();
			await transaction.CommitAsync();
		});
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}

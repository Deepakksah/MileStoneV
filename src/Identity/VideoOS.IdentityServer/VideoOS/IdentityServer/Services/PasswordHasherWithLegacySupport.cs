using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Services;

public class PasswordHasherWithLegacySupport : PasswordHasher<VmsUser>
{
	private readonly IServiceProvider _serviceProvider;

	private readonly ILogger<PasswordHasherWithLegacySupport> _logger;

	public PasswordHasherWithLegacySupport(IOptions<PasswordHasherOptions> optionsAccessor, IServiceProvider serviceProvider, ILogger<PasswordHasherWithLegacySupport> logger)
		: base(optionsAccessor)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	public override PasswordVerificationResult VerifyHashedPassword(VmsUser user, string hashedPassword, string providedPassword)
	{
		PasswordVerificationResult passwordVerificationResult = base.VerifyHashedPassword(user, hashedPassword, providedPassword);
		if (passwordVerificationResult != PasswordVerificationResult.Failed)
		{
			return passwordVerificationResult;
		}
		string legacyPasswordHash = GetLegacyPasswordHash(providedPassword, user);
		if (string.IsNullOrEmpty(legacyPasswordHash))
		{
			return PasswordVerificationResult.Failed;
		}
		PasswordVerificationResult passwordVerificationResult2 = base.VerifyHashedPassword(user, hashedPassword, legacyPasswordHash);
		if (passwordVerificationResult2 == PasswordVerificationResult.Failed)
		{
			return passwordVerificationResult2;
		}
		CleanupLegacyPasswordClaims(user);
		return PasswordVerificationResult.SuccessRehashNeeded;
	}

	private void CleanupLegacyPasswordClaims(VmsUser user)
	{
		using IServiceScope serviceScope = _serviceProvider.CreateScope();
		IdentityDbContextWrapper requiredService = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContextWrapper>();
		IQueryable<VmsUserClaim> source = requiredService.UserClaims.Where((VmsUserClaim c) => c.UserId == user.Id).IgnoreQueryFilters();
		IQueryable<VmsUserClaim> entities = source.Where((VmsUserClaim c) => c.ClaimType == "MigrateSalt" || c.ClaimType == "MigrateEncryptionMethod");
		requiredService.UserClaims.RemoveRange(entities);
		requiredService.SaveChanges();
	}

	private string GetLegacyPasswordHash(string providedPassword, VmsUser user)
	{
		using IServiceScope serviceScope = _serviceProvider.CreateScope();
		IdentityDbContextWrapper requiredService = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContextWrapper>();
		IQueryable<VmsUserClaim> source = requiredService.UserClaims.Where((VmsUserClaim c) => c.UserId == user.Id).IgnoreQueryFilters();
		string text = source.FirstOrDefault((VmsUserClaim c) => c.ClaimType == "MigrateSalt")?.ClaimValue;
		string text2 = source.FirstOrDefault((VmsUserClaim c) => c.ClaimType == "MigrateEncryptionMethod")?.ClaimValue;
		if (text != null)
		{
			string result;
			switch (text2)
			{
			case "SHA2_512":
				result = HashedPasswordSha512(providedPassword, text);
				goto IL_01d5;
			case "SHA1":
				result = HashedPasswordSha1(providedPassword, text);
				goto IL_01d5;
			default:
				result = string.Empty;
				goto IL_01d5;
			case null:
				break;
				IL_01d5:
				return result;
			}
		}
		if (text == null != (text2 == null))
		{
			string value = ((text == null) ? "MigrateSalt" : "MigrateEncryptionMethod");
			_logger.LogWarning($"Unable to verify legacy password for user '{user.UserName}' - Reason: {value} not found");
		}
		return string.Empty;
	}

	private static string HashedPasswordSha512(string providedPassword, string salt)
	{
		using SHA512 sHA = SHA512.Create();
		byte[] inArray = sHA.ComputeHash(Encoding.Unicode.GetBytes(providedPassword + salt));
		return Convert.ToBase64String(inArray);
	}

	private static string HashedPasswordSha1(string providedPassword, string salt)
	{
		using SHA1 sHA = SHA1.Create();
		byte[] inArray = sHA.ComputeHash(Encoding.Unicode.GetBytes(providedPassword + salt));
		return Convert.ToBase64String(inArray);
	}
}

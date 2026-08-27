using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Resources;
using VideoOS.IdentityServer.Services;

namespace VideoOS.IdentityServer.Validators;

public sealed class PasswordValidator : IPasswordValidator<VmsUser>
{
	private readonly IPasswordRequirements _passwordRequirements;

	private readonly IPasswordHasher<VmsUser> _passwordHasher;

	public PasswordValidator(IPasswordRequirements passwordRequirements, IPasswordHasher<VmsUser> passwordHasher)
	{
		_passwordRequirements = passwordRequirements ?? throw new ArgumentNullException("passwordRequirements");
		_passwordHasher = passwordHasher ?? throw new ArgumentNullException("passwordHasher");
	}

	public Task<IdentityResult> ValidateAsync(UserManager<VmsUser> manager, VmsUser user, string password)
	{
		if (user == null)
		{
			throw new ArgumentNullException("user");
		}
		if (string.IsNullOrEmpty(password))
		{
			throw new ArgumentException("Value cannot be null or empty.", "password");
		}
		if (!Regex.IsMatch(password, _passwordRequirements.Pattern))
		{
			return Task.FromResult(IdentityResult.Failed(new IdentityError
			{
				Code = "ErrorStrongPassword",
				Description = IdentityResource.ErrorStrongPassword
			}));
		}
		if (string.IsNullOrEmpty(user.PasswordHash))
		{
			return Task.FromResult(IdentityResult.Success);
		}
		PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
		if (passwordVerificationResult == PasswordVerificationResult.Success)
		{
			return Task.FromResult(IdentityResult.Failed(new IdentityError
			{
				Code = "ErrorTheSamePasswords",
				Description = IdentityResource.ErrorTheSamePasswords
			}));
		}
		return Task.FromResult(IdentityResult.Success);
	}
}

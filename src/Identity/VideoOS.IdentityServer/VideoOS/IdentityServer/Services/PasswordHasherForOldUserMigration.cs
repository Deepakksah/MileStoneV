using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VideoOS.IdentityServer.Services;

public class PasswordHasherForOldUserMigration : PasswordHasherWithLegacySupport
{
	public PasswordHasherForOldUserMigration(IOptions<PasswordHasherOptions> options, IServiceProvider serviceProvider, ILogger<PasswordHasherWithLegacySupport> logger)
		: base(options, serviceProvider, logger)
	{
	}
}

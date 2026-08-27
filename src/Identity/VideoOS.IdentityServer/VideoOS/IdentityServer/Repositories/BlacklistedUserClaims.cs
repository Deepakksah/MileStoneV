using System.Collections.Generic;

namespace VideoOS.IdentityServer.Repositories;

public class BlacklistedUserClaims
{
	public static List<string> Blacklist { get; } = new List<string> { "MigrateSalt", "MigrateEncryptionMethod" };
}

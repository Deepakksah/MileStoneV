using System.Collections.Generic;

namespace VideoOS.IdentityServer.Repositories;

public static class SqlErrorCodesToRetryOn
{
	public static ICollection<int> ErrorCodes { get; } = new List<int> { 4060 };
}

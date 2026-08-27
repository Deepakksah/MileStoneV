using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace VideoOS.IdentityServer.Repositories;

public static class IAuthenticationSchemeProviderExtensions
{
	public static bool SchemeExists(this IAuthenticationSchemeProvider authenticationSchemeProvider, string name)
	{
		IEnumerable<AuthenticationScheme> result = authenticationSchemeProvider.GetAllSchemesAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
		return result.Any((AuthenticationScheme s) => s.Name == name);
	}
}

using System.Collections.Generic;

namespace VideoOS.IdentityServer.Common;

public class RegisterClientModel
{
	public string ClientName { get; set; }

	public IEnumerable<string> ClientScopes { get; set; }

	public IEnumerable<string> ClientGrantTypes { get; set; }

	public IEnumerable<string> ClientRedirectUris { get; set; }

	public IEnumerable<string> PostLogoutRedirectUris { get; set; }
}

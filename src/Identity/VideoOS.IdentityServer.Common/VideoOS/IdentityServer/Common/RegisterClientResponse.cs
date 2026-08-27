using System.Collections.Generic;

namespace VideoOS.IdentityServer.Common;

public class RegisterClientResponse
{
	public string Name { get; set; }

	public string Secret { get; set; }

	public IEnumerable<string> Scopes { get; set; }
}

using System.Collections.ObjectModel;

namespace VideoOS.Management.Server;

public class IdpClientData
{
	public int Id { get; set; }

	public string ClientName { get; set; }

	public string ClientId { get; set; }

	public Collection<string> Scopes { get; set; }

	public Collection<string> RedirectUris { get; set; }
}

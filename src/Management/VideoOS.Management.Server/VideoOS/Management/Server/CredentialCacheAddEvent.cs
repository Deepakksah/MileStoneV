using System.Web.Management;

namespace VideoOS.Management.Server;

public class CredentialCacheAddEvent : WebAuditEvent
{
	public CredentialCacheAddEvent(object sender, string username)
		: base("Credential identifier added for: " + username, sender, 100005)
	{
	}
}

using System.Web.Management;

namespace VideoOS.Management.Server;

public class CredentialCacheHitEvent : WebAuditEvent
{
	public CredentialCacheHitEvent(object sender, string username)
		: base("Cache hit for: " + username, sender, 100003)
	{
	}
}

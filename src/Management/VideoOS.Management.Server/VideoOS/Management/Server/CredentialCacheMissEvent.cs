using System.Web.Management;

namespace VideoOS.Management.Server;

public class CredentialCacheMissEvent : WebAuditEvent
{
	public CredentialCacheMissEvent(object sender, string username)
		: base("Cache miss for: " + username, sender, 100003)
	{
	}
}

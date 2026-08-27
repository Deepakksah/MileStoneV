using System.Web.Management;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class AuthenticationSuccessEvent : WebAuthenticationSuccessAuditEvent
{
	public AuthenticationSuccessEvent(object sender, string username)
		: base(Resources.AuthenticationSuccess, sender, 100002, username)
	{
	}
}

using System.Web.Management;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class AuthenticationFailureEvent : WebAuthenticationFailureAuditEvent
{
	public AuthenticationFailureEvent(object sender, string username)
		: base(Resources.AuthenticationFailure, sender, 100001, username)
	{
	}
}

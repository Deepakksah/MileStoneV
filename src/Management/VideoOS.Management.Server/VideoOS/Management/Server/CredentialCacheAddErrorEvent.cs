using System;
using System.Web.Management;

namespace VideoOS.Management.Server;

public class CredentialCacheAddErrorEvent : WebFailureAuditEvent
{
	private Exception _exception;

	public CredentialCacheAddErrorEvent(object sender, string username, Exception exception)
		: base("Failed to add credential identifier for: " + username, sender, 100005)
	{
		_exception = exception;
	}

	public override void FormatCustomEventDetails(WebEventFormatter formatter)
	{
		base.FormatCustomEventDetails(formatter);
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		formatter.AppendLine(_exception.ToString());
	}
}

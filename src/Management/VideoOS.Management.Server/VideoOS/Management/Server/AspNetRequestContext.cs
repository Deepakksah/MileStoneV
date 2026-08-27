using System;
using System.Web;

namespace VideoOS.Management.Server;

public class AspNetRequestContext : GeminiRequestContext
{
	public AspNetRequestContext(GeminiApplicationHost applicationHost, HttpContext context)
		: base(applicationHost, RequestContextType.UserContext)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		base.RemotePort = context.Request.ServerVariables["REMOTE_PORT"];
		base.RemoteIPAddress = context.Request.ServerVariables["REMOTE_ADDR"];
		base.LocalIPAddress = context.Request.ServerVariables["LOCAL_ADDR"];
	}
}

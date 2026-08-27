using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace VideoOS.Management.Server;

public class WcfRequestContext : GeminiRequestContext
{
	public WcfRequestContext(GeminiApplicationHost applicationHost, OperationContext context)
		: base(applicationHost, RequestContextType.UserContext)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (ServiceSecurityContext.Current.WindowsIdentity.User != null)
		{
			SetAuthenticationPropertiesForWindowsUserLegacy();
		}
		else
		{
			SetAuthenticationPropertiesUsingAccessToken();
		}
		ClaimToRoleMapper.CheckForPossibleRoleMappings(this);
		RemoteEndpointMessageProperty remoteEndpointMessageProperty = context.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
		base.RemoteIPAddress = remoteEndpointMessageProperty?.Address;
		base.RemotePort = remoteEndpointMessageProperty?.Port.ToString(CultureInfo.InvariantCulture);
		base.LocalIPAddress = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
	}

	private void SetAuthenticationPropertiesForWindowsUserLegacy()
	{
		base.AuthenticationType = ServiceSecurityContext.Current.WindowsIdentity.AuthenticationType;
		base.AuthenticatedUserSid = ServiceSecurityContext.Current.WindowsIdentity.User.ToString();
		base.AuthenticatedUserDomain = ServiceSecurityContext.Current.WindowsIdentity.Name.Split('\\')[0];
		base.AuthenticatedUserName = ServiceSecurityContext.Current.WindowsIdentity.Name.Split('\\')[1];
	}
}

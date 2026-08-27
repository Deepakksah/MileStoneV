using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public sealed class WcfCallContextInfo : ICallContextInfo
{
	private WindowsIdentity Identity { get; set; }

	public string LocationIPAddress { get; private set; }

	public string UserDomainAccountName
	{
		[DebuggerStepThrough]
		get
		{
			return Identity.Name;
		}
	}

	public string UserSid
	{
		[DebuggerStepThrough]
		get
		{
			return Identity.User.ToString();
		}
	}

	public static WcfCallContextInfo Current
	{
		get
		{
			RemoteEndpointMessageProperty remoteEndpointMessageProperty = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
			return new WcfCallContextInfo(ServiceSecurityContext.Current.WindowsIdentity, remoteEndpointMessageProperty.Address);
		}
	}

	private WcfCallContextInfo(WindowsIdentity identity, string locationIPAddress)
	{
		Identity = identity;
		LocationIPAddress = locationIPAddress;
	}
}

using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace VideoOS.Management.Server;

public class OAuthServiceHostFactory : ServiceHostFactory
{
	protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
	{
		return new OAuthServiceHost(serviceType, baseAddresses);
	}
}

using System;
using System.ServiceModel.Configuration;

namespace VideoOS.Management.Server;

public class BasicAuthenticationExtensionElement : BehaviorExtensionElement
{
	public override Type BehaviorType => typeof(BasicAuthenticationEndpointBehavior);

	protected override object CreateBehavior()
	{
		return new BasicAuthenticationEndpointBehavior();
	}
}

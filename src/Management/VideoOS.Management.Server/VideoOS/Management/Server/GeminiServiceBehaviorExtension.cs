using System;
using System.ServiceModel.Configuration;

namespace VideoOS.Management.Server;

public class GeminiServiceBehaviorExtension : BehaviorExtensionElement
{
	public override Type BehaviorType => typeof(GeminiServiceBehavior);

	protected override object CreateBehavior()
	{
		return new GeminiServiceBehavior();
	}
}

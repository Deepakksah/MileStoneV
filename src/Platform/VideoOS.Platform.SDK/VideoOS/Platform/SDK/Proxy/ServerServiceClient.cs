using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Platform.SDK.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServerServiceClient : ClientBase<IServerService>, IServerService
{
	public ServerServiceClient()
	{
	}

	public ServerServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServerServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string CheckAuthentication()
	{
		return base.Channel.CheckAuthentication();
	}

	public Version GetServerVersion()
	{
		return base.Channel.GetServerVersion();
	}

	public ServerInformation GetServerInformation()
	{
		return base.Channel.GetServerInformation();
	}
}

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.SCH.Services.Agents;

public class ServerCommandProxy : ClientBase<IServerCommand>, IServerCommand
{
	public ServerCommandProxy()
	{
	}

	public ServerCommandProxy(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServerCommandProxy(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetVersion()
	{
		return base.Channel.GetVersion();
	}

	public string GetServerVersion()
	{
		return base.Channel.GetServerVersion();
	}

	public LoginInfo Login(Guid instanceId)
	{
		return base.Channel.Login(instanceId);
	}

	public UserInfo GetUserInfo(string token)
	{
		return base.Channel.GetUserInfo(token);
	}
}

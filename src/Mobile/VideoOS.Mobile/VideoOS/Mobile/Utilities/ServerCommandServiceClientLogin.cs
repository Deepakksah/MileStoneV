using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Common.Proxy.Server.WCF;

namespace VideoOS.Mobile.Utilities;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServerCommandServiceClientLogin : ClientBase<IServerCommandServiceLogin>, IServerCommandServiceLogin
{
	public ServerCommandServiceClientLogin(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		return base.Channel.Login(instanceId, currentToken);
	}
}

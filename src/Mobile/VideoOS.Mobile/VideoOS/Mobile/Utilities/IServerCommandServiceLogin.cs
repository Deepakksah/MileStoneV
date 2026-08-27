using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Common.Proxy.Server.WCF;

namespace VideoOS.Mobile.Utilities;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand", ConfigurationName = "VideoOS.Common.Proxy.Server.WCF.IServerCommandService")]
public interface IServerCommandServiceLogin
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Login", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LoginResponse")]
	LoginInfo Login(Guid instanceId, string currentToken);
}

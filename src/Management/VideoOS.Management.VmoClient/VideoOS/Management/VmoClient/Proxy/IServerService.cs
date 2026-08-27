using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Management.VmoClient.Proxy;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/ServerService/2008/05/Vmo/01", ConfigurationName = "VideoOS.Management.VmoClient.Proxy.IServerService")]
public interface IServerService
{
	[OperationContract(Action = "http://schemas.videoos/ServerService/2008/05/Vmo/01/IServerService/CheckAuthentication", ReplyAction = "http://schemas.videoos/ServerService/2008/05/Vmo/01/IServerService/CheckAuthenticationResponse")]
	string CheckAuthentication();

	[OperationContract(Action = "http://schemas.videoos/ServerService/2008/05/Vmo/01/IServerService/GetServerVersion", ReplyAction = "http://schemas.videoos/ServerService/2008/05/Vmo/01/IServerService/GetServerVersionResponse")]
	Version GetServerVersion();
}

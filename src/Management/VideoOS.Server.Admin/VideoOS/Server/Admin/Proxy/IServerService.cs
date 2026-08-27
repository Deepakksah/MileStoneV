using System.ServiceModel;

namespace VideoOS.Server.Admin.Proxy;

[ServiceContract(Namespace = "http://schemas.videoos/ServerService/2008/05/Vmo/01", ConfigurationName = "Proxy.IServerService")]
public interface IServerService
{
	[OperationContract(Action = "http://schemas.videoos/ServerService/2008/05/Vmo/01/IServerService/CheckAuthentication", ReplyAction = "http://schemas.videoos/ServerService/2008/05/Vmo/01/IServerService/CheckAuthenticationResponse")]
	string CheckAuthentication();
}

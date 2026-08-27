using System;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://schemas.videoos/ServerService/2008/05/Vmo/01")]
public interface IServerService
{
	[OperationContract]
	string CheckAuthentication();

	[OperationContract]
	Version GetServerVersion();

	[OperationContract]
	ServerInformationOld GetServerInformation();

	[OperationContract]
	ServerInformationOld UpdateServerInformation(string name, string description, ServerAddress[] serverAddresses);
}

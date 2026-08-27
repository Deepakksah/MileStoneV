using System;
using System.ServiceModel;

namespace VideoOS.SCH.Services.Agents;

[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public interface IServerCommand
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/GetVersion")]
	int GetVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/GetServerVersion")]
	string GetServerVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/Login")]
	LoginInfo Login(Guid instanceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/GetUserInfo")]
	UserInfo GetUserInfo(string token);
}

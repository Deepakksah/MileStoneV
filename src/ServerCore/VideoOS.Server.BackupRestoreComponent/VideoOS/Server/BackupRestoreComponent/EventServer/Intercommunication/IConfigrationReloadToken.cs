using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerConfigrationReloadToken", ConfigurationName = "VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication.IConfigrationReloadToken", SessionMode = SessionMode.NotAllowed)]
public interface IConfigrationReloadToken
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfiguration", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationResponse")]
	[FaultContract(typeof(ConfigurationReloadServiceFault), Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationConfigurationReloadServiceFaultFault", Name = "ConfigurationReloadServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationReloadCommandToken")]
	void ReloadConfiguration(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfiguration", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationResponse")]
	Task ReloadConfigurationAsync(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationStarted", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationStartedResponse")]
	[FaultContract(typeof(ConfigurationReloadServiceFault), Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationStartedConfigurationReloadServiceFaultFault", Name = "ConfigurationReloadServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationReloadCommandToken")]
	bool IsReloadConfigurationStarted(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationStarted", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationStartedResponse")]
	Task<bool> IsReloadConfigurationStartedAsync(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationDone", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationDoneResponse")]
	[FaultContract(typeof(ConfigurationReloadServiceFault), Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationDoneConfigurationReloadServiceFaultFault", Name = "ConfigurationReloadServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationReloadCommandToken")]
	bool IsReloadConfigurationDone(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationDone", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/IsReloadConfigurationDoneResponse")]
	Task<bool> IsReloadConfigurationDoneAsync(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationProgress", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationProgressResponse")]
	[FaultContract(typeof(ConfigurationReloadServiceFault), Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationProgressConfigurationReloadServiceFaultFault", Name = "ConfigurationReloadServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationReloadCommandToken")]
	int ReloadConfigurationProgress(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationProgress", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/ReloadConfigurationProgressResponse")]
	Task<int> ReloadConfigurationProgressAsync(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/GetServiceAccount", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/GetServiceAccountResponse")]
	[FaultContract(typeof(ConfigurationReloadServiceFault), Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/GetServiceAccountConfigurationReloadServiceFaultFault", Name = "ConfigurationReloadServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationReloadCommandToken")]
	ConfigurationReloadUserID GetServiceAccount(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/GetServiceAccount", ReplyAction = "http://videoos.net/2/CentralServerConfigrationReloadToken/IConfigrationReloadToken/GetServiceAccountResponse")]
	Task<ConfigurationReloadUserID> GetServiceAccountAsync(string token);
}

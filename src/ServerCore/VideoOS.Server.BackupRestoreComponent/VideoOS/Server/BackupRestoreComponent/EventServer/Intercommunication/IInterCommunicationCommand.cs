using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand", ConfigurationName = "VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication.IInterCommunicationCommand", SessionMode = SessionMode.NotAllowed)]
public interface IInterCommunicationCommand
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/GetStatus", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/GetStatusResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/GetStatusMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapObjectState[] GetStatus(InterCommunicationUserID[] userIDs, string ID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/ReloadConfiguration", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/ReloadConfigurationResponse")]
	void ReloadConfiguration();

	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/IsReloadConfigurationStarted", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/IsReloadConfigurationStartedResponse")]
	bool IsReloadConfigurationStarted();

	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/IsReloadConfigurationDone", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/IsReloadConfigurationDoneResponse")]
	bool IsReloadConfigurationDone();

	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/ReloadConfigurationProgress", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/ReloadConfigurationProgressResponse")]
	int ReloadConfigurationProgress();

	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/GetServiceAccount", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommand/IInterCommunicationCommand/GetServiceAccountResponse")]
	InterCommunicationUserID GetServiceAccount();
}

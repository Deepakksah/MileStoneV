using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.EventServer.Communication.AlarmSecurity.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerAlarmSecurity", ConfigurationName = "VideoOS.EventServer.Communication.AlarmSecurity.Client.IAlarmSecurity", SessionMode = SessionMode.NotAllowed)]
public interface IAlarmSecurity
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurity/IAlarmSecurity/GetUsersPermittedByObject", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurity/IAlarmSecurity/GetUsersPermittedByObjectResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurity/IAlarmSecurity/GetUsersPermittedByObjectAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetUsersPermittedByObject(string objectId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurity/IAlarmSecurity/GetAllUsers", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurity/IAlarmSecurity/GetAllUsersResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurity/IAlarmSecurity/GetAllUsersAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetAllUsers();
}

using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.EventServer.Communication.AlarmSecurity.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerAlarmSecurityToken", ConfigurationName = "VideoOS.EventServer.Communication.AlarmSecurity.Client.IAlarmSecurityToken", SessionMode = SessionMode.NotAllowed)]
public interface IAlarmSecurityToken
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByObject", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByObjectResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByObjectAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetUsersPermittedByObject(string token, string objectId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByObjectList", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByObjectListResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByObjectListAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetUsersPermittedByObjectList(string token, string[] objectIdList);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByKind", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByKindResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByKindAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetUsersPermittedByKind(string token, Guid type);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByKindOnServer", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByKindOnServerResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetUsersPermittedByKindOnServerAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetUsersPermittedByKindOnServer(string token, Guid type, string[] serverIds);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetAllUsers", ReplyAction = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetAllUsersResponse")]
	[FaultContract(typeof(AlarmSecurityServiceFault), Action = "http://videoos.net/2/CentralServerAlarmSecurityToken/IAlarmSecurityToken/GetAllUsersAlarmSecurityServiceFaultFault", Name = "AlarmSecurityServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Identity[] GetAllUsers(string token);
}

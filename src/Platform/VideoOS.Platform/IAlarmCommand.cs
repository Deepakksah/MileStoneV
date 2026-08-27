using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerAlarmCommand", ConfigurationName = "IAlarmCommand", SessionMode = SessionMode.NotAllowed)]
public interface IAlarmCommand
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/Add", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AddResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AddAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void Add(Alarm alarm);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AddEvent", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AddEventResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AddEventAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Alarm")]
	void AddEvent(BaseEvent baseEvent);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/Get", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Alarm Get(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEvent", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	BaseEvent GetEvent(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetHeader", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetHeaderResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetHeaderAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	EventHeader GetHeader(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSnapshots", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSnapshotsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSnapshotsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	SnapshotList GetSnapshots(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AttachSnapshot", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AttachSnapshotResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AttachSnapshotAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AttachSnapshot(Guid id, Snapshot snapshot);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateAlarmValues", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateAlarmValuesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateAlarmValuesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateAlarm", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateAlarmResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateAlarmAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/Acknowledge", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AcknowledgeResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AcknowledgeAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void Acknowledge(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateMultipleAlarms", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateMultipleAlarmsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/UpdateMultipleAlarmsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AcknowledgeMultipleAlarms", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AcknowledgeMultipleAlarmsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/AcknowledgeMultipleAlarmsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AcknowledgeMultipleAlarms(Guid objectId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StartAlarmLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StartAlarmLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StartAlarmLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Guid StartAlarmLineSession(AlarmFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StopAlarmLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StopAlarmLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StopAlarmLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void StopAlarmLineSession(Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSessionAlarmLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSessionAlarmLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSessionAlarmLinesLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StartEventLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StartEventLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StartEventLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Guid StartEventLineSession(EventFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StopEventLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StopEventLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/StopEventLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void StopEventLineSession(Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSessionEventLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSessionEventLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetSessionEventLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	EventLine[] GetEventLines(int from, int maxCount, EventFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetStatistics", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetStatisticsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetStatisticsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Statistic[] GetStatistics();

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmHistory", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmHistoryResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmHistoryAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmHistory[] GetAlarmHistory(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmUpdateHistory", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmUpdateHistoryResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmUpdateHistoryAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmUpdate[] GetAlarmUpdateHistory(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmMessages", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmMessagesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmMessagesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	string[] GetAlarmMessages();
}

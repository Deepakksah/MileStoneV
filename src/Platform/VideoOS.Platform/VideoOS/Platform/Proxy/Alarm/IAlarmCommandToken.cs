using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Proxy.Alarm;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerAlarmCommand", ConfigurationName = "IAlarmCommandToken", SessionMode = SessionMode.NotAllowed)]
public interface IAlarmCommandToken
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/Add", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void Add(string token, VideoOS.Platform.Data.Alarm alarm);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddEvent", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddEventResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddEventAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AddEvent(string token, BaseEvent baseEvent);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/Get", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	VideoOS.Platform.Data.Alarm Get(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEvent", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEventResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEventAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	BaseEvent GetEvent(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetHeader", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetHeaderResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetHeaderAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	EventHeader GetHeader(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSnapshots", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSnapshotsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSnapshotsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	SnapshotList GetSnapshots(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AttachSnapshot", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AttachSnapshotResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AttachSnapshotAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AttachSnapshot(string token, Guid id, Snapshot snapshot);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateAlarmValues", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateAlarmValuesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateAlarmValuesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void UpdateAlarmValues(string token, Guid id, KeyValuePair<string, string>[] updates);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateAlarm", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateAlarmResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateAlarmAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void UpdateAlarm(string token, Guid id, string text, int state, int priority, DateTime time, string assignedTo);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/Acknowledge", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AcknowledgeResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AcknowledgeAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void Acknowledge(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateMultipleAlarms", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateMultipleAlarmsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/UpdateMultipleAlarmsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void UpdateMultipleAlarms(string token, Guid objectId, int oldState, int newState, DateTime time, string assignedTo);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AcknowledgeMultipleAlarms", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AcknowledgeMultipleAlarmsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AcknowledgeMultipleAlarmsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AcknowledgeMultipleAlarms(string token, Guid objectId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartAlarmLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartAlarmLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartAlarmLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Guid StartAlarmLineSession(string token, AlarmFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopAlarmLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopAlarmLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopAlarmLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void StopAlarmLineSession(string token, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionAlarmLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionAlarmLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionAlarmLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmUpdateData GetSessionAlarmLines(string token, int maxCount, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmLine[] GetAlarmLines(string token, int from, int maxCount, AlarmFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartEventLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartEventLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartEventLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Guid StartEventLineSession(string token, EventFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopEventLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopEventLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopEventLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void StopEventLineSession(string token, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionEventLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEventLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionEventLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	EventUpdateData GetSessionEventLines(string token, int maxCount, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEventLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEventLinesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetEventLinesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	EventLine[] GetEventLines(string token, int from, int maxCount, EventFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetStatistics", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetStatisticsResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetStatisticsAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Statistic[] GetStatistics(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmHistory", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmHistoryResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmHistoryAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmHistory[] GetAlarmHistory(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmUpdateHistory", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmUpdateHistoryResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmUpdateHistoryAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmUpdate[] GetAlarmUpdateHistory(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmMessages", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmMessagesResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmMessagesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	string[] GetAlarmMessages(string token);
}

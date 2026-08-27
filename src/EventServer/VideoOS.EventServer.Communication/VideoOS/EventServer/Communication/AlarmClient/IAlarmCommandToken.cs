using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerAlarmCommand", ConfigurationName = "VideoOS.Event.PlatformPlugin.AlarmCommandClient.IAlarmCommandToken", SessionMode = SessionMode.NotAllowed)]
public interface IAlarmCommandToken
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/Add", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void Add(string token, Alarm alarm);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddEvent", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddEventResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddEventAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AddEvent(string token, BaseEvent baseEvent);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddAsEvent", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddAsEventResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/AddAsEventAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void AddAsEvent(string token, Alarm alarm);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/Get", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Alarm Get(string token, Guid id);

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

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmsWithRelatedHardware", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmsWithRelatedHardwareResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmsWithRelatedHardwareAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	AlarmLine[] GetAlarmsWithRelatedHardware(string token, int maxCount, AlarmSearchCriteria[] filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmsWithRelatedHardware", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetAlarmsWithRelatedHardwareResponse")]
	Task<AlarmLine[]> GetAlarmsWithRelatedHardwareAsync(string token, int maxCount, AlarmSearchCriteria[] filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartEventLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartEventLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StartEventLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	Guid StartEventLineSession(string token, EventFilter filter);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopEventLineSession", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopEventLineSessionResponse")]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/StopEventLineSessionAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	void StopEventLineSession(string token, Guid sessionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionEventLines", ReplyAction = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommandToken/GetSessionEventLinesResponse")]
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

	[OperationContract(AsyncPattern = true)]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetAlarmMessagesAlarmServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	IAsyncResult BeginGetEventLines2(string token, GetEventLines2Params parameters, AsyncCallback callback, object asyncState);

	GetEventLines2Resp EndGetEventLines2(IAsyncResult result);

	[OperationContract(AsyncPattern = true)]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/BeginGetAccessControlEventsServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	IAsyncResult BeginGetAccessControlEvents(string token, GetAccessControlEventsParams parameters, AsyncCallback callback, object asyncState);

	GetAccessControlEventsResp EndGetAccessControlEvents(IAsyncResult result);

	[OperationContract(AsyncPattern = true)]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventCountServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	IAsyncResult BeginGetEventCount(string token, GetEventCountParams parameters, AsyncCallback callback, object asyncState);

	GetEventCountResp EndGetEventCount(IAsyncResult result);

	[OperationContract(AsyncPattern = true)]
	[FaultContract(typeof(AlarmServiceFault), Action = "http://videoos.net/2/CentralServerAlarmCommand/IAlarmCommand/GetEventsServiceFaultFault", Name = "AlarmServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
	IAsyncResult BeginGetEvents(string token, GetEventsParams parameters, AsyncCallback callback, object asyncState);

	GetEventsResp EndGetEvents(IAsyncResult result);
}

using System;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.Platform.Proxy.AlarmClient;

public interface IAlarmClient
{
	CommunicationState State { get; }

	void Add(VideoOS.Platform.Data.Alarm alarm);

	[Obsolete("Do not use this method. Use AddEvent instead")]
	void AddAsEvent(VideoOS.Platform.Data.Alarm alarm);

	void AddEvent(BaseEvent baseEvent);

	VideoOS.Platform.Data.Alarm Get(Guid id);

	BaseEvent GetEvent(Guid id);

	EventHeader GetHeader(Guid id);

	SnapshotList GetSnapshots(Guid id);

	void AttachSnapshot(Guid id, Snapshot snapshot);

	void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo);

	void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo);

	void Acknowledge(Guid id);

	void AcknowledgeMultipleAlarms(Guid objectId);

	AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter);

	EventLine[] GetEventLines(int from, int maxCount, EventFilter filter);

	Statistic[] GetStatistics();

	AlarmHistory[] GetAlarmHistory(Guid id);

	AlarmUpdate[] GetAlarmUpdateHistory(Guid id);

	string[] GetAlarmMessages();

	void OpenClient();

	void CloseClient();

	Guid StartAlarmLineSession(AlarmFilter filter);

	AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId);

	void StopAlarmLineSession(Guid sessionId);

	Guid StartEventLineSession(EventFilter filter);

	EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId);

	void StopEventLineSession(Guid sessionId);

	void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates);
}

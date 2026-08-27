using System;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.AlarmClient;

public interface IAlarmClientFactory
{
	AlarmClient Create(ServerId serverId, string eventServerAddress, Version eventServerVersion, string alarmServerAddress);
}

using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAlarmsSync
{
	GetAlarmListResponse GetAlarmList(GetAlarmParams getAlarmParams, TimeSpan timeout);

	BaseCommandResponse UpdateAlarm(UpdateAlarmParams updateAlarmParams, TimeSpan timeout);

	GetAlarmDataSettingsResponse GetAlarmDataSettings(TimeSpan timeout);

	BaseCommandResponse GetPermittedUsers(PermittedUsersParams permittedUsersParams, TimeSpan timeout);

	BaseCommandResponse AcknowledgeAlarm(AcknowledgeAlarmParams acknowledgeAlarmParams, TimeSpan timeout);
}

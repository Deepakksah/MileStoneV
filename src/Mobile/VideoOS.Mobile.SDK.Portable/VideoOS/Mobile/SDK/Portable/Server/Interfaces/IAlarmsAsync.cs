using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAlarmsAsync
{
	IAsyncResult GetAlarmList(GetAlarmParams getAlarmParams, OnResponse<GetAlarmListResponse> onSuccess = null, OnResponse<GetAlarmListResponse> onFail = null);

	IAsyncResult UpdateAlarm(UpdateAlarmParams updateAlarmParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult GetAlarmDataSettings(OnResponse<GetAlarmDataSettingsResponse> onSuccess = null, OnResponse<GetAlarmDataSettingsResponse> onFail = null);

	IAsyncResult GetPermittedUsers(PermittedUsersParams permittedUsersParams, OnResponse<GetPermittedUsersResponse> onSuccess = null, OnResponse<GetPermittedUsersResponse> onFail = null);

	IAsyncResult AcknowledgeAlarm(AcknowledgeAlarmParams acknowledgeAlarmParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}

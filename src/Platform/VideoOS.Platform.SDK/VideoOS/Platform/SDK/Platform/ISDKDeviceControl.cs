using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform;

public interface ISDKDeviceControl
{
	void ReservePTZ(UserContext userContext, Guid cameraGuid, bool reserve);

	void ExtendPTZ(UserContext userContext, Guid cameraGuid);

	PtzSessionInfo GetPTZSessionInfo(UserContext userContext, Guid cameraGuid);

	void PTZMoveStart(UserContext userContext, Guid cameraGuid, PTZArgs ptzArgs);

	PTZArgs PTZGetAbsolutePosition(UserContext userContext, Guid cameraGuid);

	void PTZSetAbsolutePosition(UserContext userContext, Guid cameraId, PTZArgs ptzArgs);

	void GotoPreset(UserContext userContext, Guid cameraId, string presetId);

	void SetAux(UserContext userContext, Guid cameraGuid, int auxNo, bool onOrOff);

	string DriverCommandWithResponse(UserContext userContext, Guid anyDeviceId, string command, string parameter);

	void PTZActivatePatrollingSchema(UserContext userContext, Guid cameraId, Guid patrollingSchemaId);

	void PTZDeactivatePatrollingSchema(UserContext userContext, Guid cameraId, Guid patrollingSchemaId);

	void OutputActivate(UserContext userContext, Guid outputId);

	void OutputDeactivate(UserContext userContext, Guid outputId);

	IOState OutputGetState(UserContext userContext, Guid outputId);

	void OutputTrigger(UserContext userContext, Guid outputId);

	void StartRecording(UserContext userContext, List<Guid> anyDeviceIds);

	void StopRecording(UserContext userContext, List<Guid> anyDeviceIds);

	Tuple<DateTime, string> GetVideoFrame(UserContext userContext, Guid cameraId, DateTime atTimeStamp);
}

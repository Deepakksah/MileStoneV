using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public interface IRecorder
{
	Guid Instance { get; }

	string XmlEncoding { get; }

	string Name { get; }

	DateTime LastModified { get; }

	Guid Guid { get; }

	string HostName { get; }

	Uri WebServerUri { get; }

	int Port { get; }

	ServerId ServerId { get; }

	object LockObject { get; }

	CameraCollection Cameras { get; }

	MicrophoneCollection Microphones { get; }

	SpeakerCollection Speakers { get; }

	UserDefinedEventCollection Events { get; }

	OutputDeviceCollection OutputDevices { get; }

	MatrixMonitorCollection MatrixMonitors { get; }

	IDictionary<Guid, IHardwareInfo> Hardware { get; }

	IDictionary<Guid, IMetadataDevice> MetadataDevices { get; }

	SequenceType[] GetSequenceTypes(Guid deviceGuid);

	SequenceCollection GetSequenceAtTimeMinutes(Guid deviceGuid, DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType);

	TimeLineInformationCollection GetTimeLineInformation(Guid deviceGuid, Guid[] types, DateTime beginTime, int interval, int maxCount);

	void MoveToPreset(string deviceId, string presetName, bool throwOnError);

	void CameraPtzMoveAction(ICamera device, PtzMoveRelativeTypes ptzMoveType);

	void CameraPtzMoveAction(ICamera device, PtzMoveRelativeTypes ptzMoveType, double speed, double step);

	void CameraPtzMoveActionAbsolute(ICamera camera, double pan, double tilt, double zoom);

	void CameraPtzMoveActionAbsolute(ICamera camera, PTZArgs ptzArgs, bool throwOnError);

	void CameraPtzMoveStart(ICamera camera, PTZArgs ptzArgs, bool throwOnError);

	void CameraPtzMoveStart(Guid cameraGuid, PTZArgs ptzargs, bool throwOnError);

	void CameraPtzMoveRelative(Guid cameraGuid, PTZArgs ptzargs);

	PTZArgs PtzGetAbsolutePosition(ICamera Camera, bool throwOnError);

	void PtzMoveHome(ICamera camera);

	void CameraPtzAuxOn(ICamera camera, int auxNumber, bool throwOnError);

	void CameraPtzAuxOff(ICamera camera, int auxNumber, bool throwOnError);

	void CameraPtzMoveStop(ICamera camera);

	void CameraPtzCenter(ICamera camera, double refWidth, double refHeight, double centerX, double centerY, double zoom);

	void CameraPtzRectangle(ICamera camera, double refWidth, double refHeight, double left, double top, double bottom, double right);

	void ExtendPTZ(Guid cameraGuid, bool throwOnError);

	void ReservePTZ(Guid cameraGuid, bool reserve, bool throwOnError);

	void OutputActivate(string deviceId, bool throwOnError);

	void OutputDeactive(string deviceId, bool throwOnError);

	IOState OutputGetState(string deviceId, bool throwOnError);

	void OuputTrigger(string deviceId, int duration, bool throwOnError);

	VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ConvertPTZArgsToService(PTZArgs ptzArgs);

	PTZArgs ConvertPTZArgsFromService(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs);

	Task<MulticastInfo[]> AsyncMulticastStart(List<Guid> sessionIds, ClientCapabilities clientCapabilities);

	Task AsyncMulticastStop(List<Guid> sessionIds);

	Task AsyncMulticastContinue(Guid[] sessionIds);

	Task<string> AsyncDriverCommandWithResponse(Guid cameraGuid, string command, string parameter);

	string DriverCommandWithResponse(Guid cameraGuid, string command, string parameter, bool throwOnError);

	PtzSessionInfo GetPTZSessionInfo(Guid cameraGuid, bool throwOnError);

	Tuple<DateTime, string> GetLiveJpeg(Guid cameraId, int width, int height);

	Tuple<DateTime, string> GetJpeg(Guid cameraId, DateTime atTimeStamp);

	void PTZActivatePatrollingSchema(Guid deviceId, Guid patrollingSchemaId, bool throwOnError);

	void PTZDeactivatePatrollingSchema(Guid deviceId, Guid patrollingSchemaId, bool throwOnError);
}

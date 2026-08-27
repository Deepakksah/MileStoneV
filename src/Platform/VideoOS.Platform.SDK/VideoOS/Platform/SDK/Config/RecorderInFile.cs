using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class RecorderInFile : IRecorder
{
	private readonly ISystemInfo _systemInfo;

	private readonly string _name;

	private readonly Guid _guid;

	private readonly ServerId _serverId;

	private readonly IDictionary<Guid, IHardwareInfo> _hardware = new Dictionary<Guid, IHardwareInfo>();

	private readonly object _lockObject = new object();

	private Guid _instance = Guid.NewGuid();

	public Guid Instance => _instance;

	public string XmlEncoding => "utf-8";

	public object LockObject => _lockObject;

	public string Name => _name;

	public DateTime LastModified => DateTime.MinValue;

	public Guid Guid => _guid;

	public string HostName => _serverId.ServerHostname;

	public Uri WebServerUri => new Uri("file:\\" + _serverId.ServerHostname);

	public int Port => 0;

	public ServerId ServerId => _serverId;

	public CameraCollection Cameras => _systemInfo.Cameras;

	public MicrophoneCollection Microphones => _systemInfo.Microphones;

	public SpeakerCollection Speakers => _systemInfo.Speakers;

	public UserDefinedEventCollection Events => _systemInfo.Events;

	public OutputDeviceCollection OutputDevices => _systemInfo.OutputDevices;

	public MatrixMonitorCollection MatrixMonitors => _systemInfo.MatrixMonitors;

	public IDictionary<Guid, IMetadataDevice> MetadataDevices => _systemInfo.MetadataDevices;

	public IDictionary<Guid, IHardwareInfo> Hardware => _hardware;

	public RecorderInFile(ISystemInfo systemInfo, string name, Guid guid, ServerId serverId)
	{
		_systemInfo = systemInfo;
		_name = name;
		_guid = guid;
		_serverId = serverId;
	}

	public static void UpdateOrAdd(XmlDocument doc)
	{
	}

	public SequenceType[] GetSequenceTypes(Guid deviceGuid)
	{
		throw new NotImplementedException();
	}

	public SequenceCollection GetSequenceAtTimeMinutes(Guid deviceGuid, DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		throw new NotImplementedException();
	}

	public TimeLineInformationCollection GetTimeLineInformation(Guid deviceGuid, Guid[] types, DateTime beginTime, int interval, int maxCount)
	{
		throw new NotImplementedException();
	}

	public void MoveToPreset(string deviceId, string presetName, bool throwOnError)
	{
	}

	public void CameraPtzMoveAction(ICamera device, PtzMoveRelativeTypes ptzMoveType)
	{
	}

	public void CameraPtzMoveAction(ICamera device, PtzMoveRelativeTypes ptzMoveType, double speed, double step)
	{
	}

	public void CameraPtzMoveActionAbsolute(ICamera camera, double pan, double tilt, double zoom)
	{
	}

	public void CameraPtzMoveActionAbsolute(ICamera camera, PTZArgs ptzArgs, bool throwOnError)
	{
	}

	public void CameraPtzMoveStart(Guid cameraGuid, PTZArgs ptzargs, bool throwOnError)
	{
	}

	public void CameraPtzMoveRelative(Guid cameraGuid, PTZArgs ptzargs)
	{
	}

	public PTZArgs PtzGetAbsolutePosition(ICamera Camera, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	public void PtzMoveHome(ICamera camera)
	{
	}

	public void CameraPtzMoveStart(ICamera camera, PTZArgs ptzArgs, bool throwOnError)
	{
	}

	public void CameraPtzAuxOn(ICamera camera, int auxNumber, bool throwOnError)
	{
	}

	public void CameraPtzAuxOff(ICamera camera, int auxNumber, bool throwOnError)
	{
	}

	public void CameraPtzMoveStop(ICamera camera)
	{
	}

	public void CameraPtzCenter(ICamera camera, double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		throw new NotImplementedException();
	}

	public void CameraPtzRectangle(ICamera camera, double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
	}

	public void OutputActivate(string deviceId, bool throwOnError)
	{
	}

	public void OutputDeactive(string deviceId, bool throwOnError)
	{
	}

	public IOState OutputGetState(string deviceId)
	{
		throw new NotImplementedException();
	}

	public void OuputTrigger(string deviceId, int duration, bool throwOnError)
	{
	}

	public VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ConvertPTZArgsToService(PTZArgs ptzArgs)
	{
		throw new NotImplementedException();
	}

	public PTZArgs ConvertPTZArgsFromService(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs)
	{
		throw new NotImplementedException();
	}

	public Task AsyncMulticastContinue(Guid[] sessionIds)
	{
		throw new NotImplementedException();
	}

	public Task<MulticastInfo[]> AsyncMulticastStart(List<Guid> sessionIds, ClientCapabilities clientCapabilities)
	{
		throw new NotImplementedException();
	}

	public Task AsyncMulticastStop(List<Guid> sessionIds)
	{
		throw new NotImplementedException();
	}

	public Task<string> AsyncDriverCommandWithResponse(Guid cameraGuid, string command, string parameter)
	{
		throw new NotImplementedException();
	}

	public Tuple<DateTime, string> GetLiveJpeg(Guid cameraId, int width, int height)
	{
		throw new NotImplementedException();
	}

	public Tuple<DateTime, string> GetJpeg(Guid cameraId, DateTime atTimeStamp)
	{
		throw new NotImplementedException();
	}

	public void ExtendPTZ(Guid cameraGuid, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	public void ReservePTZ(Guid cameraGuid, bool reserve, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	IOState IRecorder.OutputGetState(string deviceId, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	public string DriverCommandWithResponse(Guid cameraGuid, string command, string parameter, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	public PtzSessionInfo GetPTZSessionInfo(Guid cameraGuid, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	public void PTZActivatePatrollingSchema(Guid deviceId, Guid patrollingSchemaId, bool throwOnError)
	{
		throw new NotImplementedException();
	}

	public void PTZDeactivatePatrollingSchema(Guid deviceId, Guid patrollingSchemaId, bool throwOnError)
	{
		throw new NotImplementedException();
	}
}

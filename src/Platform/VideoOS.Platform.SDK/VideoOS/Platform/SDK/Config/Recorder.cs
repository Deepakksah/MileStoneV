#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Config;

public class Recorder : IRecorder
{
	private readonly ISystemInfo _systemInfo;

	private IRecorderInfo _recorderInfo;

	private readonly ServerId _serverId;

	private CameraCollection _cameras;

	private MicrophoneCollection _microphones;

	private SpeakerCollection _speakers;

	private UserDefinedEventCollection _userDefinedEvents;

	private OutputDeviceCollection _outputs;

	private IDictionary<Guid, IMetadataDevice> _metadataDevices = new Dictionary<Guid, IMetadataDevice>();

	private IDictionary<Guid, IHardwareInfo> _hardware = new Dictionary<Guid, IHardwareInfo>();

	private string _name;

	private readonly Guid _guid;

	private RecorderCommandServiceSoapClient _recorderApiClient;

	private object _lockObject = new object();

	private Guid _instance = Guid.NewGuid();

	public Guid Instance => _instance;

	public object LockObject => _lockObject;

	public string XmlEncoding
	{
		get
		{
			if (_recorderInfo == null || _recorderInfo.XmlEncoding == null)
			{
				return string.Empty;
			}
			return _recorderInfo.XmlEncoding;
		}
	}

	public string Name => _name;

	public DateTime LastModified
	{
		get
		{
			return _recorderInfo.LastModified;
		}
		set
		{
			if (_recorderInfo != null)
			{
				_recorderInfo.LastModified = value;
			}
		}
	}

	public Guid Guid => _guid;

	public string Scheme => _serverId.ServerScheme;

	public string HostName => _serverId.ServerHostname;

	public Uri WebServerUri
	{
		get
		{
			try
			{
				return new Uri(_recorderInfo.WebServerUri);
			}
			catch (Exception ex)
			{
				EnvironmentManager instance = EnvironmentManager.Instance;
				string message = ex.Message;
				object[] args = new Exception[1] { ex };
				instance.Log(error: true, "Recorder:WebServerUri", message, args);
				return null;
			}
		}
		set
		{
			if (_recorderInfo != null)
			{
				DisposeRecorderCommandService(_recorderApiClient);
				_recorderApiClient = null;
				_recorderInfo.WebServerUri = value.ToString();
				_serverId.ServerScheme = value.Scheme;
				_serverId.ServerHostname = value.DnsSafeHost;
				_serverId.Serverport = value.Port;
				_recorderApiClient = GenerateRecorderCommandService(GetRecorderCommandUri());
			}
		}
	}

	public int Port => _serverId.Serverport;

	public CameraCollection Cameras => _cameras;

	public MicrophoneCollection Microphones => _microphones;

	public SpeakerCollection Speakers => _speakers;

	public UserDefinedEventCollection Events => _userDefinedEvents;

	public OutputDeviceCollection OutputDevices => _outputs;

	public IDictionary<Guid, IMetadataDevice> MetadataDevices => _metadataDevices;

	public IDictionary<Guid, IHardwareInfo> Hardware => _hardware;

	public ServerId ServerId => _serverId;

	public FQID FQID => new FQID(_serverId, Guid.Empty, _serverId.Id, FolderType.SystemDefined, Kind.Server);

	public MatrixMonitorCollection MatrixMonitors => new MatrixMonitorCollection(_systemInfo);

	private Uri GetRecorderCommandUri()
	{
		return new UriBuilder(Scheme, HostName, Port, "recordercommandservice/recordercommandservice.asmx").Uri;
	}

	public Recorder(ISystemInfo systemInfo, IRecorderInfo recorderInfo)
	{
		Recorder recorder = this;
		_systemInfo = systemInfo;
		_recorderInfo = recorderInfo;
		try
		{
			Uri uri = new Uri(_recorderInfo.WebServerUri);
			string host = uri.Host;
			int port = uri.Port;
			_name = _recorderInfo.Name;
			_guid = _recorderInfo.RecorderId;
			string type = "XPCORS";
			_serverId = new ServerId(type, uri.Scheme, host, port, _guid);
			_recorderApiClient = GenerateRecorderCommandService(GetRecorderCommandUri());
			_serverId.UserContext = systemInfo.UserContext;
			_cameras = new CameraCollection(systemInfo, this, recorderInfo);
			_microphones = new MicrophoneCollection(_systemInfo, this, recorderInfo);
			_speakers = new SpeakerCollection(_systemInfo, this, recorderInfo);
			_outputs = new OutputDeviceCollection(_systemInfo, this, recorderInfo);
			_userDefinedEvents = new UserDefinedEventCollection(_systemInfo, this, recorderInfo);
			if (recorderInfo.MetadataDevices != null)
			{
				_metadataDevices = recorderInfo.MetadataDevices.Select((IMetadataDeviceInfo info) => new MetadataDevice(systemInfo, info, recorder, recorderInfo)).ToDictionary((Func<MetadataDevice, Guid>)((MetadataDevice device) => device.Guid), (Func<MetadataDevice, IMetadataDevice>)((MetadataDevice device) => device));
			}
			if (recorderInfo.Hardware != null)
			{
				IHardwareInfo[] hardware = recorderInfo.Hardware;
				foreach (IHardwareInfo hardwareInfo in hardware)
				{
					_hardware[hardwareInfo.HardwareId] = hardwareInfo;
				}
			}
		}
		catch
		{
			_recorderApiClient = null;
		}
	}

	public void UpdateRecorder(IRecorderInfo recorderInfo)
	{
		try
		{
			lock (_lockObject)
			{
				_name = _recorderInfo.Name;
				CameraCollection cameraCollection = new CameraCollection(_systemInfo, this, recorderInfo);
				MicrophoneCollection microphoneCollection = new MicrophoneCollection(_systemInfo, this, recorderInfo);
				SpeakerCollection speakerCollection = new SpeakerCollection(_systemInfo, this, recorderInfo);
				OutputDeviceCollection outputDeviceCollection = new OutputDeviceCollection(_systemInfo, this, recorderInfo);
				UserDefinedEventCollection userDefinedEventCollection = new UserDefinedEventCollection(_systemInfo, this, recorderInfo);
				IDictionary<Guid, IMetadataDevice> dictionary = ((recorderInfo.MetadataDevices == null) ? new Dictionary<Guid, IMetadataDevice>() : recorderInfo.MetadataDevices.Select((IMetadataDeviceInfo info) => new MetadataDevice(_systemInfo, info, this, recorderInfo)).ToDictionary((Func<MetadataDevice, Guid>)((MetadataDevice device) => device.Guid), (Func<MetadataDevice, IMetadataDevice>)((MetadataDevice device) => device)));
				IDictionary<Guid, IHardwareInfo> dictionary2 = new Dictionary<Guid, IHardwareInfo>();
				if (recorderInfo.Hardware != null)
				{
					IHardwareInfo[] hardware = recorderInfo.Hardware;
					foreach (IHardwareInfo hardwareInfo in hardware)
					{
						dictionary2[hardwareInfo.HardwareId] = hardwareInfo;
					}
				}
				foreach (ICamera camera in _cameras)
				{
					if (!cameraCollection.ContainsKey(camera.Guid))
					{
						camera.Disabled = true;
						cameraCollection.Add(camera);
					}
				}
				_cameras = cameraCollection;
				foreach (IMicrophone microphone in _microphones)
				{
					if (!microphoneCollection.ContainsKey(microphone.Guid))
					{
						microphone.Disabled = true;
						microphoneCollection.Add(microphone);
					}
				}
				_microphones = microphoneCollection;
				foreach (OutputDevice output in _outputs)
				{
					if (!outputDeviceCollection.ContainsKey(output.Guid))
					{
						output.Disabled = true;
						outputDeviceCollection.AddOutput(output);
					}
				}
				_outputs = outputDeviceCollection;
				foreach (Speaker speaker in _speakers)
				{
					if (!speakerCollection.ContainsKey(speaker.Guid))
					{
						speaker.Disabled = true;
						speakerCollection.Add(speaker);
					}
				}
				_speakers = speakerCollection;
				foreach (UserDefinedEvent userDefinedEvent in _userDefinedEvents)
				{
					if (!userDefinedEventCollection.ContainsKey(userDefinedEvent.Guid))
					{
						userDefinedEvent.Disabled = true;
						userDefinedEventCollection.AddEvent(userDefinedEvent);
					}
				}
				_userDefinedEvents = userDefinedEventCollection;
				foreach (IMetadataDevice value in _metadataDevices.Values)
				{
					if (!dictionary.ContainsKey(value.Guid))
					{
						value.Disabled = true;
						dictionary.Add(value.Guid, value);
					}
				}
				_metadataDevices = dictionary;
				foreach (IHardwareInfo value2 in _hardware.Values)
				{
					if (!dictionary2.ContainsKey(value2.HardwareId))
					{
						value2.Disabled = true;
						dictionary2.Add(value2.HardwareId, value2);
					}
				}
				_hardware = dictionary2;
				_recorderInfo = recorderInfo;
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Recorder", "UpdateRecorder", exception);
		}
	}

	public void Cleanup()
	{
		DisposeRecorderCommandService(_recorderApiClient);
	}

	public override string ToString()
	{
		return Name;
	}

	private static RecorderCommandServiceSoapClient GenerateRecorderCommandService(Uri url)
	{
		RecorderCommandServiceSoapClient recorderCommandServiceSoapClient = new RecorderCommandServiceSoapClient(ManagementServerWcfHelper.GetNonOAuthBinding(url.Scheme == Uri.UriSchemeHttps), ManagementServerWcfHelper.GetEndpointAddress(url));
		ManagementServerWcfHelper.SetupCertificateValidation(recorderCommandServiceSoapClient);
		return recorderCommandServiceSoapClient;
	}

	private static void DisposeRecorderCommandService(RecorderCommandServiceSoapClient client)
	{
		if (client != null)
		{
			client = (RecorderCommandServiceSoapClient)ManagementServerWcfHelper.DisposeSoapClient(client);
		}
	}

	public List<object> RemoveHardware(Guid hardwareId)
	{
		List<object> list = new List<object>();
		lock (_lockObject)
		{
			if (_hardware.TryGetValue(hardwareId, out var value))
			{
				List<ICamera> list2 = new List<ICamera>();
				foreach (ICamera camera in _cameras)
				{
					if (value.DeviceIds.Contains(camera.Guid))
					{
						list.Add(camera);
						list2.Add(camera);
					}
				}
				foreach (ICamera item in list2)
				{
					_cameras.Remove(item);
				}
				List<IMicrophone> list3 = new List<IMicrophone>();
				foreach (IMicrophone microphone in _microphones)
				{
					if (value.DeviceIds.Contains(microphone.Guid))
					{
						list.Add(microphone);
						list3.Add(microphone);
					}
				}
				foreach (IMicrophone item2 in list3)
				{
					_microphones.Remove(item2);
				}
				List<Speaker> list4 = new List<Speaker>();
				foreach (Speaker speaker in _speakers)
				{
					if (value.DeviceIds.Contains(speaker.Guid))
					{
						list.Add(speaker);
						list4.Add(speaker);
					}
				}
				foreach (Speaker item3 in list4)
				{
					_speakers.Remove(item3);
				}
				List<OutputDevice> list5 = new List<OutputDevice>();
				foreach (OutputDevice output in _outputs)
				{
					if (value.DeviceIds.Contains(output.Guid))
					{
						list.Add(output);
						list5.Add(output);
					}
				}
				foreach (OutputDevice item4 in list5)
				{
					_outputs.Remove(item4);
				}
				List<UserDefinedEvent> list6 = new List<UserDefinedEvent>();
				foreach (UserDefinedEvent userDefinedEvent in _userDefinedEvents)
				{
					if (value.DeviceIds.Contains(userDefinedEvent.Guid))
					{
						list.Add(userDefinedEvent);
						list6.Add(userDefinedEvent);
					}
				}
				foreach (UserDefinedEvent item5 in list6)
				{
					_userDefinedEvents.Remove(item5);
				}
				List<IMetadataDevice> list7 = new List<IMetadataDevice>();
				foreach (IMetadataDevice value2 in _metadataDevices.Values)
				{
					if (value.DeviceIds.Contains(value2.Guid))
					{
						list.Add(value2);
						list7.Add(value2);
					}
				}
				foreach (IMetadataDevice item6 in list7)
				{
					_metadataDevices.Remove(item6.Guid);
				}
				list.Add(value);
				_hardware.Remove(hardwareId);
			}
		}
		return list;
	}

	public void AddHardware(List<object> objects)
	{
		lock (_lockObject)
		{
			foreach (object @object in objects)
			{
				ICamera camera = @object as ICamera;
				IMicrophone microphone = @object as IMicrophone;
				Speaker speaker = @object as Speaker;
				OutputDevice outputDevice = @object as OutputDevice;
				UserDefinedEvent userDefinedEvent = @object as UserDefinedEvent;
				IMetadataDevice metadataDevice = @object as IMetadataDevice;
				IHardwareInfo hardwareInfo = @object as IHardwareInfo;
				if (camera != null)
				{
					camera.Recorder = this;
					_cameras.Add(camera);
				}
				else if (microphone != null)
				{
					microphone.Recorder = this;
					_microphones.Add(microphone);
				}
				else if (speaker != null)
				{
					speaker.Recorder = this;
					_speakers.Add(speaker);
				}
				else if (outputDevice != null)
				{
					outputDevice.Recorder = this;
					_outputs.AddOutput(outputDevice);
				}
				else if (userDefinedEvent != null)
				{
					userDefinedEvent.Recorder = this;
					_userDefinedEvents.AddEvent(userDefinedEvent);
				}
				else if (metadataDevice != null)
				{
					metadataDevice.Recorder = this;
					_metadataDevices.Add(metadataDevice.Guid, metadataDevice);
				}
				else if (hardwareInfo != null)
				{
					hardwareInfo.RecorderId = Guid;
					_hardware.Add(hardwareInfo.HardwareId, hardwareInfo);
				}
				else
				{
					EnvironmentManager.Instance.Log(error: true, "Recorder", "Unhandled device type for hardware move:" + @object.GetType());
				}
			}
		}
	}

	public SequenceType[] GetSequenceTypes(Guid deviceGuid)
	{
		try
		{
			return _recorderApiClient.SequencesGetTypes(_systemInfo.Token, deviceGuid);
		}
		catch (Exception e)
		{
			HandleRecorderException("SequencesGetTypes", e, deviceGuid);
		}
		return null;
	}

	public SequenceCollection GetSequenceAtTimeMinutes(Guid deviceGuid, DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		TimeDuration timeDuration = new TimeDuration();
		timeDuration.MicroSeconds = minutesBefore.Ticks / 10;
		TimeDuration timeDuration2 = new TimeDuration();
		timeDuration2.MicroSeconds = minutesAfter.Ticks / 10;
		try
		{
			SequenceCollection sequenceCollection = new SequenceCollection();
			sequenceCollection.CreateSequenceCollection(GetSequences(deviceGuid, time, timeDuration, maxCountBefore, timeDuration2, maxCountAfter, seqType, CancellationToken.None));
			return sequenceCollection;
		}
		catch (Exception e)
		{
			HandleRecorderException("GetSequences", e, deviceGuid);
		}
		return null;
	}

	public TimeLineInformationCollection GetTimeLineInformation(Guid deviceGuid, Guid[] types, DateTime beginTime, int interval, int maxCount)
	{
		TimeSpan timeSpan = new TimeSpan(0, 0, interval);
		TimeDuration timeDuration = new TimeDuration();
		timeDuration.MicroSeconds = timeSpan.Ticks / 10;
		try
		{
			TimeLineInformationCollection timeLineInformationCollection = new TimeLineInformationCollection();
			timeLineInformationCollection.CreateTimeLineInformationCollection(_recorderApiClient.TimeLineInformationGet(_systemInfo.Token, deviceGuid, types, beginTime, timeDuration, maxCount));
			return timeLineInformationCollection;
		}
		catch (Exception e)
		{
			HandleRecorderException("TimeLineInformationGet", e, deviceGuid);
		}
		return null;
	}

	public void MoveToPreset(string deviceId, string presetName, bool throwOnError)
	{
		Guid guid = new Guid(deviceId);
		try
		{
			_recorderApiClient.PresetMoveTo(_systemInfo.Token, guid, presetName);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PresetMoveTo", ex, guid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PresetMoveTo' returns error: " + ex.Message);
			}
		}
	}

	public VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ConvertPTZArgsToService(PTZArgs ptzArgs)
	{
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs pTZArgs = new VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs();
		ArrayList arrayList = new ArrayList();
		foreach (PTZPairDouble item in ptzArgs.movement)
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble pTZPairDouble2 = new VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble();
			pTZPairDouble2.name = item.Name;
			pTZPairDouble2.value = item.Value;
			arrayList.Add(pTZPairDouble2);
		}
		pTZArgs.movement = (VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble[])arrayList.ToArray(typeof(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble));
		ArrayList arrayList2 = new ArrayList();
		foreach (PTZPairDouble item2 in ptzArgs.speed)
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble pTZPairDouble4 = new VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble();
			pTZPairDouble4.name = item2.Name;
			pTZPairDouble4.value = item2.Value;
			arrayList2.Add(pTZPairDouble4);
		}
		pTZArgs.speed = (VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble[])arrayList2.ToArray(typeof(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble));
		ArrayList arrayList3 = new ArrayList();
		foreach (PTZPairBoolean item3 in ptzArgs.automatic)
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairBoolean pTZPairBoolean2 = new VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairBoolean();
			pTZPairBoolean2.name = item3.Name;
			pTZPairBoolean2.value = item3.Value;
			arrayList3.Add(pTZPairBoolean2);
		}
		pTZArgs.automatic = (VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairBoolean[])arrayList3.ToArray(typeof(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairBoolean));
		return pTZArgs;
	}

	public PTZArgs ConvertPTZArgsFromService(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs)
	{
		PTZArgs pTZArgs = new PTZArgs();
		if (ptzArgs.movement != null)
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble[] movement = ptzArgs.movement;
			foreach (VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble pTZPairDouble in movement)
			{
				PTZPairDouble pTZPairDouble2 = new PTZPairDouble();
				pTZPairDouble2.Name = pTZPairDouble.name;
				pTZPairDouble2.Value = pTZPairDouble.value;
				pTZArgs.movement.Add(pTZPairDouble2);
			}
		}
		if (ptzArgs.speed != null)
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble[] movement = ptzArgs.speed;
			foreach (VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairDouble pTZPairDouble3 in movement)
			{
				PTZPairDouble pTZPairDouble4 = new PTZPairDouble();
				pTZPairDouble4.Name = pTZPairDouble3.name;
				pTZPairDouble4.Value = pTZPairDouble3.value;
				pTZArgs.speed.Add(pTZPairDouble4);
			}
		}
		if (ptzArgs.automatic != null)
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairBoolean[] automatic = ptzArgs.automatic;
			foreach (VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZPairBoolean pTZPairBoolean in automatic)
			{
				PTZPairBoolean pTZPairBoolean2 = new PTZPairBoolean();
				pTZPairBoolean2.Name = pTZPairBoolean.name;
				pTZPairBoolean2.Value = pTZPairBoolean.value;
				pTZArgs.automatic.Add(pTZPairBoolean2);
			}
		}
		return pTZArgs;
	}

	public void CameraPtzMoveAction(ICamera device, PtzMoveRelativeTypes ptzMoveType)
	{
		Guid guid = device.Guid;
		if (ptzMoveType == PtzMoveRelativeTypes.Home)
		{
			try
			{
				_recorderApiClient.PTZMoveHome(_systemInfo.Token, guid);
				return;
			}
			catch (Exception e)
			{
				HandleRecorderException("PTZMoveHome", e, guid);
				return;
			}
		}
		PTZArgs pTZArgs = new PTZArgs();
		switch (ptzMoveType)
		{
		case PtzMoveRelativeTypes.Up:
			pTZArgs.Tilt = -1.0;
			pTZArgs.TiltSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.UpRight:
			pTZArgs.Tilt = -1.0;
			pTZArgs.TiltSpeed = 1.0;
			pTZArgs.Pan = 1.0;
			pTZArgs.PanSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.Right:
			pTZArgs.Pan = 1.0;
			pTZArgs.PanSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.DownRight:
			pTZArgs.Tilt = 1.0;
			pTZArgs.TiltSpeed = 1.0;
			pTZArgs.Pan = 1.0;
			pTZArgs.PanSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.Down:
			pTZArgs.Tilt = 1.0;
			pTZArgs.TiltSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.DownLeft:
			pTZArgs.Tilt = 1.0;
			pTZArgs.TiltSpeed = 1.0;
			pTZArgs.Pan = -1.0;
			pTZArgs.PanSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.Left:
			pTZArgs.Pan = -1.0;
			pTZArgs.PanSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.UpLeft:
			pTZArgs.Tilt = -1.0;
			pTZArgs.TiltSpeed = 1.0;
			pTZArgs.Pan = -1.0;
			pTZArgs.PanSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.ZoomIn:
			pTZArgs.Zoom = 1.0;
			pTZArgs.ZoomSpeed = 1.0;
			break;
		case PtzMoveRelativeTypes.ZoomOut:
			pTZArgs.Zoom = -1.0;
			pTZArgs.ZoomSpeed = 1.0;
			break;
		}
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs = ConvertPTZArgsToService(pTZArgs);
		try
		{
			_recorderApiClient.PTZMoveRelative(_systemInfo.Token, guid, ptzArgs);
		}
		catch (Exception e2)
		{
			HandleRecorderException("PTZMoveRelative", e2, guid);
		}
	}

	public void CameraPtzMoveAction(ICamera device, PtzMoveRelativeTypes ptzMoveType, double speed, double step)
	{
		Guid guid = device.Guid;
		if (ptzMoveType == PtzMoveRelativeTypes.Home)
		{
			try
			{
				_recorderApiClient.PTZMoveHome(_systemInfo.Token, guid);
				return;
			}
			catch (Exception e)
			{
				HandleRecorderException("PTZMoveHome", e, guid);
				return;
			}
		}
		PTZArgs pTZArgs = new PTZArgs();
		switch (ptzMoveType)
		{
		case PtzMoveRelativeTypes.Up:
			pTZArgs.Tilt = 0.0 - step;
			pTZArgs.TiltSpeed = speed;
			break;
		case PtzMoveRelativeTypes.UpRight:
			pTZArgs.Tilt = 0.0 - step;
			pTZArgs.TiltSpeed = speed;
			pTZArgs.Pan = step;
			pTZArgs.PanSpeed = speed;
			break;
		case PtzMoveRelativeTypes.Right:
			pTZArgs.Pan = step;
			pTZArgs.PanSpeed = speed;
			break;
		case PtzMoveRelativeTypes.DownRight:
			pTZArgs.Tilt = step;
			pTZArgs.TiltSpeed = speed;
			pTZArgs.Pan = step;
			pTZArgs.PanSpeed = speed;
			break;
		case PtzMoveRelativeTypes.Down:
			pTZArgs.Tilt = step;
			pTZArgs.TiltSpeed = speed;
			break;
		case PtzMoveRelativeTypes.DownLeft:
			pTZArgs.Tilt = step;
			pTZArgs.TiltSpeed = speed;
			pTZArgs.Pan = 0.0 - step;
			pTZArgs.PanSpeed = speed;
			break;
		case PtzMoveRelativeTypes.Left:
			pTZArgs.Pan = 0.0 - step;
			pTZArgs.PanSpeed = speed;
			break;
		case PtzMoveRelativeTypes.UpLeft:
			pTZArgs.Tilt = 0.0 - step;
			pTZArgs.TiltSpeed = speed;
			pTZArgs.Pan = 0.0 - step;
			pTZArgs.PanSpeed = speed;
			break;
		case PtzMoveRelativeTypes.ZoomIn:
			pTZArgs.Zoom = step;
			pTZArgs.ZoomSpeed = speed;
			break;
		case PtzMoveRelativeTypes.ZoomOut:
			pTZArgs.Zoom = 0.0 - step;
			pTZArgs.ZoomSpeed = speed;
			break;
		}
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs = ConvertPTZArgsToService(pTZArgs);
		try
		{
			_recorderApiClient.PTZMoveRelative(_systemInfo.Token, guid, ptzArgs);
		}
		catch (Exception e2)
		{
			HandleRecorderException("PTZMoveRelative", e2, guid);
		}
	}

	public void CameraPtzMoveActionAbsolute(ICamera camera, double pan, double tilt, double zoom)
	{
		PTZArgs pTZArgs = new PTZArgs();
		pTZArgs.Pan = pan;
		pTZArgs.PanSpeed = 1.0;
		pTZArgs.Tilt = tilt;
		pTZArgs.TiltSpeed = 1.0;
		pTZArgs.Zoom = zoom;
		pTZArgs.ZoomSpeed = 1.0;
		Guid guid = camera.Guid;
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs = ConvertPTZArgsToService(pTZArgs);
		try
		{
			_recorderApiClient.PTZMoveAbsolute(_systemInfo.Token, guid, ptzArgs);
		}
		catch (Exception e)
		{
			HandleRecorderException("PTZMoveAbsolute", e, guid);
		}
	}

	public void CameraPtzMoveActionAbsolute(ICamera camera, PTZArgs ptzArgs, bool throwOnError)
	{
		Guid guid = camera.Guid;
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs2 = ConvertPTZArgsToService(ptzArgs);
		try
		{
			_recorderApiClient.PTZMoveAbsolute(_systemInfo.Token, guid, ptzArgs2);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZMoveAbsolute", ex, guid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PTZMoveAbsolute' returns error: " + ex.Message);
			}
		}
	}

	public PTZArgs PtzGetAbsolutePosition(ICamera camera, bool throwOnError)
	{
		Guid guid = camera.Guid;
		try
		{
			VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs = _recorderApiClient.PTZGetAbsolutePosition(_systemInfo.Token, guid);
			return ConvertPTZArgsFromService(ptzArgs);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZGetAbsolutePosition", ex, guid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PtzGetAbsolutePosition' returns error: " + ex.Message);
			}
		}
		return null;
	}

	public void PtzMoveHome(ICamera camera)
	{
		Guid guid = camera.Guid;
		try
		{
			_recorderApiClient.PTZMoveHome(_systemInfo.Token, guid);
		}
		catch (Exception e)
		{
			HandleRecorderException("PTZMoveHome", e, guid);
		}
	}

	public void CameraPtzMoveStart(ICamera camera, PTZArgs ptzArgs, bool throwOnError)
	{
		Guid guid = camera.Guid;
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs2 = ConvertPTZArgsToService(ptzArgs);
		try
		{
			_recorderApiClient.PTZMoveStart(_systemInfo.Token, guid, ptzArgs2);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZMoveStart", ex, guid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PTZMoveStart' returns error: " + ex.Message);
			}
		}
	}

	public void CameraPtzMoveStart(Guid cameraGuid, PTZArgs ptzargs, bool throwOnError)
	{
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs = ConvertPTZArgsToService(ptzargs);
		try
		{
			_recorderApiClient.PTZMoveStart(_systemInfo.Token, cameraGuid, ptzArgs);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZMoveStart", ex, cameraGuid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PTZMoveStart' returns error: " + ex.Message);
			}
		}
	}

	public void CameraPtzMoveRelative(Guid cameraGuid, PTZArgs ptzargs)
	{
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PTZArgs ptzArgs = ConvertPTZArgsToService(ptzargs);
		try
		{
			_recorderApiClient.PTZMoveRelative(_systemInfo.Token, cameraGuid, ptzArgs);
		}
		catch (Exception e)
		{
			HandleRecorderException("PTZMoveRelative", e, cameraGuid);
		}
	}

	public void CameraDriverCommand(Guid cameraGuid, string command, bool throwOnError)
	{
		try
		{
			_recorderApiClient.SendDriverCommandAsync(_systemInfo.Token, cameraGuid, command);
		}
		catch (Exception ex)
		{
			HandleRecorderException("SendDriverCommandAsync", ex, cameraGuid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PtzGetAbsolutePosition' returns error: " + ex.Message);
			}
		}
	}

	public async Task<string> AsyncDriverCommandWithResponse(Guid cameraGuid, string command, string parameter)
	{
		try
		{
			return await _recorderApiClient.SendDriverCommandWithResponseAsync(_systemInfo.Token, cameraGuid, command, parameter);
		}
		catch (Exception ex)
		{
			HandleRecorderException("SendDriverCommandWithResponseAsync", ex, cameraGuid);
			if (ex.Message != null && ex.Message.StartsWith("ERROR:"))
			{
				return ex.Message;
			}
			return $"ERROR: {ex.HResult}: {ex.Message}";
		}
	}

	public string DriverCommandWithResponse(Guid cameraGuid, string command, string parameter, bool throwOnError)
	{
		try
		{
			return _recorderApiClient.SendDriverCommandWithResponse(_systemInfo.Token, cameraGuid, command, parameter);
		}
		catch (Exception ex)
		{
			HandleRecorderException("DriverCommandWithResponse", ex, cameraGuid);
			string text = ((ex.Message != null && ex.Message.StartsWith("ERROR:", StringComparison.InvariantCulture)) ? ex.Message : $"ERROR: {ex.HResult}: {ex.Message}");
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'SendDriverCommandWithResponse' returns error: " + text, ex);
			}
			return text;
		}
	}

	public void CameraPtzAuxOn(ICamera camera, int auxNumber, bool throwOnError)
	{
		Guid guid = camera.Guid;
		try
		{
			_recorderApiClient.SendDriverCommandAsync(_systemInfo.Token, guid, "PTZ_AUX_" + auxNumber + "_ON");
		}
		catch (Exception ex)
		{
			HandleRecorderException("CameraPtzAuxOn", ex, guid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'CameraPtzAuxOn' returns error: " + ex.Message);
			}
		}
	}

	public void CameraPtzAuxOff(ICamera camera, int auxNumber, bool throwOnError)
	{
		Guid guid = camera.Guid;
		try
		{
			_recorderApiClient.SendDriverCommandAsync(_systemInfo.Token, guid, "PTZ_AUX_" + auxNumber + "_OFF");
		}
		catch (Exception ex)
		{
			HandleRecorderException("CameraPtzAuxOff", ex, guid);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'CameraPtzAuxOff' returns error: " + ex.Message);
			}
		}
	}

	public void CameraPtzMoveStop(ICamera camera)
	{
		Guid guid = camera.Guid;
		try
		{
			_recorderApiClient.PTZMoveStop(_systemInfo.Token, guid);
		}
		catch (Exception e)
		{
			HandleRecorderException("PTZMoveStop", e, guid);
		}
	}

	public void CameraPtzCenter(ICamera camera, double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		Guid guid = camera.Guid;
		try
		{
			_recorderApiClient.PTZCenter(_systemInfo.Token, guid, refWidth, refHeight, centerX, centerY, zoom);
		}
		catch (Exception e)
		{
			HandleRecorderException("PTZCenter", e, guid);
		}
	}

	public void CameraPtzRectangle(ICamera camera, double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
		Guid guid = camera.Guid;
		try
		{
			_recorderApiClient.PTZRectangle(_systemInfo.Token, guid, refWidth, refHeight, left, top, bottom, right);
		}
		catch (Exception e)
		{
			HandleRecorderException("PTZRectangle", e, guid);
		}
	}

	public void PTZActivatePatrollingSchema(Guid deviceId, Guid patrollingSchemaId, bool throwOnError)
	{
		try
		{
			_recorderApiClient.PTZActivatePatrollingSchema(_systemInfo.Token, deviceId, patrollingSchemaId);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZActivatePatrollingSchema", ex, deviceId);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PTZActivatePatrollingSchema' returns error: " + ex.Message);
			}
		}
	}

	public void PTZDeactivatePatrollingSchema(Guid deviceId, Guid patrollingSchemaId, bool throwOnError)
	{
		try
		{
			_recorderApiClient.PTZDeactivatePatrollingSchema(_systemInfo.Token, deviceId, patrollingSchemaId);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZDeactivatePatrollingSchema", ex, deviceId);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'PTZDeactivatePatrollingSchema' returns error: " + ex.Message);
			}
		}
	}

	public void OutputActivate(string deviceId, bool throwOnError)
	{
		Guid guid = new Guid(deviceId);
		try
		{
			_recorderApiClient.OutputActivate(_systemInfo.Token, guid);
		}
		catch (Exception ex)
		{
			HandleRecorderException("OutputActivate", ex, guid, Kind.Output);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'OutputActivate' returns error: " + ex.Message);
			}
		}
	}

	public void OutputDeactive(string deviceId, bool throwOnError)
	{
		Guid guid = new Guid(deviceId);
		try
		{
			_recorderApiClient.OutputDeactivate(_systemInfo.Token, guid);
		}
		catch (Exception ex)
		{
			HandleRecorderException("OutputDeactivate", ex, guid, Kind.Output);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'OutputDeactive' returns error: " + ex.Message);
			}
		}
	}

	public IOState OutputGetState(string deviceId, bool throwOnError)
	{
		Guid guid = new Guid(deviceId);
		try
		{
			return _recorderApiClient.OutputGetState(_systemInfo.Token, guid) switch
			{
				VideoOS.Platform.SDK.Proxy.RecorderServices.Net.IOState.Activated => IOState.Activated, 
				VideoOS.Platform.SDK.Proxy.RecorderServices.Net.IOState.Deactivated => IOState.Deactivated, 
				_ => IOState.Unknown, 
			};
		}
		catch (Exception ex)
		{
			HandleRecorderException("OutputGetState", ex, guid, Kind.Output);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'OutputGetState' returns error: " + ex.Message);
			}
		}
		return IOState.Unknown;
	}

	public void OuputTrigger(string deviceId, int duration, bool throwOnError)
	{
		Guid guid = new Guid(deviceId);
		TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, duration);
		TimeDuration timeDuration = new TimeDuration();
		timeDuration.MicroSeconds = timeSpan.Ticks / 10;
		try
		{
			_recorderApiClient.OutputTrigger(_systemInfo.Token, guid, timeDuration);
		}
		catch (Exception ex)
		{
			HandleRecorderException("OutputTrigger", ex, guid, Kind.Output);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'OuputTrigger' returns error: " + ex.Message);
			}
		}
	}

	public async Task AsyncMulticastContinue(Guid[] sessionIds)
	{
		try
		{
			await _recorderApiClient.MulticastContinueAsync(_systemInfo.Token, sessionIds);
		}
		catch (Exception ex)
		{
			HandleAsyncException("AsyncMulticastContinue", ex);
		}
	}

	public async Task<MulticastInfo[]> AsyncMulticastStart(List<Guid> sessionIds, ClientCapabilities clientCapabilities)
	{
		try
		{
			return await _recorderApiClient.MulticastStartAsync(_systemInfo.Token, sessionIds.ToArray(), clientCapabilities);
		}
		catch (Exception ex)
		{
			HandleAsyncException("AsyncMulticastStart", ex);
		}
		return null;
	}

	public async Task AsyncMulticastStop(List<Guid> sessionIds)
	{
		try
		{
			await _recorderApiClient.MulticastStopAsync(_systemInfo.Token, sessionIds.ToArray());
		}
		catch (Exception ex)
		{
			HandleAsyncException("AsyncMulticastStop", ex);
		}
	}

	public Tuple<DateTime, string> GetLiveJpeg(Guid cameraId, int width, int height)
	{
		try
		{
			JPEGData jPEGData = _recorderApiClient.JPEGGetLive(_systemInfo.Token, cameraId, width, height);
			return new Tuple<DateTime, string>(jPEGData.Time, Convert.ToBase64String(jPEGData.Data));
		}
		catch (Exception ex)
		{
			HandleRecorderException("GetLiveJpeg", ex, cameraId);
			throw new MIPException(ex.Message, ex);
		}
	}

	public Tuple<DateTime, string> GetJpeg(Guid cameraId, DateTime atTimeStamp)
	{
		try
		{
			JPEGData jPEGData = _recorderApiClient.JPEGGetAt(_systemInfo.Token, cameraId, atTimeStamp);
			return new Tuple<DateTime, string>(jPEGData.Time, Convert.ToBase64String(jPEGData.Data));
		}
		catch (Exception ex)
		{
			HandleRecorderException("GetJpeg", ex, cameraId);
			throw new MIPException(ex.Message, ex);
		}
	}

	public void ReservePTZ(Guid cameraId, bool reserve, bool throwOnError)
	{
		try
		{
			if (reserve)
			{
				_recorderApiClient.PTZReserveSession(_systemInfo.Token, cameraId);
			}
			else
			{
				_recorderApiClient.PTZReleaseSession(_systemInfo.Token, cameraId);
			}
		}
		catch (Exception ex)
		{
			string text = (reserve ? "ReservePTZ" : "ReleasePTZ");
			HandleRecorderException(text, ex, cameraId);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method '" + text + "' returns error: " + ex.Message);
			}
		}
	}

	public void ExtendPTZ(Guid cameraId, bool throwOnError)
	{
		try
		{
			_recorderApiClient.PTZExtendedManualSession(_systemInfo.Token, cameraId);
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZReserve", ex, cameraId);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'ExtendPTZ' returns error: " + ex.Message);
			}
		}
	}

	public PtzSessionInfo GetPTZSessionInfo(Guid cameraId, bool throwOnError)
	{
		try
		{
			return new PtzSessionInfo(_recorderApiClient.PTZGetSessionInfo(_systemInfo.Token, cameraId));
		}
		catch (Exception ex)
		{
			HandleRecorderException("PTZGetSessionInfo", ex, cameraId);
			if (throwOnError)
			{
				throw new CommunicationMIPException("Method 'GetPTZSessionInfo' returns error: " + ex.Message);
			}
		}
		return null;
	}

	private static void ValidateAsyncResponse(AsyncCompletedEventArgs asyncCompletedEventArgs)
	{
		if (asyncCompletedEventArgs.Error != null)
		{
			throw asyncCompletedEventArgs.Error;
		}
	}

	private static void HandleAsyncException(string caller, Exception ex)
	{
		EnvironmentManager.Instance.Log(error: true, "RecorderCommandService", ex.Message);
	}

	private SequenceEntry[] GetSequences(Guid deviceId, DateTime time, TimeDuration timeBefore, int maxCountBefore, TimeDuration minutesAfter, int maxCountAfter, Guid seqType, CancellationToken cancellationToken)
	{
		bool flag = false;
		do
		{
			try
			{
				return _recorderApiClient.SequencesGetAroundWithSpan(_systemInfo.Token, deviceId, seqType, time, timeBefore, maxCountBefore, minutesAfter, maxCountAfter);
			}
			catch (Exception ex)
			{
				MIPException ex2 = ExceptionHandler.HandleException(ex);
				if (flag)
				{
					throw ex2;
				}
				TimeSpan timeSpan = TimeSpan.Zero;
				if (ex.InnerException is WebException { Response: HttpWebResponse { StatusCode: HttpStatusCode.ServiceUnavailable, Headers: not null } response })
				{
					string s = response.Headers[HttpResponseHeader.RetryAfter];
					int result = 0;
					if (int.TryParse(s, out result))
					{
						int num = 30;
						timeSpan = TimeSpan.FromSeconds((result < num) ? result : num);
					}
				}
				if (timeSpan > TimeSpan.Zero && WaitHandle.WaitAny(new WaitHandle[1] { cancellationToken.WaitHandle }, timeSpan, exitContext: false) == 258)
				{
					flag = true;
				}
				if (!flag)
				{
					throw ex2;
				}
			}
		}
		while (flag);
		return null;
	}

	private void HandleRecorderException(string command, Exception e, Guid deviceGuid)
	{
		HandleRecorderException(command, e, deviceGuid, Kind.Camera);
	}

	private void HandleRecorderException(string command, Exception e, Guid deviceGuid, Guid kind)
	{
		Trace.WriteLine(e.GetType()?.ToString() + ": " + e.Message + "\n" + e.StackTrace);
		MIPException ex = ExceptionHandler.HandleException(e);
		CommunicationMIPException ex2 = ex as CommunicationMIPException;
		int recorderWebInterfaceErrorCode = 0;
		int ptzErrorDetail = 0;
		if (ex2 != null)
		{
			recorderWebInterfaceErrorCode = ex2.ErrorCode;
			ptzErrorDetail = ex2.SubErrorNumber;
		}
		FQID sender = new FQID(_serverId, _guid, deviceGuid, FolderType.No, kind);
		EnvironmentManager.Instance.SendMessage(new Message("Control.RecorderErrorIndication", new RecorderErrorIndicationData
		{
			Command = command,
			UrlString = GetRecorderCommandUri().ToString(),
			ErrorText = ex.Message,
			RecorderWebInterfaceErrorCode = recorderWebInterfaceErrorCode,
			PtzErrorDetail = ptzErrorDetail
		}), null, sender);
	}
}

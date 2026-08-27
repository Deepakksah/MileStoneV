using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class RecorderInfo : IRecorderInfo
{
	private Guid recorderIdField;

	private string nameField;

	private string descriptionField;

	private string hostNameField;

	private string webServerUriField;

	private string timeZoneNameField;

	private int defaultRecordingTimeSecondsField;

	private string xmlEncodingField;

	private DateTime lastModifiedField;

	private CameraInfo[] camerasField;

	private InputInfo[] inputsField;

	private OutputInfo[] outputsField;

	private MicrophoneInfo[] microphonesField;

	private SpeakerInfo[] speakersField;

	private MetadataDeviceInfo[] metadataDevicesField;

	private HardwareInfo[] hardwareField;

	private Guid serviceIdField;

	ICameraInfo[] IRecorderInfo.Cameras
	{
		get
		{
			return Cameras;
		}
		set
		{
			Cameras = (CameraInfo[])value;
		}
	}

	IInputInfo[] IRecorderInfo.Inputs
	{
		get
		{
			return Inputs;
		}
		set
		{
			Inputs = (InputInfo[])value;
		}
	}

	IOutputInfo[] IRecorderInfo.Outputs
	{
		get
		{
			return Outputs;
		}
		set
		{
			Outputs = (OutputInfo[])value;
		}
	}

	IMicrophoneInfo[] IRecorderInfo.Microphones
	{
		get
		{
			return Microphones;
		}
		set
		{
			Microphones = (MicrophoneInfo[])value;
		}
	}

	ISpeakerInfo[] IRecorderInfo.Speakers
	{
		get
		{
			return Speakers;
		}
		set
		{
			Speakers = (SpeakerInfo[])value;
		}
	}

	IMetadataDeviceInfo[] IRecorderInfo.MetadataDevices
	{
		get
		{
			return MetadataDevices;
		}
		set
		{
			MetadataDevices = (MetadataDeviceInfo[])value;
		}
	}

	IHardwareInfo[] IRecorderInfo.Hardware
	{
		get
		{
			return Hardware;
		}
		set
		{
			Hardware = (HardwareInfo[])value;
		}
	}

	public Guid RecorderId
	{
		get
		{
			return recorderIdField;
		}
		set
		{
			recorderIdField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public string HostName
	{
		get
		{
			return hostNameField;
		}
		set
		{
			hostNameField = value;
		}
	}

	public string WebServerUri
	{
		get
		{
			return webServerUriField;
		}
		set
		{
			webServerUriField = value;
		}
	}

	public string TimeZoneName
	{
		get
		{
			return timeZoneNameField;
		}
		set
		{
			timeZoneNameField = value;
		}
	}

	public int DefaultRecordingTimeSeconds
	{
		get
		{
			return defaultRecordingTimeSecondsField;
		}
		set
		{
			defaultRecordingTimeSecondsField = value;
		}
	}

	public string XmlEncoding
	{
		get
		{
			return xmlEncodingField;
		}
		set
		{
			xmlEncodingField = value;
		}
	}

	public DateTime LastModified
	{
		get
		{
			return lastModifiedField;
		}
		set
		{
			lastModifiedField = value;
		}
	}

	public CameraInfo[] Cameras
	{
		get
		{
			return camerasField;
		}
		set
		{
			camerasField = value;
		}
	}

	public InputInfo[] Inputs
	{
		get
		{
			return inputsField;
		}
		set
		{
			inputsField = value;
		}
	}

	public OutputInfo[] Outputs
	{
		get
		{
			return outputsField;
		}
		set
		{
			outputsField = value;
		}
	}

	public MicrophoneInfo[] Microphones
	{
		get
		{
			return microphonesField;
		}
		set
		{
			microphonesField = value;
		}
	}

	public SpeakerInfo[] Speakers
	{
		get
		{
			return speakersField;
		}
		set
		{
			speakersField = value;
		}
	}

	public MetadataDeviceInfo[] MetadataDevices
	{
		get
		{
			return metadataDevicesField;
		}
		set
		{
			metadataDevicesField = value;
		}
	}

	public HardwareInfo[] Hardware
	{
		get
		{
			return hardwareField;
		}
		set
		{
			hardwareField = value;
		}
	}

	public Guid ServiceId
	{
		get
		{
			return serviceIdField;
		}
		set
		{
			serviceIdField = value;
		}
	}

	public void RemoveCamera(Guid deviceId)
	{
		CameraInfo[] cameras = Cameras;
		Cameras = new CameraInfo[cameras.Length - 1];
		int i = 0;
		int num = 0;
		for (; i < cameras.Length; i++)
		{
			if (num >= cameras.Length - 1)
			{
				break;
			}
			if (!cameras[i].DeviceId.Equals(deviceId))
			{
				Cameras[num] = cameras[i];
				num++;
			}
		}
	}

	public void RemoveMicrophone(Guid deviceId)
	{
		MicrophoneInfo[] microphones = Microphones;
		Microphones = new MicrophoneInfo[microphones.Length - 1];
		int i = 0;
		int num = 0;
		for (; i < microphones.Length; i++)
		{
			if (num >= microphones.Length - 1)
			{
				break;
			}
			if (!microphones[i].DeviceId.Equals(deviceId))
			{
				Microphones[num] = microphones[i];
				num++;
			}
		}
	}

	public void RemoveSpeaker(Guid deviceId)
	{
		SpeakerInfo[] speakers = Speakers;
		Speakers = new SpeakerInfo[speakers.Length - 1];
		int i = 0;
		int num = 0;
		for (; i < speakers.Length; i++)
		{
			if (num >= speakers.Length - 1)
			{
				break;
			}
			if (!speakers[i].DeviceId.Equals(deviceId))
			{
				Speakers[num] = speakers[i];
				num++;
			}
		}
	}
}

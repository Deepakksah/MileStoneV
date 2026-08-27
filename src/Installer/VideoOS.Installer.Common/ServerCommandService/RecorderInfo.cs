using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RecorderInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RecorderInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private CameraInfo[] CamerasField;

	private int DefaultRecordingTimeSecondsField;

	private string DescriptionField;

	private HardwareInfo[] HardwareField;

	private string HostNameField;

	private InputInfo[] InputsField;

	private DateTime LastModifiedField;

	private MetadataDeviceInfo[] MetadataDevicesField;

	private MicrophoneInfo[] MicrophonesField;

	private string NameField;

	private OutputInfo[] OutputsField;

	private Guid RecorderIdField;

	private Guid ServiceIdField;

	private SpeakerInfo[] SpeakersField;

	private string TimeZoneNameField;

	private string WebServerUriField;

	private string XmlEncodingField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public CameraInfo[] Cameras
	{
		get
		{
			return CamerasField;
		}
		set
		{
			CamerasField = value;
		}
	}

	[DataMember]
	public int DefaultRecordingTimeSeconds
	{
		get
		{
			return DefaultRecordingTimeSecondsField;
		}
		set
		{
			DefaultRecordingTimeSecondsField = value;
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public HardwareInfo[] Hardware
	{
		get
		{
			return HardwareField;
		}
		set
		{
			HardwareField = value;
		}
	}

	[DataMember]
	public string HostName
	{
		get
		{
			return HostNameField;
		}
		set
		{
			HostNameField = value;
		}
	}

	[DataMember]
	public InputInfo[] Inputs
	{
		get
		{
			return InputsField;
		}
		set
		{
			InputsField = value;
		}
	}

	[DataMember]
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			LastModifiedField = value;
		}
	}

	[DataMember]
	public MetadataDeviceInfo[] MetadataDevices
	{
		get
		{
			return MetadataDevicesField;
		}
		set
		{
			MetadataDevicesField = value;
		}
	}

	[DataMember]
	public MicrophoneInfo[] Microphones
	{
		get
		{
			return MicrophonesField;
		}
		set
		{
			MicrophonesField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public OutputInfo[] Outputs
	{
		get
		{
			return OutputsField;
		}
		set
		{
			OutputsField = value;
		}
	}

	[DataMember]
	public Guid RecorderId
	{
		get
		{
			return RecorderIdField;
		}
		set
		{
			RecorderIdField = value;
		}
	}

	[DataMember]
	public Guid ServiceId
	{
		get
		{
			return ServiceIdField;
		}
		set
		{
			ServiceIdField = value;
		}
	}

	[DataMember]
	public SpeakerInfo[] Speakers
	{
		get
		{
			return SpeakersField;
		}
		set
		{
			SpeakersField = value;
		}
	}

	[DataMember]
	public string TimeZoneName
	{
		get
		{
			return TimeZoneNameField;
		}
		set
		{
			TimeZoneNameField = value;
		}
	}

	[DataMember]
	public string WebServerUri
	{
		get
		{
			return WebServerUriField;
		}
		set
		{
			WebServerUriField = value;
		}
	}

	[DataMember]
	public string XmlEncoding
	{
		get
		{
			return XmlEncodingField;
		}
		set
		{
			XmlEncodingField = value;
		}
	}
}

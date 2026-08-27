using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Status", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class Status : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private CameraDeviceStatus[] CameraDeviceStatusArrayField;

	private InputDeviceStatus[] InputDeviceStatusArrayField;

	private OutputDeviceStatus[] OutputDeviceStatusArrayField;

	private MicrophoneDeviceStatus[] MicrophoneDeviceStatusArrayField;

	private SpeakerDeviceStatus[] SpeakerDeviceStatusArrayField;

	private MetadataDeviceStatus[] MetadataDeviceStatusArrayField;

	private EventStatus[] EventStatusArrayField;

	private HardwareStatus[] HardwareStatusArrayField;

	private ConfigurationChangedStatus ConfigurationChangedStatusField;

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

	[DataMember(EmitDefaultValue = false)]
	public CameraDeviceStatus[] CameraDeviceStatusArray
	{
		get
		{
			return CameraDeviceStatusArrayField;
		}
		set
		{
			CameraDeviceStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public InputDeviceStatus[] InputDeviceStatusArray
	{
		get
		{
			return InputDeviceStatusArrayField;
		}
		set
		{
			InputDeviceStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public OutputDeviceStatus[] OutputDeviceStatusArray
	{
		get
		{
			return OutputDeviceStatusArrayField;
		}
		set
		{
			OutputDeviceStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 3)]
	public MicrophoneDeviceStatus[] MicrophoneDeviceStatusArray
	{
		get
		{
			return MicrophoneDeviceStatusArrayField;
		}
		set
		{
			MicrophoneDeviceStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public SpeakerDeviceStatus[] SpeakerDeviceStatusArray
	{
		get
		{
			return SpeakerDeviceStatusArrayField;
		}
		set
		{
			SpeakerDeviceStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 5)]
	public MetadataDeviceStatus[] MetadataDeviceStatusArray
	{
		get
		{
			return MetadataDeviceStatusArrayField;
		}
		set
		{
			MetadataDeviceStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 6)]
	public EventStatus[] EventStatusArray
	{
		get
		{
			return EventStatusArrayField;
		}
		set
		{
			EventStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 7)]
	public HardwareStatus[] HardwareStatusArray
	{
		get
		{
			return HardwareStatusArrayField;
		}
		set
		{
			HardwareStatusArrayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 8)]
	public ConfigurationChangedStatus ConfigurationChangedStatus
	{
		get
		{
			return ConfigurationChangedStatusField;
		}
		set
		{
			ConfigurationChangedStatusField = value;
		}
	}
}

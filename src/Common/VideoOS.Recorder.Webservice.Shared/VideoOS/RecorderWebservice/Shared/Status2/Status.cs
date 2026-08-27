using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class Status
{
	private CameraDeviceStatus[] cameraDeviceStatusArrayField;

	private InputDeviceStatus[] inputDeviceStatusArrayField;

	private OutputDeviceStatus[] outputDeviceStatusArrayField;

	private MicrophoneDeviceStatus[] microphoneDeviceStatusArrayField;

	private SpeakerDeviceStatus[] speakerDeviceStatusArrayField;

	private MetadataDeviceStatus[] metadataDeviceStatusArrayField;

	private EventStatus[] eventStatusArrayField;

	private HardwareStatus[] hardwareStatusArrayField;

	private ConfigurationChangedStatus configurationChangedStatusField;

	public CameraDeviceStatus[] CameraDeviceStatusArray
	{
		get
		{
			return cameraDeviceStatusArrayField;
		}
		set
		{
			cameraDeviceStatusArrayField = value;
		}
	}

	public InputDeviceStatus[] InputDeviceStatusArray
	{
		get
		{
			return inputDeviceStatusArrayField;
		}
		set
		{
			inputDeviceStatusArrayField = value;
		}
	}

	public OutputDeviceStatus[] OutputDeviceStatusArray
	{
		get
		{
			return outputDeviceStatusArrayField;
		}
		set
		{
			outputDeviceStatusArrayField = value;
		}
	}

	public MicrophoneDeviceStatus[] MicrophoneDeviceStatusArray
	{
		get
		{
			return microphoneDeviceStatusArrayField;
		}
		set
		{
			microphoneDeviceStatusArrayField = value;
		}
	}

	public SpeakerDeviceStatus[] SpeakerDeviceStatusArray
	{
		get
		{
			return speakerDeviceStatusArrayField;
		}
		set
		{
			speakerDeviceStatusArrayField = value;
		}
	}

	public MetadataDeviceStatus[] MetadataDeviceStatusArray
	{
		get
		{
			return metadataDeviceStatusArrayField;
		}
		set
		{
			metadataDeviceStatusArrayField = value;
		}
	}

	public EventStatus[] EventStatusArray
	{
		get
		{
			return eventStatusArrayField;
		}
		set
		{
			eventStatusArrayField = value;
		}
	}

	public HardwareStatus[] HardwareStatusArray
	{
		get
		{
			return hardwareStatusArrayField;
		}
		set
		{
			hardwareStatusArrayField = value;
		}
	}

	public ConfigurationChangedStatus ConfigurationChangedStatus
	{
		get
		{
			return configurationChangedStatusField;
		}
		set
		{
			configurationChangedStatusField = value;
		}
	}
}

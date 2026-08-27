using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status;

[Serializable]
[XmlInclude(typeof(DeviceStatusBase))]
[XmlInclude(typeof(InputDeviceStatus))]
[XmlInclude(typeof(MicrophoneDeviceStatus))]
[XmlInclude(typeof(CameraDeviceStatus))]
[XmlInclude(typeof(OutputDeviceStatus))]
[XmlInclude(typeof(SpeakerDeviceStatus))]
[XmlInclude(typeof(ConfigurationChangedStatus))]
[XmlInclude(typeof(EventStatus))]
[GeneratedCode("wsdl", "2.0.50727.1432")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus")]
public class StatusBase
{
	private DateTime timeField;

	private long sequenceNoField;

	public DateTime Time
	{
		get
		{
			return timeField;
		}
		set
		{
			timeField = value;
		}
	}

	public long SequenceNo
	{
		get
		{
			return sequenceNoField;
		}
		set
		{
			sequenceNoField = value;
		}
	}
}

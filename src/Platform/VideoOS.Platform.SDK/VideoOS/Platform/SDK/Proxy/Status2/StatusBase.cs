using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[XmlInclude(typeof(EventStatus))]
[XmlInclude(typeof(HardwareStatus))]
[XmlInclude(typeof(ConfigurationChangedStatus))]
[XmlInclude(typeof(DeviceStatusBase))]
[XmlInclude(typeof(IODeviceStatusBase))]
[XmlInclude(typeof(InputDeviceStatus))]
[XmlInclude(typeof(OutputDeviceStatus))]
[XmlInclude(typeof(MediaStreamDeviceStatusBase))]
[XmlInclude(typeof(MicrophoneDeviceStatus))]
[XmlInclude(typeof(SpeakerDeviceStatus))]
[XmlInclude(typeof(MetadataDeviceStatus))]
[XmlInclude(typeof(CameraDeviceStatus))]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class StatusBase
{
	private DateTime timeField;

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
}

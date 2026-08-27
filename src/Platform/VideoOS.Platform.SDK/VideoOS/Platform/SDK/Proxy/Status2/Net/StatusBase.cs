using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StatusBase", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
[KnownType(typeof(ConfigurationChangedStatus))]
[KnownType(typeof(HardwareStatus))]
[KnownType(typeof(EventStatus))]
[KnownType(typeof(DeviceStatusBase))]
[KnownType(typeof(IODeviceStatusBase))]
[KnownType(typeof(OutputDeviceStatus))]
[KnownType(typeof(InputDeviceStatus))]
[KnownType(typeof(MediaStreamDeviceStatusBase))]
[KnownType(typeof(MetadataDeviceStatus))]
[KnownType(typeof(SpeakerDeviceStatus))]
[KnownType(typeof(MicrophoneDeviceStatus))]
[KnownType(typeof(CameraDeviceStatus))]
public class StatusBase : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DateTime TimeField;

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

	[DataMember(IsRequired = true)]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			TimeField = value;
		}
	}
}

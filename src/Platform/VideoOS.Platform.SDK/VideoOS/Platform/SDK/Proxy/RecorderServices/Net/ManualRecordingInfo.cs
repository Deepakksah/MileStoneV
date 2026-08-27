using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class ManualRecordingInfo
{
	private Guid deviceIdField;

	private bool isManualRecordingField;

	[XmlElement(Order = 0)]
	public Guid DeviceId
	{
		get
		{
			return deviceIdField;
		}
		set
		{
			deviceIdField = value;
		}
	}

	[XmlElement(Order = 1)]
	public bool IsManualRecording
	{
		get
		{
			return isManualRecordingField;
		}
		set
		{
			isManualRecordingField = value;
		}
	}
}

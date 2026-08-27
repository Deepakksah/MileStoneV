using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class ManualRecordingInfo
{
	private Guid deviceIdField;

	private bool isManualRecordingField;

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

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus")]
public class CameraDeviceStatus : DeviceStatusBase
{
	private bool recordingField;

	private bool motionField;

	public bool Recording
	{
		get
		{
			return recordingField;
		}
		set
		{
			recordingField = value;
		}
	}

	public bool Motion
	{
		get
		{
			return motionField;
		}
		set
		{
			motionField = value;
		}
	}
}

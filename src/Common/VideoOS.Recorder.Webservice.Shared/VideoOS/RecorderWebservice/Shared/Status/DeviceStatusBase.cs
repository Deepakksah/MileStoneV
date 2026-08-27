using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status;

[Serializable]
[XmlInclude(typeof(InputDeviceStatus))]
[XmlInclude(typeof(MicrophoneDeviceStatus))]
[XmlInclude(typeof(CameraDeviceStatus))]
[XmlInclude(typeof(OutputDeviceStatus))]
[XmlInclude(typeof(SpeakerDeviceStatus))]
[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus")]
public class DeviceStatusBase : StatusBase
{
	private Guid deviceIdField;

	private bool isChangeField;

	private bool startedField;

	private bool enabledField;

	private bool errorField;

	private bool licensedField;

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

	public bool IsChange
	{
		get
		{
			return isChangeField;
		}
		set
		{
			isChangeField = value;
		}
	}

	public bool Started
	{
		get
		{
			return startedField;
		}
		set
		{
			startedField = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return enabledField;
		}
		set
		{
			enabledField = value;
		}
	}

	public bool Error
	{
		get
		{
			return errorField;
		}
		set
		{
			errorField = value;
		}
	}

	public bool Licensed
	{
		get
		{
			return licensedField;
		}
		set
		{
			licensedField = value;
		}
	}
}

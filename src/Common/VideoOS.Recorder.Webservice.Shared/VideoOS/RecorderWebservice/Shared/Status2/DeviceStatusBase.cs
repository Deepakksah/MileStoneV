using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status2;

[Serializable]
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
public class DeviceStatusBase : StatusBase
{
	private Guid deviceIdField;

	private bool isChangeField;

	private bool enabledField;

	private bool startedField;

	private bool errorField;

	private bool errorNotLicensedField;

	private bool errorNoConnectionField;

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

	public bool ErrorNotLicensed
	{
		get
		{
			return errorNotLicensedField;
		}
		set
		{
			errorNotLicensedField = value;
		}
	}

	public bool ErrorNoConnection
	{
		get
		{
			return errorNoConnectionField;
		}
		set
		{
			errorNoConnectionField = value;
		}
	}
}

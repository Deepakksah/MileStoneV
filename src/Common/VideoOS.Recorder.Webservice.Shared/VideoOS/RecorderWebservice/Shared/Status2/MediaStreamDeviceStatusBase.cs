using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status2;

[Serializable]
[XmlInclude(typeof(MicrophoneDeviceStatus))]
[XmlInclude(typeof(SpeakerDeviceStatus))]
[XmlInclude(typeof(MetadataDeviceStatus))]
[XmlInclude(typeof(CameraDeviceStatus))]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class MediaStreamDeviceStatusBase : DeviceStatusBase
{
	private bool recordingField;

	private bool dbMoveInProgressField;

	private bool errorOverflowField;

	private bool errorWritingGopField;

	private bool dbRepairInProgressField;

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

	public bool DbMoveInProgress
	{
		get
		{
			return dbMoveInProgressField;
		}
		set
		{
			dbMoveInProgressField = value;
		}
	}

	public bool ErrorOverflow
	{
		get
		{
			return errorOverflowField;
		}
		set
		{
			errorOverflowField = value;
		}
	}

	public bool ErrorWritingGop
	{
		get
		{
			return errorWritingGopField;
		}
		set
		{
			errorWritingGopField = value;
		}
	}

	public bool DbRepairInProgress
	{
		get
		{
			return dbRepairInProgressField;
		}
		set
		{
			dbRepairInProgressField = value;
		}
	}
}

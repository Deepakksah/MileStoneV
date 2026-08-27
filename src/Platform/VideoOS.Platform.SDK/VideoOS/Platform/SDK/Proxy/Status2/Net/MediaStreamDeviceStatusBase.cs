using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MediaStreamDeviceStatusBase", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
[KnownType(typeof(MetadataDeviceStatus))]
[KnownType(typeof(SpeakerDeviceStatus))]
[KnownType(typeof(MicrophoneDeviceStatus))]
[KnownType(typeof(CameraDeviceStatus))]
public class MediaStreamDeviceStatusBase : DeviceStatusBase
{
	private bool RecordingField;

	private bool DbMoveInProgressField;

	private bool ErrorOverflowField;

	private bool ErrorWritingGopField;

	private bool DbRepairInProgressField;

	[DataMember(IsRequired = true)]
	public bool Recording
	{
		get
		{
			return RecordingField;
		}
		set
		{
			RecordingField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 1)]
	public bool DbMoveInProgress
	{
		get
		{
			return DbMoveInProgressField;
		}
		set
		{
			DbMoveInProgressField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 2)]
	public bool ErrorOverflow
	{
		get
		{
			return ErrorOverflowField;
		}
		set
		{
			ErrorOverflowField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public bool ErrorWritingGop
	{
		get
		{
			return ErrorWritingGopField;
		}
		set
		{
			ErrorWritingGopField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 4)]
	public bool DbRepairInProgress
	{
		get
		{
			return DbRepairInProgressField;
		}
		set
		{
			DbRepairInProgressField = value;
		}
	}
}

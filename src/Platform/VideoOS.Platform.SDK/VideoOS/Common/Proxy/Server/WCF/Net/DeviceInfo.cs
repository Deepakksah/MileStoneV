using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "DeviceInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[KnownType(typeof(InputInfo))]
[KnownType(typeof(MetadataDeviceInfo))]
[KnownType(typeof(MicrophoneInfo))]
[KnownType(typeof(OutputInfo))]
[KnownType(typeof(SpeakerInfo))]
[KnownType(typeof(CameraInfo))]
public class DeviceInfo : IDeviceInfo
{
	private double CoverageDepthField;

	private double CoverageDirectionField;

	private double CoverageFieldOfViewField;

	private string DescriptionField;

	private Guid DeviceIdField;

	private int DeviceIndexField;

	private string GisPointField;

	private Guid HardwareIdField;

	private int IconField;

	private string NameField;

	private Guid RecorderIdField;

	private string ShortNameField;

	private string ShortcutField;

	[DataMember]
	public double CoverageDepth
	{
		get
		{
			return CoverageDepthField;
		}
		set
		{
			CoverageDepthField = value;
		}
	}

	[DataMember]
	public double CoverageDirection
	{
		get
		{
			return CoverageDirectionField;
		}
		set
		{
			CoverageDirectionField = value;
		}
	}

	[DataMember]
	public double CoverageFieldOfView
	{
		get
		{
			return CoverageFieldOfViewField;
		}
		set
		{
			CoverageFieldOfViewField = value;
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			DeviceIdField = value;
		}
	}

	[DataMember]
	public int DeviceIndex
	{
		get
		{
			return DeviceIndexField;
		}
		set
		{
			DeviceIndexField = value;
		}
	}

	[DataMember]
	public string GisPoint
	{
		get
		{
			return GisPointField;
		}
		set
		{
			GisPointField = value;
		}
	}

	[DataMember]
	public Guid HardwareId
	{
		get
		{
			return HardwareIdField;
		}
		set
		{
			HardwareIdField = value;
		}
	}

	[DataMember]
	public int Icon
	{
		get
		{
			return IconField;
		}
		set
		{
			IconField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public Guid RecorderId
	{
		get
		{
			return RecorderIdField;
		}
		set
		{
			RecorderIdField = value;
		}
	}

	[DataMember]
	public string ShortName
	{
		get
		{
			return ShortNameField;
		}
		set
		{
			ShortNameField = value;
		}
	}

	[DataMember]
	public string Shortcut
	{
		get
		{
			return ShortcutField;
		}
		set
		{
			ShortcutField = value;
		}
	}
}

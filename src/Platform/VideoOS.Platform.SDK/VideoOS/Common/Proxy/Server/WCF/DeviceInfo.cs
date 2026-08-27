using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DeviceInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[KnownType(typeof(InputInfo))]
[KnownType(typeof(MetadataDeviceInfo))]
[KnownType(typeof(MicrophoneInfo))]
[KnownType(typeof(OutputInfo))]
[KnownType(typeof(SpeakerInfo))]
[KnownType(typeof(CameraInfo))]
[ExcludeFromCodeCoverage]
public class DeviceInfo : IExtensibleDataObject, IDeviceInfo
{
	private ExtensionDataObject extensionDataField;

	private string DescriptionField;

	private Guid DeviceIdField;

	private int DeviceIndexField;

	private Guid HardwareIdField;

	private string NameField;

	private Guid RecorderIdField;

	private string ShortcutField;

	private string gisPointField;

	private string shortNameField;

	private int iconField;

	private double coverageDirectionField;

	private double coverageDepthField;

	private double coverageFieldOfViewField;

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

	[DataMember]
	public string GisPoint
	{
		get
		{
			return gisPointField;
		}
		set
		{
			gisPointField = value;
		}
	}

	[DataMember]
	public string ShortName
	{
		get
		{
			return shortNameField;
		}
		set
		{
			shortNameField = value;
		}
	}

	[DataMember]
	public int Icon
	{
		get
		{
			return iconField;
		}
		set
		{
			iconField = value;
		}
	}

	[DataMember]
	public double CoverageDirection
	{
		get
		{
			return coverageDirectionField;
		}
		set
		{
			coverageDirectionField = value;
		}
	}

	[DataMember]
	public double CoverageDepth
	{
		get
		{
			return coverageDepthField;
		}
		set
		{
			coverageDepthField = value;
		}
	}

	[DataMember]
	public double CoverageFieldOfView
	{
		get
		{
			return coverageFieldOfViewField;
		}
		set
		{
			coverageFieldOfViewField = value;
		}
	}
}

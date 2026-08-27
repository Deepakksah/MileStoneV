using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[XmlInclude(typeof(InputInfo))]
[XmlInclude(typeof(OutputInfo))]
[XmlInclude(typeof(MicrophoneInfo))]
[XmlInclude(typeof(SpeakerInfo))]
[XmlInclude(typeof(CameraInfo))]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public abstract class DeviceInfo : IDeviceInfo
{
	private Guid recorderIdField;

	private Guid hardwareIdField;

	private Guid deviceIdField;

	private string nameField;

	private string descriptionField;

	private string shortcutField;

	private int deviceIndexField;

	private string gisPointField;

	private string shortNameField;

	private int iconField;

	private double coverageDirectionField;

	private double coverageDepthField;

	private double coverageFieldOfViewField;

	public Guid RecorderId
	{
		get
		{
			return recorderIdField;
		}
		set
		{
			recorderIdField = value;
		}
	}

	public Guid HardwareId
	{
		get
		{
			return hardwareIdField;
		}
		set
		{
			hardwareIdField = value;
		}
	}

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

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public string Shortcut
	{
		get
		{
			return shortcutField;
		}
		set
		{
			shortcutField = value;
		}
	}

	public int DeviceIndex
	{
		get
		{
			return deviceIndexField;
		}
		set
		{
			deviceIndexField = value;
		}
	}

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

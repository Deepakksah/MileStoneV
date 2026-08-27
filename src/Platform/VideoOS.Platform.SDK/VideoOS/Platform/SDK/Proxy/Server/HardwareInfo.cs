using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class HardwareInfo : IHardwareInfo
{
	private Guid hardwareIdField;

	private Guid[] deviceIdsField;

	private Guid recorderIdField;

	private string nameField;

	private string descriptionField;

	private string uriField;

	private bool interconnectedField;

	public bool Disabled { get; set; }

	public DateTime LastModified { get; set; }

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

	public Guid[] DeviceIds
	{
		get
		{
			return deviceIdsField;
		}
		set
		{
			deviceIdsField = value;
		}
	}

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

	public string Uri
	{
		get
		{
			return uriField;
		}
		set
		{
			uriField = value;
		}
	}

	public bool Interconnected
	{
		get
		{
			return interconnectedField;
		}
		set
		{
			interconnectedField = value;
		}
	}
}

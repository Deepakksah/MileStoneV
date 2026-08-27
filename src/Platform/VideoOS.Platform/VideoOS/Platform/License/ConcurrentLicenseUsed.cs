using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.License;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "ConcurrentLicenseUsed", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.License")]
public class ConcurrentLicenseUsed : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string CustomDataField;

	private string IPAddressField;

	private string LicenseTypeField;

	private string MachineNameField;

	private Guid PluginIdField;

	private string WindowsUserField;

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
	public string CustomData
	{
		get
		{
			return CustomDataField;
		}
		set
		{
			CustomDataField = value;
		}
	}

	[DataMember]
	public string IPAddress
	{
		get
		{
			return IPAddressField;
		}
		set
		{
			IPAddressField = value;
		}
	}

	[DataMember]
	public string LicenseType
	{
		get
		{
			return LicenseTypeField;
		}
		set
		{
			LicenseTypeField = value;
		}
	}

	[DataMember]
	public string MachineName
	{
		get
		{
			return MachineNameField;
		}
		set
		{
			MachineNameField = value;
		}
	}

	[DataMember]
	public Guid PluginId
	{
		get
		{
			return PluginIdField;
		}
		set
		{
			PluginIdField = value;
		}
	}

	[DataMember]
	public string WindowsUser
	{
		get
		{
			return WindowsUserField;
		}
		set
		{
			WindowsUserField = value;
		}
	}
}

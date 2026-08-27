using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServerInformation", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class ServerInformation : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ComputerNameField;

	private string DescriptionField;

	private string IdField;

	private string InstanceIdField;

	private DateTime LastModifiedField;

	private string MasterSiteField;

	private string NameField;

	private int PhysicalMemoryField;

	private string PlatformField;

	private int ProcessorsField;

	private string ServiceAccountField;

	private Version VersionField;

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
	public string ComputerName
	{
		get
		{
			return ComputerNameField;
		}
		set
		{
			ComputerNameField = value;
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
	public string Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public string InstanceId
	{
		get
		{
			return InstanceIdField;
		}
		set
		{
			InstanceIdField = value;
		}
	}

	[DataMember]
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			LastModifiedField = value;
		}
	}

	[DataMember]
	public string MasterSite
	{
		get
		{
			return MasterSiteField;
		}
		set
		{
			MasterSiteField = value;
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
	public int PhysicalMemory
	{
		get
		{
			return PhysicalMemoryField;
		}
		set
		{
			PhysicalMemoryField = value;
		}
	}

	[DataMember]
	public string Platform
	{
		get
		{
			return PlatformField;
		}
		set
		{
			PlatformField = value;
		}
	}

	[DataMember]
	public int Processors
	{
		get
		{
			return ProcessorsField;
		}
		set
		{
			ProcessorsField = value;
		}
	}

	[DataMember]
	public string ServiceAccount
	{
		get
		{
			return ServiceAccountField;
		}
		set
		{
			ServiceAccountField = value;
		}
	}

	[DataMember]
	public Version Version
	{
		get
		{
			return VersionField;
		}
		set
		{
			VersionField = value;
		}
	}
}

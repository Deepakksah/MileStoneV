using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServerGroupInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class ServerGroupInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DateTime CreatedDateField;

	private string DescriptionField;

	private string IdField;

	private DateTime LastModifiedField;

	private string NameField;

	private string ParentIdField;

	private string PathField;

	private ServerGroupOptions ServerGroupOptionsField;

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
	public DateTime CreatedDate
	{
		get
		{
			return CreatedDateField;
		}
		set
		{
			CreatedDateField = value;
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
	public string ParentId
	{
		get
		{
			return ParentIdField;
		}
		set
		{
			ParentIdField = value;
		}
	}

	[DataMember]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}

	[DataMember]
	public ServerGroupOptions ServerGroupOptions
	{
		get
		{
			return ServerGroupOptionsField;
		}
		set
		{
			ServerGroupOptionsField = value;
		}
	}
}

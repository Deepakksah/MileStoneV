using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "BasicUserInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class BasicUserInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool CanChangePasswordField;

	private DateTime CreatedDateField;

	private string DescriptionField;

	private string IdField;

	private DateTime LastModifiedField;

	private string NameField;

	private string SidField;

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
	public bool CanChangePassword
	{
		get
		{
			return CanChangePasswordField;
		}
		set
		{
			CanChangePasswordField = value;
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
	public string Sid
	{
		get
		{
			return SidField;
		}
		set
		{
			SidField = value;
		}
	}
}

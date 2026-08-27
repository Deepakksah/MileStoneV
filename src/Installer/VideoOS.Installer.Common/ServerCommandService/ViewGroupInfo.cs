using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ViewGroupInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IdField;

	private DateTime LastModifiedTimeField;

	private string LastModifiedUserField;

	private string NameField;

	private bool ReadOnlyField;

	private ViewGroupType ViewGroupTypeField;

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
	public Guid Id
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
	public DateTime LastModifiedTime
	{
		get
		{
			return LastModifiedTimeField;
		}
		set
		{
			LastModifiedTimeField = value;
		}
	}

	[DataMember]
	public string LastModifiedUser
	{
		get
		{
			return LastModifiedUserField;
		}
		set
		{
			LastModifiedUserField = value;
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
	public bool ReadOnly
	{
		get
		{
			return ReadOnlyField;
		}
		set
		{
			ReadOnlyField = value;
		}
	}

	[DataMember]
	public ViewGroupType ViewGroupType
	{
		get
		{
			return ViewGroupTypeField;
		}
		set
		{
			ViewGroupTypeField = value;
		}
	}
}

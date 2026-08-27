using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Security;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Identity", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class Identity : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AccountNameField;

	private bool DeletedField;

	private string DescriptionField;

	private string DisplayNameField;

	private string DistinguishedNameField;

	private string DomainField;

	private string[] MemberOfField;

	private string[] MembersField;

	private string SidField;

	private IdentityType TypeField;

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
	public string AccountName
	{
		get
		{
			return AccountNameField;
		}
		set
		{
			AccountNameField = value;
		}
	}

	[DataMember]
	public bool Deleted
	{
		get
		{
			return DeletedField;
		}
		set
		{
			DeletedField = value;
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
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public string DistinguishedName
	{
		get
		{
			return DistinguishedNameField;
		}
		set
		{
			DistinguishedNameField = value;
		}
	}

	[DataMember]
	public string Domain
	{
		get
		{
			return DomainField;
		}
		set
		{
			DomainField = value;
		}
	}

	[DataMember]
	public string[] MemberOf
	{
		get
		{
			return MemberOfField;
		}
		set
		{
			MemberOfField = value;
		}
	}

	[DataMember]
	public string[] Members
	{
		get
		{
			return MembersField;
		}
		set
		{
			MembersField = value;
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

	[DataMember]
	public IdentityType Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			TypeField = value;
		}
	}
}

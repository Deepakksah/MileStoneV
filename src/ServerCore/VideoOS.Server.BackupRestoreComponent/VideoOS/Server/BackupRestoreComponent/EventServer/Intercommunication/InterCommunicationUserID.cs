using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InterCommunicationUserID", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class InterCommunicationUserID : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool DeletedField;

	private string DescriptionField;

	private string DisplayNameField;

	private string DistinguishedNameField;

	private string LocationField;

	private string[] MemberOfField;

	private string[] MembersField;

	private string NameField;

	private string SIDField;

	private Guid TypeField;

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
	public string Location
	{
		get
		{
			return LocationField;
		}
		set
		{
			LocationField = value;
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
	public string SID
	{
		get
		{
			return SIDField;
		}
		set
		{
			SIDField = value;
		}
	}

	[DataMember]
	public Guid Type
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

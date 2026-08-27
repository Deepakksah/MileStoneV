using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SimpleVisualElement", Namespace = "http://videoos.net/2/CentralServerMapCommand")]
public class SimpleVisualElement : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private TypeID IdField;

	private string NameField;

	private TypeID RelationIdField;

	private DateTime TimestampField;

	private TypeID VisualOwnerIdField;

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
	public TypeID Id
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
	public TypeID RelationId
	{
		get
		{
			return RelationIdField;
		}
		set
		{
			RelationIdField = value;
		}
	}

	[DataMember]
	public DateTime Timestamp
	{
		get
		{
			return TimestampField;
		}
		set
		{
			TimestampField = value;
		}
	}

	[DataMember]
	public TypeID VisualOwnerId
	{
		get
		{
			return VisualOwnerIdField;
		}
		set
		{
			VisualOwnerIdField = value;
		}
	}
}

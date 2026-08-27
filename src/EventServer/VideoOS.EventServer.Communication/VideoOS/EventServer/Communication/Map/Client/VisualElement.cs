using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "VisualElement", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class VisualElement : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Bounds BoundsField;

	private string DisplayXMLField;

	private TypeID IDField;

	private string NameField;

	private TypeID RelationIDField;

	private DateTime TimestampField;

	private TypeID VisualOwnerIDField;

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
	public Bounds Bounds
	{
		get
		{
			return BoundsField;
		}
		set
		{
			BoundsField = value;
		}
	}

	[DataMember]
	public string DisplayXML
	{
		get
		{
			return DisplayXMLField;
		}
		set
		{
			DisplayXMLField = value;
		}
	}

	[DataMember]
	public TypeID ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
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
	public TypeID RelationID
	{
		get
		{
			return RelationIDField;
		}
		set
		{
			RelationIDField = value;
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
	public TypeID VisualOwnerID
	{
		get
		{
			return VisualOwnerIDField;
		}
		set
		{
			VisualOwnerIDField = value;
		}
	}
}

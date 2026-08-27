using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Rule", Namespace = "urn:milestone-systems")]
public class Rule : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IDField;

	private string NameField;

	private string TypeField;

	private PolygonList PolygonListField;

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

	[DataMember(EmitDefaultValue = false)]
	public Guid ID
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

	[DataMember(EmitDefaultValue = false)]
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

	[DataMember(EmitDefaultValue = false)]
	public string Type
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

	[DataMember(EmitDefaultValue = false, Order = 3)]
	public PolygonList PolygonList
	{
		get
		{
			return PolygonListField;
		}
		set
		{
			PolygonListField = value;
		}
	}
}

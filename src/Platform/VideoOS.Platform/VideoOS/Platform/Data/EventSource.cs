using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventSource", Namespace = "urn:milestone-systems")]
public class EventSource : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string NameField;

	private string DescriptionField;

	private FQID FQIDField;

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

	[DataMember(EmitDefaultValue = false, Order = 1)]
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

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public FQID FQID
	{
		get
		{
			return FQIDField;
		}
		set
		{
			FQIDField = value;
		}
	}
}

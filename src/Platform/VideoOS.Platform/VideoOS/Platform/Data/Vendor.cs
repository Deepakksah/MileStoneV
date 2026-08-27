using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Vendor", Namespace = "urn:milestone-systems")]
public class Vendor : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string NameField;

	private string CustomDataField;

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
	public string CustomData
	{
		get
		{
			return CustomDataField;
		}
		set
		{
			CustomDataField = value;
		}
	}
}

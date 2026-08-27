using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Reference", Namespace = "urn:milestone-systems")]
public class Reference : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

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

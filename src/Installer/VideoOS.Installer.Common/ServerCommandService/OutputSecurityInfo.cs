using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OutputSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputSecurityInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool ActivateField;

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
	public bool Activate
	{
		get
		{
			return ActivateField;
		}
		set
		{
			ActivateField = value;
		}
	}
}

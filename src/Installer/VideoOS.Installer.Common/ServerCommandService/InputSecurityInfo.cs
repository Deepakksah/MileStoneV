using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class InputSecurityInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool ReadInputField;

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
	public bool ReadInput
	{
		get
		{
			return ReadInputField;
		}
		set
		{
			ReadInputField = value;
		}
	}
}

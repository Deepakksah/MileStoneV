using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class InputInfo : DeviceInfo
{
	private InputSecurityInfo InputSecurityField;

	[DataMember]
	public InputSecurityInfo InputSecurity
	{
		get
		{
			return InputSecurityField;
		}
		set
		{
			InputSecurityField = value;
		}
	}
}

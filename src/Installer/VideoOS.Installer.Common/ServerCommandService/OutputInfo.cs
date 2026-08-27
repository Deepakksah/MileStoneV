using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OutputInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputInfo : DeviceInfo
{
	private OutputSecurityInfo OutputSecurityField;

	[DataMember]
	public OutputSecurityInfo OutputSecurity
	{
		get
		{
			return OutputSecurityField;
		}
		set
		{
			OutputSecurityField = value;
		}
	}
}

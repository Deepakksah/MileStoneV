using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TimeDuration", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class TimeDuration : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private long MicroSecondsField;

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
	public long MicroSeconds
	{
		get
		{
			return MicroSecondsField;
		}
		set
		{
			MicroSecondsField = value;
		}
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "NameNumberPairValue", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class NameNumberPairValue : IExtensibleDataObject
{
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public int Number { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}

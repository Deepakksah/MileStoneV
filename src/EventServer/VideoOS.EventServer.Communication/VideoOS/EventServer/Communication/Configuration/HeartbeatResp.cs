using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HeartbeatResp", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class HeartbeatResp : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

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
}

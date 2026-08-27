using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HeartbeatParams", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class HeartbeatParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private HeartbeatData[] HeartbeatDataField;

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
	public HeartbeatData[] HeartbeatData
	{
		get
		{
			return HeartbeatDataField;
		}
		set
		{
			HeartbeatDataField = value;
		}
	}
}

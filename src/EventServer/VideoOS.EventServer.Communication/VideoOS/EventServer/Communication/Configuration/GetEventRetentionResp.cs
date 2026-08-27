using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetEventRetentionResp", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class GetEventRetentionResp : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private EventRetentions EventRetentionsField;

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
	public EventRetentions EventRetentions
	{
		get
		{
			return EventRetentionsField;
		}
		set
		{
			EventRetentionsField = value;
		}
	}
}

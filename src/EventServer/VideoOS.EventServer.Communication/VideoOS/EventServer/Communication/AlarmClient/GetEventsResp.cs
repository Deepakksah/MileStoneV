using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetEventsResp", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class GetEventsResp : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private BaseEvent[] EventsField;

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
	public BaseEvent[] Events
	{
		get
		{
			return EventsField;
		}
		set
		{
			EventsField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetEventsParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class GetEventsParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid[] EventIdsField;

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
	public Guid[] EventIds
	{
		get
		{
			return EventIdsField;
		}
		set
		{
			EventIdsField = value;
		}
	}
}

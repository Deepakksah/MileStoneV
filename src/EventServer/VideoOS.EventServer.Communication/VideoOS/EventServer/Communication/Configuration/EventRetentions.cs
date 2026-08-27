using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventRetentions", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class EventRetentions : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int DefaultRetentionField;

	private EventRetention[] EventGroupRetentionsField;

	private EventRetention[] EventTypeRetentionsField;

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
	public int DefaultRetention
	{
		get
		{
			return DefaultRetentionField;
		}
		set
		{
			DefaultRetentionField = value;
		}
	}

	[DataMember]
	public EventRetention[] EventGroupRetentions
	{
		get
		{
			return EventGroupRetentionsField;
		}
		set
		{
			EventGroupRetentionsField = value;
		}
	}

	[DataMember]
	public EventRetention[] EventTypeRetentions
	{
		get
		{
			return EventTypeRetentionsField;
		}
		set
		{
			EventTypeRetentionsField = value;
		}
	}
}

using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DataContract(Name = "EventData", Namespace = "urn:milestone-systems")]
public class EventData : BaseEvent
{
	[DataMember]
	public EventSequence EventSequence { get; set; }
}

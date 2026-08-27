using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DataContract(Name = "FreeFormBaseEvent", Namespace = "urn:milestone-systems")]
public sealed class FreeFormBaseEvent : BaseEvent
{
	[DataMember(Order = 1)]
	public string DataBlob { get; set; }
}

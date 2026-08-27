using System.Runtime.Serialization;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

[DataContract(Name = "BaseEvent", Namespace = "urn:milestone-systems")]
[KnownType(typeof(AnalyticsEvent))]
[KnownType(typeof(EventData))]
[KnownType(typeof(AccessControlEvent))]
[KnownType(typeof(FreeFormBaseEvent))]
public class BaseEvent : IExtensibleDataObject
{
	[DataMember(IsRequired = true, Order = 0)]
	public EventHeader EventHeader { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public BaseEvent()
	{
		Migration.Initialize();
	}
}

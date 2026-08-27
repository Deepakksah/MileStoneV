using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DataContract(Name = "AccessControlEvent", Namespace = "urn:milestone-systems")]
public class AccessControlEvent : BaseEvent
{
	[DataMember(Order = 1)]
	public string AccessControlSystemEventId { get; set; }

	[DataMember(Order = 2)]
	public Guid AccessControlSystemId { get; set; }

	[DataMember(Order = 3)]
	public Guid AccessControlEventTypeId { get; set; }

	[DataMember(Order = 4)]
	public Guid AccessControlEventSourceTypeId { get; set; }

	[DataMember(Order = 5)]
	public Guid AccessControlEventSourceId { get; set; }

	[DataMember(Order = 6)]
	public string[] RelatedAccessControlCredentialHolderIds { get; set; }

	[DataMember(Order = 7)]
	public Guid[] RelatedAccessControlElementIds { get; set; }

	[DataMember(Order = 8)]
	public Guid[] EventCategories { get; set; }

	[DataMember(Order = 9)]
	public FQID[] RelatedSurveillanceItems { get; set; }

	[DataMember(Order = 10)]
	public Property[] Properties { get; set; }

	[DataMember(Order = 11)]
	public string Reason { get; set; }
}

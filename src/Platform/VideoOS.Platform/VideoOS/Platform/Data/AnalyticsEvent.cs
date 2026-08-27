using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DataContract(Name = "AnalyticsEvent", Namespace = "urn:milestone-systems")]
public class AnalyticsEvent : BaseEvent
{
	[DataMember(Order = 1)]
	public string Description { get; set; }

	[DataMember(Order = 2)]
	public DateTime StartTime { get; set; }

	[DataMember(Order = 3)]
	public DateTime EndTime { get; set; }

	[DataMember(Order = 4)]
	public string Location { get; set; }

	[DataMember(Order = 5)]
	public uint Count { get; set; }

	[DataMember(Order = 6)]
	public RuleList RuleList { get; set; }

	[DataMember(Order = 7)]
	public AnalyticsObjectList ObjectList { get; set; }

	[DataMember(Order = 8)]
	public ReferenceList ReferenceList { get; set; }

	[DataMember(Order = 9)]
	public SnapshotList SnapshotList { get; set; }

	[DataMember(Order = 10)]
	public Vendor Vendor { get; set; }
}

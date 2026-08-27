using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public class VarianceDefinition
{
	[DataMember]
	public Guid RecurrenceId { get; set; }

	[DataMember]
	public DateTime OriginalStartDate { get; set; }

	[DataMember]
	public DateTime StartDate { get; set; }

	[DataMember]
	public DateTime EndDate { get; set; }

	[DataMember]
	public string Subject { get; set; }
}

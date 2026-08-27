using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public class AppointmentDefinition
{
	[DataMember]
	public bool AllDayEvent { get; set; }

	[DataMember]
	public string Subject { get; set; }

	[DataMember]
	public DateTime StartDateTime { get; set; }

	[DataMember]
	public DateTime EndDateTime { get; set; }

	[DataMember(EmitDefaultValue = true)]
	public AppointmentRecurrenceDefinition AppointmentRecurrence { get; set; }

	[DataMember(EmitDefaultValue = true)]
	public VarianceDefinition Variance { get; set; }

	public AppointmentDefinition()
	{
	}

	public AppointmentDefinition(DateTime start, DateTime end)
	{
		StartDateTime = start;
		EndDateTime = end;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} {1:yyyy-MM-dd HH:mm.ss} - {2:yyyy-MM-dd HH:mm.ss}", Subject ?? "", StartDateTime, EndDateTime);
	}
}

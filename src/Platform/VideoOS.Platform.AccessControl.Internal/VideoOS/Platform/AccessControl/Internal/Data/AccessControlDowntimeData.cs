using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract]
public class AccessControlDowntimeData
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public DateTime Start { get; set; }

	[DataMember]
	public DateTime? End { get; set; }

	public AccessControlDowntimeData()
	{
	}

	public AccessControlDowntimeData(Guid id, DateTime start, DateTime? end)
	{
		Id = id;
		Start = start;
		End = end;
	}
}

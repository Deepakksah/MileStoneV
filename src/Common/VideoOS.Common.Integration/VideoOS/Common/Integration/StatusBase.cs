using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public class StatusBase
{
	[DataMember]
	public DateTime Time;

	[DataMember]
	public long SequenceNo;

	public StatusBase(DateTime time)
	{
		Time = time;
		SequenceNo = 0L;
	}
}

using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Agents;

public class TimeDuration
{
	[DataMember(Order = 0)]
	public long MicroSeconds { get; set; }
}

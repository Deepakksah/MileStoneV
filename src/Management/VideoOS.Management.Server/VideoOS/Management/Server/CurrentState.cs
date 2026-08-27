using System;
using System.Runtime.Serialization;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

[DataContract]
public class CurrentState
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public Guid ParentId { get; set; }

	[DataMember]
	public int StateType { get; set; }

	[DataMember]
	public CounterState State { get; set; }

	[DataMember]
	public DateTime UpdateTime { get; set; }

	[DataMember]
	public double TriggerValue { get; set; }

	public int DataCollectorId { get; set; }

	public CurrentState(Guid id, Guid parentId, int type, CounterState state, double triggerValue, DateTime updateTime, int dataCollectorId)
	{
		Id = id;
		ParentId = parentId;
		StateType = type;
		State = state;
		UpdateTime = updateTime;
		TriggerValue = triggerValue;
		DataCollectorId = dataCollectorId;
	}
}

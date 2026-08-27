using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.EventServer.Shared.Aggregation;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AlarmClientAggregator
{
	private readonly AlarmCommandClientManager _manager;

	public AlarmClientAggregator(AlarmCommandClientManager manager)
	{
		_manager = manager;
	}

	private AlarmClient GetClient(ServerId serverId)
	{
		return _manager.GetAlarmCommandClient(serverId) ?? throw new ArgumentException("No AlarmClient for serverId " + serverId?.ToString() + " found");
	}

	public Task<AggregationResult<ServerId, GetEventLines2Resp>> AggregateGetEventLines2Async(IEnumerable<ServerId> serverIds, GetEventLines2Params parameters)
	{
		return AggregationHelper.AggregateAsync(serverIds, delegate(ServerId serverId)
		{
			AlarmClient client = GetClient(serverId);
			return Task<GetEventLines2Resp>.Factory.FromAsync(client.BeginGetEventLines2, client.EndGetEventLines2, parameters, null);
		});
	}
}

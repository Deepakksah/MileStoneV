using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Rule.Events;

namespace VideoOS.DataCollector.Service;

public class RecordingServiceAvailableRule : ServiceAvailableRule
{
	public RecordingServiceAvailableRule(int dataCollectorId, string name, string serviceName, string serviceType, string id, IStateHandlerFactory stateHandlerFactory)
		: base(dataCollectorId, name, serviceName, serviceType, id, stateHandlerFactory)
	{
	}

	public override async Task SampleIfDue(ISamplerFactory samplerFactory, CancellationToken cancellationToken)
	{
		await base.SampleIfDue(samplerFactory, cancellationToken);
		if (base.DomainEvents?.FirstOrDefault((INotification e) => e is StateChangeEvent) is StateChangeEvent stateChangeEvent)
		{
			if (stateChangeEvent.StateChange.State == 3)
			{
				AddDomainEvent(new RecordingServerChangeStateEvent(up: false));
			}
			else if (stateChangeEvent.StateChange.State == 1)
			{
				AddDomainEvent(new RecordingServerChangeStateEvent(up: true));
			}
		}
	}
}

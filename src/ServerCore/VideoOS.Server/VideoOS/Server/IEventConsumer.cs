using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server;

public interface IEventConsumer
{
	void SentEventToConsumer(PostEventBody body);

	void SentEventToConsumer(TriggerEvent[] eventStatus);
}

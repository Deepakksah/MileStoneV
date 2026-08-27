using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public interface IEventHandler
{
	void AddEventReceiver(IEventReceiver eventReceiver);

	bool RemoveEventReceiver(IEventReceiver eventReceiver);
}

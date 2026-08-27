using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoOS.Platform.EventsAndState;

public interface IEventReceiver
{
	Task OnEventsReceivedAsync(IEnumerable<Event> events);

	Task OnConnectionStateChangedAsync(ConnectionState newState);
}

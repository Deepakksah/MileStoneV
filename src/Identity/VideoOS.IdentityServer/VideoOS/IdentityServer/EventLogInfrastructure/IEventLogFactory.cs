using System.Threading.Tasks;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public interface IEventLogFactory
{
	Task<EventLog> Create(string eventName, string objectId, string objectName);
}

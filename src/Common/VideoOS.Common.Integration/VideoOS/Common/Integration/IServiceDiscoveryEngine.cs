using System;

namespace VideoOS.Common.Integration;

public interface IServiceDiscoveryEngine
{
	void CommunicationConfirmed(Guid serviceId);

	void CommunicationError(Guid serviceId);
}

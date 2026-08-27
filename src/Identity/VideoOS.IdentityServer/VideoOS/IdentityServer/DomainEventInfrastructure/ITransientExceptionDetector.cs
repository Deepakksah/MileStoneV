using System;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public interface ITransientExceptionDetector
{
	bool IsTransient(Exception ex);
}

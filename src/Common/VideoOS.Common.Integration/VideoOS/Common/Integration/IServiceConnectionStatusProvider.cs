using System;

namespace VideoOS.Common.Integration;

public interface IServiceConnectionStatusProvider
{
	IServiceConnectionStatus GetService(Guid serviceId);

	IServiceConnectionStatus[] GetServices();
}

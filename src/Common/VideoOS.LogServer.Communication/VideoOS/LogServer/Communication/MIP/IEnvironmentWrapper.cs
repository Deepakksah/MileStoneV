using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.LogServer.Communication.MIP;

public interface IEnvironmentWrapper
{
	ServerId ServerId { get; }

	LoginSettings LoginSettings { get; }

	IEnumerable<Configuration.ServiceURIInfo> GetRegisteredServiceUriInfo(Guid serviceType);
}

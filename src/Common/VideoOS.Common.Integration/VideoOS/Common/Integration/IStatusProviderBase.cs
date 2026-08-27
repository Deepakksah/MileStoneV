using System;
using System.ServiceModel;

namespace VideoOS.Common.Integration;

[ServiceContract]
public interface IStatusProviderBase
{
	[OperationContract]
	Guid StartStatusSession();

	[OperationContract]
	void StopStatusSession(Guid statusSessionId);

	[OperationContract]
	StatusBase[] GetStatus(Guid statusSessionId, int timeoutMs);
}

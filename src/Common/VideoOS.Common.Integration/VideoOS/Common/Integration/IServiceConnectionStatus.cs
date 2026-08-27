using System;

namespace VideoOS.Common.Integration;

public interface IServiceConnectionStatus
{
	TimeSpan Timeout { get; set; }

	event EventHandler<ServiceConnectionOnlineEventArgs> ServiceOnline;

	event EventHandler<ServiceConnectionOfflineEventArgs> ServiceOffline;

	event EventHandler<ServerCommunicationBrokenEventArgs> CommunicationBroken;

	void CommunicationConfirmed();

	void CommunicationError();

	void AuthenticationError();

	void EndpointNotFound();

	void InvalidCertificate();

	void CheckTimeout();
}

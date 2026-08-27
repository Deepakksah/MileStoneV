using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public interface IServer
{
	Version Version { get; }

	ILogger Logger { get; }

	bool IsClosing { get; }

	bool IsInitialized { get; }

	Guid HomeSiteId { get; }

	IEventConsumer EVSEventConsumer { get; }

	IServerStorageManager ServerStorageManager { get; }

	ISymmetricEncryption CorporateEncryptionSymmetric { get; }

	ICallContextInfo CreateServerServiceCallContextUserInfo();

	void UpdateEventTypes(string eventDefinitions, ICallContextInfo callContextInfo);
}

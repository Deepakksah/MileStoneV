using System;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server;

public interface IServerServiceSupport
{
	ILogger Logger { get; }

	event EventHandler<StatusMessageEventArgs> StatusMessage;

	event EventHandler<ShutdownRequestedEventArgs> ShutdownRequested;

	void Initialize();

	void Close(bool skipDatabaseLog);
}

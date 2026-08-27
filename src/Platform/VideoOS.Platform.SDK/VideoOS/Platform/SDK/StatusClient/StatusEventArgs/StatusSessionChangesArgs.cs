using System;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class StatusSessionChangesArgs : EventArgs
{
	private readonly Item _recorderServer;

	private readonly ConnectStates _connectionState;

	public Item RecorderServer => _recorderServer;

	public ConnectStates ConnectionState => _connectionState;

	public StatusSessionChangesArgs(Item recorderServer, ConnectStates connectionState)
	{
		_recorderServer = recorderServer;
		_connectionState = connectionState;
	}
}

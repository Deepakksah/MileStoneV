using System;

namespace VideoOS.Server.RecorderCommunication;

public interface ICommunicationProvider
{
	event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;

	void Initialize(ServerConfig config);

	void Close();
}

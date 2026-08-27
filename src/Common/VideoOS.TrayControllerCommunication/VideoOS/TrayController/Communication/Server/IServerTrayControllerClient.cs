using System.Collections.Generic;

namespace VideoOS.TrayController.Communication.Server;

public interface IServerTrayControllerClient
{
	List<LogMessage> GetMessages(int sequenceNumber);

	ServerState GetServerState();

	void CloseChannel();
}

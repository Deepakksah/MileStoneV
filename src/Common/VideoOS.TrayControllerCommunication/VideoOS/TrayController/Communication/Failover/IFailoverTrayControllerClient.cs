using System.Collections.Generic;

namespace VideoOS.TrayController.Communication.Failover;

public interface IFailoverTrayControllerClient
{
	List<LogMessage> GetMessages(int sequenceNumber);

	FailoverState GetFailoverState();

	void CloseChannel();
}

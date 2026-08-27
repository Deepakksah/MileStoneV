using System.Collections.Generic;

namespace VideoOS.TrayController.Communication;

public interface IMessageProvider
{
	IEnumerable<LogMessage> GetMessages(int sequenceNumber);
}

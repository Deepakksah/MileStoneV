using System;
using System.Threading.Tasks;

namespace VideoOS.Platform.Common.MessageClient;

public interface ICommandAcknowledgementHandler
{
	Task CommandResult(Guid commandId);

	bool SetCommandResult(Guid commandId);
}

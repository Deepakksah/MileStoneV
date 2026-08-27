using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public interface IMessageManagerCallback
{
	void HandleTimeout(CommandBase command);

	void HandleResponse(CommandBase command, ResponseBase response);

	bool IsLastExpectedResponse(CommandBase command, ResponseBase response);
}

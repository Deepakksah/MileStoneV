namespace VideoOS.Common.Integration.Command;

public interface IResponseHandler
{
	void HandleResponse(CommandBase command, ResponseBase response);
}

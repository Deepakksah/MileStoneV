namespace VideoOS.Common.Integration.Command;

public interface IResponseSink
{
	void AddResponse(ResponseBase response);

	void ProcessSync(CommandBase command, object state);

	void ProcessAsync(CommandBase command, object state);
}

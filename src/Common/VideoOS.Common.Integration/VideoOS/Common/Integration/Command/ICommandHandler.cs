namespace VideoOS.Common.Integration.Command;

public interface ICommandHandler
{
	ICommandHandler Clone();

	void Execute(CommandBase command, object state, IResponseSink responseSink);
}

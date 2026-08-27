namespace VideoOS.Common.Integration.Command;

public class CommandPackage
{
	public CommandBase CommandBase { get; private set; }

	public ICallContextInfo CallContextInfo { get; private set; }

	public CommandPackage(CommandBase commandBase, ICallContextInfo callContextInfo)
	{
		CommandBase = commandBase;
		CallContextInfo = callContextInfo;
	}
}

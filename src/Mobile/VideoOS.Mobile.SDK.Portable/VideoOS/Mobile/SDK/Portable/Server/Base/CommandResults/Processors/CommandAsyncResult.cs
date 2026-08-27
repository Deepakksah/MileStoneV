using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;

public class CommandAsyncResult : CommandAsyncResultBase
{
	public CommandAsyncResult(IConnection connection)
		: base(connection)
	{
	}
}

using VideoOS.Common.Integration.Threading;

namespace VideoOS.Common.Integration.Command;

public interface IAdvancedThreadPoolCommandHandler
{
	SmartThreadPool ThreadPool { get; }
}

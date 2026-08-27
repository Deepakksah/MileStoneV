using System.Threading.Tasks;

namespace VideoOS.Server;

public interface IBackgroundService
{
	bool IsStarted { get; }

	Task Start();

	Task Stop();
}

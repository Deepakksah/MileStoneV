using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Tasks;

public interface ITaskCommandFactory
{
	TaskCommandBase CreateTaskCommand(ServerTaskDto task);
}

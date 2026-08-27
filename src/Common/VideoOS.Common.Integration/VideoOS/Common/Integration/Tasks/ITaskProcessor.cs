using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Tasks;

public interface ITaskProcessor
{
	void UpdateConfiguration(ConfigurationDataSet diffGram);

	void UpdateSecurityConfiguration(SecurityDataSet diffGram);

	void ProcessTaskCommand(TaskCommandBase command, IResponseHandler responseHandler);
}

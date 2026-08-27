using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.Tasks;

namespace VideoOS.Server.Tasks;

public class ServerTaskProcessor : ITaskProcessor
{
	public void UpdateConfiguration(ConfigurationDataSet diffGram)
	{
		UpdateConfigurationManagerCommand command = new UpdateConfigurationManagerCommand(new ConfigurationManagerDataTransferObject(diffGram));
		CommandProcessor.ProcessAsync(command, Server.Instance.CreateServerServiceCallContextUserInfo(), new NullResponseSink());
	}

	public void UpdateSecurityConfiguration(SecurityDataSet diffGram)
	{
		AnalyzeUpdateSecurityManagerCommand command = new AnalyzeUpdateSecurityManagerCommand(new SecurityManagerDataTransferObject(diffGram), doAuditLog: false);
		CommandProcessor.ProcessAsync(command, Server.Instance.CreateServerServiceCallContextUserInfo(), new NullResponseSink());
	}

	public void ProcessTaskCommand(TaskCommandBase command, IResponseHandler responseHandler)
	{
		CommandProcessor.ProcessAsync(command, Server.Instance.CreateServerServiceCallContextUserInfo(), responseHandler);
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class InterCommunicationCommandClient : ClientBase<IInterCommunicationCommand>, IInterCommunicationCommand
{
	public InterCommunicationCommandClient()
	{
	}

	public InterCommunicationCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public InterCommunicationCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public InterCommunicationCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public InterCommunicationCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public MapObjectState[] GetStatus(InterCommunicationUserID[] userIDs, string ID)
	{
		return base.Channel.GetStatus(userIDs, ID);
	}

	public void ReloadConfiguration()
	{
		base.Channel.ReloadConfiguration();
	}

	public bool IsReloadConfigurationStarted()
	{
		return base.Channel.IsReloadConfigurationStarted();
	}

	public bool IsReloadConfigurationDone()
	{
		return base.Channel.IsReloadConfigurationDone();
	}

	public int ReloadConfigurationProgress()
	{
		return base.Channel.ReloadConfigurationProgress();
	}

	public InterCommunicationUserID GetServiceAccount()
	{
		return base.Channel.GetServiceAccount();
	}
}

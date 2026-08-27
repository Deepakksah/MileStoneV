using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ConfigrationReloadTokenClient : ClientBase<IConfigrationReloadToken>, IConfigrationReloadToken
{
	public ConfigrationReloadTokenClient()
	{
	}

	public ConfigrationReloadTokenClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ConfigrationReloadTokenClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigrationReloadTokenClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigrationReloadTokenClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void ReloadConfiguration(string token)
	{
		base.Channel.ReloadConfiguration(token);
	}

	public Task ReloadConfigurationAsync(string token)
	{
		return base.Channel.ReloadConfigurationAsync(token);
	}

	public bool IsReloadConfigurationStarted(string token)
	{
		return base.Channel.IsReloadConfigurationStarted(token);
	}

	public Task<bool> IsReloadConfigurationStartedAsync(string token)
	{
		return base.Channel.IsReloadConfigurationStartedAsync(token);
	}

	public bool IsReloadConfigurationDone(string token)
	{
		return base.Channel.IsReloadConfigurationDone(token);
	}

	public Task<bool> IsReloadConfigurationDoneAsync(string token)
	{
		return base.Channel.IsReloadConfigurationDoneAsync(token);
	}

	public int ReloadConfigurationProgress(string token)
	{
		return base.Channel.ReloadConfigurationProgress(token);
	}

	public Task<int> ReloadConfigurationProgressAsync(string token)
	{
		return base.Channel.ReloadConfigurationProgressAsync(token);
	}

	public ConfigurationReloadUserID GetServiceAccount(string token)
	{
		return base.Channel.GetServiceAccount(token);
	}

	public Task<ConfigurationReloadUserID> GetServiceAccountAsync(string token)
	{
		return base.Channel.GetServiceAccountAsync(token);
	}
}

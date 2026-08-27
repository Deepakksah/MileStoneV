using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Mip.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisMipItemCommandClient : ClientBase<IGisMipItemCommand>, IGisMipItemCommand
{
	public GisMipItemCommandClient()
	{
	}

	public GisMipItemCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisMipItemCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisMipItemCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisMipItemCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetMipItemsByKindAndAreaResp GetMipItemsByKindAndArea(GetMipItemsByKindAndAreaParams parameters)
	{
		return base.Channel.GetMipItemsByKindAndArea(parameters);
	}

	public Task<GetMipItemsByKindAndAreaResp> GetMipItemsByKindAndAreaAsync(GetMipItemsByKindAndAreaParams parameters)
	{
		return base.Channel.GetMipItemsByKindAndAreaAsync(parameters);
	}

	public GetMipItemsByKindResp GetMipItemsByKind(GetMipItemsByKindParams parameters)
	{
		return base.Channel.GetMipItemsByKind(parameters);
	}

	public Task<GetMipItemsByKindResp> GetMipItemsByKindAsync(GetMipItemsByKindParams parameters)
	{
		return base.Channel.GetMipItemsByKindAsync(parameters);
	}

	public GetMipPluginsResp GetMipPlugins(GetMipPluginsParams parameters)
	{
		return base.Channel.GetMipPlugins(parameters);
	}

	public Task<GetMipPluginsResp> GetMipPluginsAsync(GetMipPluginsParams parameters)
	{
		return base.Channel.GetMipPluginsAsync(parameters);
	}

	public GetMipItemMenuActionsResp GetMipItemMenuActions(GetMipItemMenuActionsParams parameters)
	{
		return base.Channel.GetMipItemMenuActions(parameters);
	}

	public Task<GetMipItemMenuActionsResp> GetMipItemMenuActionsAsync(GetMipItemMenuActionsParams parameters)
	{
		return base.Channel.GetMipItemMenuActionsAsync(parameters);
	}

	public ActivatePluginActionResp ActivatePluginAction(ActivatePluginActionParams parameters)
	{
		return base.Channel.ActivatePluginAction(parameters);
	}

	public Task<ActivatePluginActionResp> ActivatePluginActionAsync(ActivatePluginActionParams parameters)
	{
		return base.Channel.ActivatePluginActionAsync(parameters);
	}
}

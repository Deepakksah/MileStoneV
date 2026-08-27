using System;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Mip.Client;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Mip;

public abstract class MipClientBase : AbstractClient<GisMipItemCommandClient, IGisMipItemCommand>, IMipClient, IClientBase, IDisposable
{
	protected sealed override string Name => "GisMipItemService";

	protected MipClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	public async Task<GetMipItemsByKindAndAreaResp> GetMipItemsByKindAndAreaAsync(GetMipItemsByKindAndAreaParams parameters)
	{
		return await CallWithRetryAsync<GetMipItemsByKindAndAreaResp>((GisMipItemCommandClient client) => async () => await client.GetMipItemsByKindAndAreaAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetMipItemsByKindResp> GetMipItemsByKindAsync(GetMipItemsByKindParams parameters)
	{
		return await CallWithRetryAsync<GetMipItemsByKindResp>((GisMipItemCommandClient client) => async () => await client.GetMipItemsByKindAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetMipPluginsResp> GetMipPluginsAsync(GetMipPluginsParams parameters)
	{
		return await CallWithRetryAsync<GetMipPluginsResp>((GisMipItemCommandClient client) => async () => await client.GetMipPluginsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetMipItemMenuActionsResp> GetMipItemMenuActionsAsync(GetMipItemMenuActionsParams parameters)
	{
		return await CallWithRetryAsync<GetMipItemMenuActionsResp>((GisMipItemCommandClient client) => async () => await client.GetMipItemMenuActionsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public GetMipItemsByKindAndAreaResp GetMipItemsByKindAndArea(GetMipItemsByKindAndAreaParams parameters)
	{
		return CallWithRetry<GetMipItemsByKindAndAreaResp>((GisMipItemCommandClient client) => () => client.GetMipItemsByKindAndArea(parameters));
	}

	public GetMipItemsByKindResp GetMipItemsByKind(GetMipItemsByKindParams parameters)
	{
		return CallWithRetry<GetMipItemsByKindResp>((GisMipItemCommandClient client) => () => client.GetMipItemsByKind(parameters));
	}

	public GetMipPluginsResp GetMipPlugins(GetMipPluginsParams parameters)
	{
		return CallWithRetry<GetMipPluginsResp>((GisMipItemCommandClient client) => () => client.GetMipPlugins(parameters));
	}

	public GetMipItemMenuActionsResp GetMipItemMenuActions(GetMipItemMenuActionsParams parameters)
	{
		return CallWithRetry<GetMipItemMenuActionsResp>((GisMipItemCommandClient client) => () => client.GetMipItemMenuActions(parameters));
	}

	public ActivatePluginActionResp ActivatePluginAction(ActivatePluginActionParams parameters)
	{
		return CallWithRetry<ActivatePluginActionResp>((GisMipItemCommandClient client) => () => client.ActivatePluginAction(parameters));
	}

	public async Task<ActivatePluginActionResp> ActivatePluginActionAsync(ActivatePluginActionParams parameters)
	{
		return await CallWithRetryAsync<ActivatePluginActionResp>((GisMipItemCommandClient client) => async () => await client.ActivatePluginActionAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}
}

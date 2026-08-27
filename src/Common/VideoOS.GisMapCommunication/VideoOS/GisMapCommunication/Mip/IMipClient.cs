using System;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Mip;

public interface IMipClient : IClientBase, IDisposable
{
	Task<GetMipItemsByKindAndAreaResp> GetMipItemsByKindAndAreaAsync(GetMipItemsByKindAndAreaParams parameters);

	Task<GetMipItemsByKindResp> GetMipItemsByKindAsync(GetMipItemsByKindParams parameters);

	Task<GetMipPluginsResp> GetMipPluginsAsync(GetMipPluginsParams parameters);

	Task<GetMipItemMenuActionsResp> GetMipItemMenuActionsAsync(GetMipItemMenuActionsParams parameters);

	GetMipItemsByKindAndAreaResp GetMipItemsByKindAndArea(GetMipItemsByKindAndAreaParams parameters);

	GetMipItemsByKindResp GetMipItemsByKind(GetMipItemsByKindParams parameters);

	GetMipPluginsResp GetMipPlugins(GetMipPluginsParams parameters);

	GetMipItemMenuActionsResp GetMipItemMenuActions(GetMipItemMenuActionsParams parameters);

	ActivatePluginActionResp ActivatePluginAction(ActivatePluginActionParams parameters);

	Task<ActivatePluginActionResp> ActivatePluginActionAsync(ActivatePluginActionParams parameters);
}

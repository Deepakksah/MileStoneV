using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class PluginCommandClient : ClientBase<IPluginCommand>, IPluginCommand
{
	public PluginCommandClient()
	{
	}

	public PluginCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public PluginCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PluginCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PluginCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetVersionResp GetVersion(string token, Guid serverId, GetVersionParams parameters)
	{
		return base.Channel.GetVersion(token, serverId, parameters);
	}

	public Task<GetVersionResp> GetVersionAsync(string token, Guid serverId, GetVersionParams parameters)
	{
		return base.Channel.GetVersionAsync(token, serverId, parameters);
	}

	public GetStatusResp GetStatus(string token, Guid serverId, GetStatusParams parameters)
	{
		return base.Channel.GetStatus(token, serverId, parameters);
	}

	public Task<GetStatusResp> GetStatusAsync(string token, Guid serverId, GetStatusParams parameters)
	{
		return base.Channel.GetStatusAsync(token, serverId, parameters);
	}

	public GetIntradaCountryModulesResp GetIntradaCountryModules(string token, Guid serverId, GetIntradaCountryModulesParams parameters)
	{
		return base.Channel.GetIntradaCountryModules(token, serverId, parameters);
	}

	public Task<GetIntradaCountryModulesResp> GetIntradaCountryModulesAsync(string token, Guid serverId, GetIntradaCountryModulesParams parameters)
	{
		return base.Channel.GetIntradaCountryModulesAsync(token, serverId, parameters);
	}

	public SystemConfigurationHasChangedResp SystemConfigurationHasChanged(string token, Guid serverId, SystemConfigurationHasChangedParams parameters)
	{
		return base.Channel.SystemConfigurationHasChanged(token, serverId, parameters);
	}

	public Task<SystemConfigurationHasChangedResp> SystemConfigurationHasChangedAsync(string token, Guid serverId, SystemConfigurationHasChangedParams parameters)
	{
		return base.Channel.SystemConfigurationHasChangedAsync(token, serverId, parameters);
	}

	public AddLicensePlateListResp AddLicensePlateList(string token, AddLicensePlateListParams parameters)
	{
		return base.Channel.AddLicensePlateList(token, parameters);
	}

	public Task<AddLicensePlateListResp> AddMatchListAsync(string token, AddLicensePlateListParams parameters)
	{
		return base.Channel.AddMatchListAsync(token, parameters);
	}

	public DeleteLicensePlateListResp DeleteLicensePlateList(string token, DeleteLicensePlateListParams parameters)
	{
		return base.Channel.DeleteLicensePlateList(token, parameters);
	}

	public Task<DeleteLicensePlateListResp> DeleteMatchListAsync(string token, DeleteLicensePlateListParams parameters)
	{
		return base.Channel.DeleteMatchListAsync(token, parameters);
	}

	public RenameLicensePlateListResp RenameLicensePlateList(string token, RenameLicensePlateListParams parameters)
	{
		return base.Channel.RenameLicensePlateList(token, parameters);
	}

	public Task<RenameLicensePlateListResp> RenameMatchListAsync(string token, RenameLicensePlateListParams parameters)
	{
		return base.Channel.RenameMatchListAsync(token, parameters);
	}

	public ModifyLicensePlateListResp ModifyLicensePlateList(string token, ModifyLicensePlateListParams parameters)
	{
		return base.Channel.ModifyLicensePlateList(token, parameters);
	}

	public Task<ModifyLicensePlateListResp> ModifyMatchListAsync(string token, ModifyLicensePlateListParams parameters)
	{
		return base.Channel.ModifyMatchListAsync(token, parameters);
	}

	public AddLicensePlatesResp AddLicensePlates(string token, AddLicensePlatesParams parameters)
	{
		return base.Channel.AddLicensePlates(token, parameters);
	}

	public Task<AddLicensePlatesResp> AddMatchListItemsAsync(string token, AddLicensePlatesParams parameters)
	{
		return base.Channel.AddMatchListItemsAsync(token, parameters);
	}

	public DeleteLicensePlatesResp DeleteLicensePlates(string token, DeleteLicensePlatesParams parameters)
	{
		return base.Channel.DeleteLicensePlates(token, parameters);
	}

	public Task<DeleteLicensePlatesResp> DeleteMatchListItemsAsync(string token, DeleteLicensePlatesParams parameters)
	{
		return base.Channel.DeleteMatchListItemsAsync(token, parameters);
	}

	public ClearLicensePlatesResp ClearLicensePlates(string token, ClearLicensePlatesParams parameters)
	{
		return base.Channel.ClearLicensePlates(token, parameters);
	}

	public Task<ClearLicensePlatesResp> ClearMatchListItemsAsync(string token, ClearLicensePlatesParams parameters)
	{
		return base.Channel.ClearMatchListItemsAsync(token, parameters);
	}

	public GetLicensePlateCountResp GetLicensePlateCount(string token, GetLicensePlateCountParams parameters)
	{
		return base.Channel.GetLicensePlateCount(token, parameters);
	}

	public Task<GetLicensePlateCountResp> GetMatchListItemsCountAsync(string token, GetLicensePlateCountParams parameters)
	{
		return base.Channel.GetMatchListItemsCountAsync(token, parameters);
	}

	public GetLicensePlateListsResp GetLicensePlateLists(string token, GetLicensePlateListsParams parameters)
	{
		return base.Channel.GetLicensePlateLists(token, parameters);
	}

	public Task<GetLicensePlateListsResp> GetMatchListsAsync(string token, GetLicensePlateListsParams parameters)
	{
		return base.Channel.GetMatchListsAsync(token, parameters);
	}

	public GetLicensePlatesResp GetLicensePlates(string token, GetLicensePlatesParams parameters)
	{
		return base.Channel.GetLicensePlates(token, parameters);
	}

	public Task<GetLicensePlatesResp> GetMatchListItemsAsync(string token, GetLicensePlatesParams parameters)
	{
		return base.Channel.GetMatchListItemsAsync(token, parameters);
	}

	public LookupLicensePlateInfosResp LookupLicensePlateInfos(string token, LookupLicensePlateInfosParams parameters)
	{
		return base.Channel.LookupLicensePlateInfos(token, parameters);
	}

	public Task<LookupLicensePlateInfosResp> LookupMatchListItemInfosAsync(string token, LookupLicensePlateInfosParams parameters)
	{
		return base.Channel.LookupMatchListItemInfosAsync(token, parameters);
	}

	public SearchLicensePlateEventsResp SearchLicensePlateEvents(string token, SearchLicensePlateEventsParams parameters)
	{
		return base.Channel.SearchLicensePlateEvents(token, parameters);
	}

	public GetVideoSourcesResp GetVideoSources(string token, GetVideoSourcesParams parameters)
	{
		return base.Channel.GetVideoSources(token, parameters);
	}

	public Task<GetVideoSourcesResp> GetVideoSourcesAsync(string token, GetVideoSourcesParams parameters)
	{
		return base.Channel.GetVideoSourcesAsync(token, parameters);
	}

	public GetAllCountryCodesResp GetAllCountryCodes(string token, GetAllCountryCodesParams parameters)
	{
		return base.Channel.GetAllCountryCodes(token, parameters);
	}

	public Task<GetAllCountryCodesResp> GetAllCountryCodesAsync(string token, GetAllCountryCodesParams parameters)
	{
		return base.Channel.GetAllCountryCodesAsync(token, parameters);
	}

	public AnalyzeJpegResp AnalyzeJpeg(string token, Guid serverId, AnalyzeJpegParams parameters)
	{
		return base.Channel.AnalyzeJpeg(token, serverId, parameters);
	}

	public Task<AnalyzeJpegResp> AnalyzeJpegAsync(string token, Guid serverId, AnalyzeJpegParams parameters)
	{
		return base.Channel.AnalyzeJpegAsync(token, serverId, parameters);
	}

	public SetSnapshotsResp SetSnapshots(string token, SetSnapshotsParams parameters)
	{
		return base.Channel.SetSnapshots(token, parameters);
	}

	public Task<SetSnapshotsResp> SetSnapshotsAsync(string token, SetSnapshotsParams parameters)
	{
		return base.Channel.SetSnapshotsAsync(token, parameters);
	}

	public DeleteSnapshotsResp DeleteSnapshots(string token, DeleteSnapshotsParams parameters)
	{
		return base.Channel.DeleteSnapshots(token, parameters);
	}

	public Task<DeleteSnapshotsResp> DeleteSnapshotsAsync(string token, DeleteSnapshotsParams parameters)
	{
		return base.Channel.DeleteSnapshotsAsync(token, parameters);
	}

	public GetSnapshotsResp GetSnapshots(string token, GetSnapshotsParams parameters)
	{
		return base.Channel.GetSnapshots(token, parameters);
	}

	public Task<GetSnapshotsResp> GetSnapshotsAsync(string token, GetSnapshotsParams parameters)
	{
		return base.Channel.GetSnapshotsAsync(token, parameters);
	}

	public Task<GetCountryModulesWithLicensePlateStylesResp> GetCountryModulesWithLicensePlateStylesAsync(string token, GetCountryModulesWithLicensePlateStylesParams parameters)
	{
		return base.Channel.GetCountryModulesWithLicensePlateStylesAsync(token, parameters);
	}

	public Task<PatchCountryModulesLicensePlateStylesResp> PatchCountryModulesPlateStylesAsync(string token, PatchCountryModulesLicensePlateStylesParams parameters)
	{
		return base.Channel.PatchCountryModulesPlateStylesAsync(token, parameters);
	}

	public Task<GetLicensePlateStyleAliasResp> GetLicensePlateStyleAliasesAsync(string token, GetLicensePlateStyleAliasParams parameters)
	{
		return base.Channel.GetLicensePlateStyleAliasesAsync(token, parameters);
	}

	public Task<PatchLicensePlateStyleAliasResp> PatchLicensePlateStyleAliasesAsync(string token, PatchLicensePlateStyleAliasParams parameters)
	{
		return base.Channel.PatchLicensePlateStyleAliasesAsync(token, parameters);
	}

	public Task<DeleteLicensePlateStyleAliasResp> DeleteLicensePlateStyleAliasesAsync(string token, DeleteLicensePlateStyleAliasParams parameters)
	{
		return base.Channel.DeleteLicensePlateStyleAliasesAsync(token, parameters);
	}
}

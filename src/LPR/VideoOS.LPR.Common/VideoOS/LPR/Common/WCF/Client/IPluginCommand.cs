using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/LPRCommand", ConfigurationName = "VideoOS.LPR.Common.WCF.Client.IPluginCommand", SessionMode = SessionMode.NotAllowed)]
public interface IPluginCommand
{
	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVersion", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVersionResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVersionServiceFaultFault", Name = "ServiceFault")]
	GetVersionResp GetVersion(string token, Guid serverId, GetVersionParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVersion", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVersionResponse")]
	Task<GetVersionResp> GetVersionAsync(string token, Guid serverId, GetVersionParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetStatus", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetStatusResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetStatusServiceFaultFault", Name = "ServiceFault")]
	GetStatusResp GetStatus(string token, Guid serverId, GetStatusParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetStatus", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetStatusResponse")]
	Task<GetStatusResp> GetStatusAsync(string token, Guid serverId, GetStatusParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetIntradaCountryModules", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetIntradaCountryModulesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetIntradaCountryModulesServiceFaultFault", Name = "ServiceFault")]
	GetIntradaCountryModulesResp GetIntradaCountryModules(string token, Guid serverId, GetIntradaCountryModulesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetIntradaCountryModules", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetIntradaCountryModulesResponse")]
	Task<GetIntradaCountryModulesResp> GetIntradaCountryModulesAsync(string token, Guid serverId, GetIntradaCountryModulesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SystemConfigurationHasChanged", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/SystemConfigurationHasChangedResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SystemConfigurationHasChangedServiceFaultFault", Name = "ServiceFault")]
	SystemConfigurationHasChangedResp SystemConfigurationHasChanged(string token, Guid serverId, SystemConfigurationHasChangedParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SystemConfigurationHasChanged", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/SystemConfigurationHasChangedResponse")]
	Task<SystemConfigurationHasChangedResp> SystemConfigurationHasChangedAsync(string token, Guid serverId, SystemConfigurationHasChangedParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AddLicensePlateList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/AddLicensePlateListResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AddLicensePlateListServiceFaultFault", Name = "ServiceFault")]
	AddLicensePlateListResp AddLicensePlateList(string token, AddLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AddMatchList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/AddMatchListResponse")]
	Task<AddLicensePlateListResp> AddMatchListAsync(string token, AddLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlateList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlateListResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlateListServiceFaultFault", Name = "ServiceFault")]
	DeleteLicensePlateListResp DeleteLicensePlateList(string token, DeleteLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteMatchList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteMatchListResponse")]
	Task<DeleteLicensePlateListResp> DeleteMatchListAsync(string token, DeleteLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/RenameLicensePlateList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/RenameLicensePlateListResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/RenameLicensePlateListServiceFaultFault", Name = "ServiceFault")]
	RenameLicensePlateListResp RenameLicensePlateList(string token, RenameLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/RenameMatchList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/RenameMatchListResponse")]
	Task<RenameLicensePlateListResp> RenameMatchListAsync(string token, RenameLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/ModifyLicensePlateList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/ModifyLicensePlateListResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/ModifyLicensePlateListServiceFaultFault", Name = "ServiceFault")]
	ModifyLicensePlateListResp ModifyLicensePlateList(string token, ModifyLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/ModifyMatchList", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/ModifyMatchListResponse")]
	Task<ModifyLicensePlateListResp> ModifyMatchListAsync(string token, ModifyLicensePlateListParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AddLicensePlates", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/AddLicensePlatesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AddLicensePlatesServiceFaultFault", Name = "ServiceFault")]
	AddLicensePlatesResp AddLicensePlates(string token, AddLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AddMatchListItems", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/AddMatchListItemsResponse")]
	Task<AddLicensePlatesResp> AddMatchListItemsAsync(string token, AddLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlates", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlatesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlatesServiceFaultFault", Name = "ServiceFault")]
	DeleteLicensePlatesResp DeleteLicensePlates(string token, DeleteLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteMatchListItems", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteMatchListItemsResponse")]
	Task<DeleteLicensePlatesResp> DeleteMatchListItemsAsync(string token, DeleteLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/ClearLicensePlates", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/ClearLicensePlatesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/ClearLicensePlatesServiceFaultFault", Name = "ServiceFault")]
	ClearLicensePlatesResp ClearLicensePlates(string token, ClearLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/ClearMatchListItems", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/ClearMatchListItemsResponse")]
	Task<ClearLicensePlatesResp> ClearMatchListItemsAsync(string token, ClearLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateCount", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateCountResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateCountServiceFaultFault", Name = "ServiceFault")]
	GetLicensePlateCountResp GetLicensePlateCount(string token, GetLicensePlateCountParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetMatchListItemsCount", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetMatchListItemsCountResponse")]
	Task<GetLicensePlateCountResp> GetMatchListItemsCountAsync(string token, GetLicensePlateCountParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateLists", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateListsResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateListsServiceFaultFault", Name = "ServiceFault")]
	GetLicensePlateListsResp GetLicensePlateLists(string token, GetLicensePlateListsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetMatchLists", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetMatchListsResponse")]
	Task<GetLicensePlateListsResp> GetMatchListsAsync(string token, GetLicensePlateListsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlates", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlatesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlatesServiceFaultFault", Name = "ServiceFault")]
	GetLicensePlatesResp GetLicensePlates(string token, GetLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetMatchListItems", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetMatchListItemsResponse")]
	Task<GetLicensePlatesResp> GetMatchListItemsAsync(string token, GetLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/LookupLicensePlateInfos", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/LookupLicensePlateInfosResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/LookupLicensePlateInfosServiceFaultFault", Name = "ServiceFault")]
	LookupLicensePlateInfosResp LookupLicensePlateInfos(string token, LookupLicensePlateInfosParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/LookupMatchListItemInfos", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/LookupMatchListItemInfosResponse")]
	Task<LookupLicensePlateInfosResp> LookupMatchListItemInfosAsync(string token, LookupLicensePlateInfosParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SearchLicensePlateEvents", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/SearchLicensePlateEventsResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SearchLicensePlateEventsServiceFaultFault", Name = "ServiceFault")]
	SearchLicensePlateEventsResp SearchLicensePlateEvents(string token, SearchLicensePlateEventsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVideoSources", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVideoSourcesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVideoSourcesServiceFaultFault", Name = "ServiceFault")]
	GetVideoSourcesResp GetVideoSources(string token, GetVideoSourcesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVideoSources", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetVideoSourcesResponse")]
	Task<GetVideoSourcesResp> GetVideoSourcesAsync(string token, GetVideoSourcesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetAllCountryCodes", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetAllCountryCodesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetAllCountryCodesServiceFaultFault", Name = "ServiceFault")]
	GetAllCountryCodesResp GetAllCountryCodes(string token, GetAllCountryCodesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetAllCountryCodes", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetAllCountryCodesResponse")]
	Task<GetAllCountryCodesResp> GetAllCountryCodesAsync(string token, GetAllCountryCodesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AnalyzeJpeg", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/AnalyzeJpegResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AnalyzeJpegServiceFaultFault", Name = "ServiceFault")]
	AnalyzeJpegResp AnalyzeJpeg(string token, Guid serverId, AnalyzeJpegParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/AnalyzeJpeg", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/AnalyzeJpegResponse")]
	Task<AnalyzeJpegResp> AnalyzeJpegAsync(string token, Guid serverId, AnalyzeJpegParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SetSnapshots", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/SetSnapshotsResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SetSnapshotsServiceFaultFault", Name = "ServiceFault")]
	SetSnapshotsResp SetSnapshots(string token, SetSnapshotsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/SetSnapshots", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/SetSnapshotsResponse")]
	Task<SetSnapshotsResp> SetSnapshotsAsync(string token, SetSnapshotsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteSnapshots", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteSnapshotsResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteSnapshotsServiceFaultFault", Name = "ServiceFault")]
	DeleteSnapshotsResp DeleteSnapshots(string token, DeleteSnapshotsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteSnapshots", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteSnapshotsResponse")]
	Task<DeleteSnapshotsResp> DeleteSnapshotsAsync(string token, DeleteSnapshotsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetSnapshots", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetSnapshotsResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetSnapshotsServiceFaultFault", Name = "ServiceFault")]
	GetSnapshotsResp GetSnapshots(string token, GetSnapshotsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetSnapshots", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetSnapshotsResponse")]
	Task<GetSnapshotsResp> GetSnapshotsAsync(string token, GetSnapshotsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetCountryModulesWithLicensePlateStyles", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetCountryModulesWithLicensePlateStylesResponse")]
	Task<GetCountryModulesWithLicensePlateStylesResp> GetCountryModulesWithLicensePlateStylesAsync(string token, GetCountryModulesWithLicensePlateStylesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/PatchCountryModulesPlateStyles", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/PatchCountryModulesPlateStylesResponse")]
	Task<PatchCountryModulesLicensePlateStylesResp> PatchCountryModulesPlateStylesAsync(string token, PatchCountryModulesLicensePlateStylesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateStyleAliases", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/GetLicensePlateStyleAliasesResponse")]
	Task<GetLicensePlateStyleAliasResp> GetLicensePlateStyleAliasesAsync(string token, GetLicensePlateStyleAliasParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/PatchLicensePlateStyleAliases", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/PatchLicensePlateStyleAliasesResponse")]
	Task<PatchLicensePlateStyleAliasResp> PatchLicensePlateStyleAliasesAsync(string token, PatchLicensePlateStyleAliasParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlateStyleAliases", ReplyAction = "http://videoos.net/2/LPRCommand/IPluginCommand/DeleteLicensePlateStyleAliasesResponse")]
	Task<DeleteLicensePlateStyleAliasResp> DeleteLicensePlateStyleAliasesAsync(string token, DeleteLicensePlateStyleAliasParams parameters);
}

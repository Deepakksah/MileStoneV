using System;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Server;

[ServiceContract(Name = "IPluginCommand", Namespace = "http://videoos.net/2/LPRCommand", SessionMode = SessionMode.NotAllowed)]
public interface IPluginCommand
{
	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetVersionResp GetVersion(string token, Guid serverId, GetVersionParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetStatusResp GetStatus(string token, Guid serverId, GetStatusParams parameters);

	[OperationContract(Name = "GetIntradaCountryModules")]
	[FaultContract(typeof(ServiceFault))]
	GetIntradaCountryModulesResp GetIntradaCountryModules(string token, Guid serverId, GetIntradaCountryModulesParams parameters);

	[OperationContract(Name = "GetIntradaCountryModulesAsync")]
	[FaultContract(typeof(ServiceFault))]
	Task<GetIntradaCountryModulesResp> GetIntradaCountryModulesAsync(string token, Guid serverId, GetIntradaCountryModulesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	SystemConfigurationHasChangedResp SystemConfigurationHasChanged(string token, Guid serverId, SystemConfigurationHasChangedParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	AddLicensePlateListResp AddLicensePlateList(string token, AddLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	DeleteLicensePlateListResp DeleteLicensePlateList(string token, DeleteLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	RenameLicensePlateListResp RenameLicensePlateList(string token, RenameLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	ModifyLicensePlateListResp ModifyLicensePlateList(string token, ModifyLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	AddLicensePlatesResp AddLicensePlates(string token, AddLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	DeleteLicensePlatesResp DeleteLicensePlates(string token, DeleteLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	ClearLicensePlatesResp ClearLicensePlates(string token, ClearLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetLicensePlateCountResp GetLicensePlateCount(string token, GetLicensePlateCountParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetLicensePlateListsResp GetLicensePlateLists(string token, GetLicensePlateListsParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetLicensePlatesResp GetLicensePlates(string token, GetLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	LookupLicensePlateInfosResp LookupLicensePlateInfos(string token, LookupLicensePlateInfosParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetVideoSourcesResp GetVideoSources(string token, GetVideoSourcesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetAllCountryCodesResp> GetAllCountryCodesAsync(string token, GetAllCountryCodesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<AnalyzeJpegResp> AnalyzeJpegAsync(string token, Guid serverId, AnalyzeJpegParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<SetSnapshotsResp> SetSnapshotsAsync(string token, SetSnapshotsParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<DeleteSnapshotsResp> DeleteSnapshotsAsync(string token, DeleteSnapshotsParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetSnapshotsResp> GetSnapshotsAsync(string token, GetSnapshotsParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetCountryModulesWithLicensePlateStylesResp> GetCountryModulesWithLicensePlateStylesAsync(string token, GetCountryModulesWithLicensePlateStylesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<PatchCountryModulesLicensePlateStylesResp> PatchCountryModulesPlateStylesAsync(string token, PatchCountryModulesLicensePlateStylesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetLicensePlateStyleAliasResp> GetLicensePlateStyleAliasesAsync(string token, GetLicensePlateStyleAliasParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<PatchLicensePlateStyleAliasResp> PatchLicensePlateStyleAliasesAsync(string token, PatchLicensePlateStyleAliasParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<DeleteLicensePlateStyleAliasResp> DeleteLicensePlateStyleAliasesAsync(string token, DeleteLicensePlateStyleAliasParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetLicensePlateListsResp> GetMatchListsAsync(string token, GetLicensePlateListsParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<AddLicensePlateListResp> AddMatchListAsync(string token, AddLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<DeleteLicensePlateListResp> DeleteMatchListAsync(string token, DeleteLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<RenameLicensePlateListResp> RenameMatchListAsync(string token, RenameLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<ModifyLicensePlateListResp> ModifyMatchListAsync(string token, ModifyLicensePlateListParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<AddLicensePlatesResp> AddMatchListItemsAsync(string token, AddLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<DeleteLicensePlatesResp> DeleteMatchListItemsAsync(string token, DeleteLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<ClearLicensePlatesResp> ClearMatchListItemsAsync(string token, ClearLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetLicensePlateCountResp> GetMatchListItemsCountAsync(string token, GetLicensePlateCountParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<GetLicensePlatesResp> GetMatchListItemsAsync(string token, GetLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	Task<LookupLicensePlateInfosResp> LookupMatchListItemInfosAsync(string token, LookupLicensePlateInfosParams parameters);
}

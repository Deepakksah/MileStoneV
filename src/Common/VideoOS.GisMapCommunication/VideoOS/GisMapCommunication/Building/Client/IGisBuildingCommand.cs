using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Building.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisBuildingCommand", ConfigurationName = "VideoOS.GisMapCommunication.Building.Client.IGisBuildingCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisBuildingCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildingResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildingGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddBuildingResp AddBuilding(AddBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildingResponse")]
	Task<AddBuildingResp> AddBuildingAsync(AddBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildings", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildingsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildingsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddBuildingsResp AddBuildings(AddBuildingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildings", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddBuildingsResponse")]
	Task<AddBuildingsResp> AddBuildingsAsync(AddBuildingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildingResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildingGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateBuildingResp UpdateBuilding(UpdateBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildingResponse")]
	Task<UpdateBuildingResp> UpdateBuildingAsync(UpdateBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildings", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildingsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildingsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateBuildingsResp UpdateBuildings(UpdateBuildingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildings", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateBuildingsResponse")]
	Task<UpdateBuildingsResp> UpdateBuildingsAsync(UpdateBuildingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetBuildingResp GetBuilding(GetBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingResponse")]
	Task<GetBuildingResp> GetBuildingAsync(GetBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingsByMap", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingsByMapResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingsByMapGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetBuildingsByMapResp GetBuildingsByMap(GetBuildingsByMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingsByMap", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetBuildingsByMapResponse")]
	Task<GetBuildingsByMapResp> GetBuildingsByMapAsync(GetBuildingsByMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildingResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildingGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteBuildingResp DeleteBuilding(DeleteBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildingResponse")]
	Task<DeleteBuildingResp> DeleteBuildingAsync(DeleteBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildings", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildingsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildingsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteBuildingsResp DeleteBuildings(DeleteBuildingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildings", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteBuildingsResponse")]
	Task<DeleteBuildingsResp> DeleteBuildingsAsync(DeleteBuildingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevelResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevelGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddLevelResp AddLevel(AddLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevelResponse")]
	Task<AddLevelResp> AddLevelAsync(AddLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevelsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevelsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddLevelsResp AddLevels(AddLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/AddLevelsResponse")]
	Task<AddLevelsResp> AddLevelsAsync(AddLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevelResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevelGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateLevelResp UpdateLevel(UpdateLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevelResponse")]
	Task<UpdateLevelResp> UpdateLevelAsync(UpdateLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevelsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevelsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateLevelsResp UpdateLevels(UpdateLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/UpdateLevelsResponse")]
	Task<UpdateLevelsResp> UpdateLevelsAsync(UpdateLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLevelsByBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLevelsByBuildingResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLevelsByBuildingGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetLevelsByBuildingResp GetLevelsByBuilding(GetLevelsByBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLevelsByBuilding", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLevelsByBuildingResponse")]
	Task<GetLevelsByBuildingResp> GetLevelsByBuildingAsync(GetLevelsByBuildingParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevelResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevelGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteLevelResp DeleteLevel(DeleteLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevelResponse")]
	Task<DeleteLevelResp> DeleteLevelAsync(DeleteLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevelsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevelsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteLevelsResp DeleteLevels(DeleteLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/DeleteLevelsResponse")]
	Task<DeleteLevelsResp> DeleteLevelsAsync(DeleteLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/ReorderLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/ReorderLevelsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/ReorderLevelsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	ReorderLevelsResp ReorderLevels(ReorderLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/ReorderLevels", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/ReorderLevelsResponse")]
	Task<ReorderLevelsResp> ReorderLevelsAsync(ReorderLevelsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLayersByLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLayersByLevelResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLayersByLevelGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetLayersByLevelResp GetLayersByLevel(GetLayersByLevelParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLayersByLevel", ReplyAction = "http://videoos.net/2/GisBuildingCommand/IGisBuildingCommand/GetLayersByLevelResponse")]
	Task<GetLayersByLevelResp> GetLayersByLevelAsync(GetLayersByLevelParams parameters);
}

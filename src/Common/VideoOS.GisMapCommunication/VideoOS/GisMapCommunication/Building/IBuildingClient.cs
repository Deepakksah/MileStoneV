using System;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Building;

public interface IBuildingClient : IClientBase, IDisposable
{
	Task<AddBuildingResp> AddBuildingAsync(AddBuildingParams parameters);

	Task<AddBuildingsResp> AddBuildingsAsync(AddBuildingsParams parameters);

	Task<UpdateBuildingResp> UpdateBuildingAsync(UpdateBuildingParams parameters);

	Task<UpdateBuildingsResp> UpdateBuildingsAsync(UpdateBuildingsParams parameters);

	Task<GetBuildingResp> GetBuildingAsync(GetBuildingParams parameters);

	Task<GetBuildingsByMapResp> GetBuildingsByMapAsync(GetBuildingsByMapParams parameters);

	Task<DeleteBuildingResp> DeleteBuildingAsync(DeleteBuildingParams parameters);

	Task<DeleteBuildingsResp> DeleteBuildingsAsync(DeleteBuildingsParams parameters);

	Task<AddLevelResp> AddLevelAsync(AddLevelParams parameters);

	Task<AddLevelsResp> AddLevelsAsync(AddLevelsParams parameters);

	Task<UpdateLevelResp> UpdateLevelAsync(UpdateLevelParams parameters);

	Task<UpdateLevelsResp> UpdateLevelsAsync(UpdateLevelsParams parameters);

	Task<GetLevelsByBuildingResp> GetLevelsByBuildingAsync(GetLevelsByBuildingParams parameters);

	Task<DeleteLevelResp> DeleteLevelAsync(DeleteLevelParams parameters);

	Task<DeleteLevelsResp> DeleteLevelsAsync(DeleteLevelsParams parameters);

	Task<ReorderLevelsResp> ReorderLevelsAsync(ReorderLevelsParams parameters);

	Task<GetLayersByLevelResp> GetLayersByLevelAsync(GetLayersByLevelParams parameters);

	AddBuildingResp AddBuilding(AddBuildingParams parameters);

	AddBuildingsResp AddBuildings(AddBuildingsParams parameters);

	UpdateBuildingResp UpdateBuilding(UpdateBuildingParams parameters);

	UpdateBuildingsResp UpdateBuildings(UpdateBuildingsParams parameters);

	GetBuildingResp GetBuilding(GetBuildingParams parameters);

	GetBuildingsByMapResp GetBuildingsByMap(GetBuildingsByMapParams parameters);

	DeleteBuildingResp DeleteBuilding(DeleteBuildingParams parameters);

	DeleteBuildingsResp DeleteBuildings(DeleteBuildingsParams parameters);

	AddLevelResp AddLevel(AddLevelParams parameters);

	AddLevelsResp AddLevels(AddLevelsParams parameters);

	UpdateLevelResp UpdateLevel(UpdateLevelParams parameters);

	UpdateLevelsResp UpdateLevels(UpdateLevelsParams parameters);

	GetLevelsByBuildingResp GetLevelsByBuilding(GetLevelsByBuildingParams parameters);

	DeleteLevelResp DeleteLevel(DeleteLevelParams parameters);

	DeleteLevelsResp DeleteLevels(DeleteLevelsParams parameters);

	ReorderLevelsResp ReorderLevels(ReorderLevelsParams parameters);

	GetLayersByLevelResp GetLayersByLevel(GetLayersByLevelParams parameters);
}

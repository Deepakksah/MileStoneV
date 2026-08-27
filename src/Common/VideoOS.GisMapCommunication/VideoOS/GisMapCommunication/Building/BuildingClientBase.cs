using System;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Building.Client;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Building;

public abstract class BuildingClientBase : AbstractClient<GisBuildingCommandClient, IGisBuildingCommand>, IBuildingClient, IClientBase, IDisposable
{
	protected sealed override string Name => "GisBuildingService";

	protected BuildingClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	public async Task<AddBuildingResp> AddBuildingAsync(AddBuildingParams parameters)
	{
		return await CallWithRetryAsync<AddBuildingResp>((GisBuildingCommandClient client) => async () => await client.AddBuildingAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<AddBuildingsResp> AddBuildingsAsync(AddBuildingsParams parameters)
	{
		return await CallWithRetryAsync<AddBuildingsResp>((GisBuildingCommandClient client) => async () => await client.AddBuildingsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateBuildingResp> UpdateBuildingAsync(UpdateBuildingParams parameters)
	{
		return await CallWithRetryAsync<UpdateBuildingResp>((GisBuildingCommandClient client) => async () => await client.UpdateBuildingAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateBuildingsResp> UpdateBuildingsAsync(UpdateBuildingsParams parameters)
	{
		return await CallWithRetryAsync<UpdateBuildingsResp>((GisBuildingCommandClient client) => async () => await client.UpdateBuildingsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetBuildingResp> GetBuildingAsync(GetBuildingParams parameters)
	{
		return await CallWithRetryAsync<GetBuildingResp>((GisBuildingCommandClient client) => async () => await client.GetBuildingAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetBuildingsByMapResp> GetBuildingsByMapAsync(GetBuildingsByMapParams parameters)
	{
		return await CallWithRetryAsync<GetBuildingsByMapResp>((GisBuildingCommandClient client) => async () => await client.GetBuildingsByMapAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteBuildingResp> DeleteBuildingAsync(DeleteBuildingParams parameters)
	{
		return await CallWithRetryAsync<DeleteBuildingResp>((GisBuildingCommandClient client) => async () => await client.DeleteBuildingAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteBuildingsResp> DeleteBuildingsAsync(DeleteBuildingsParams parameters)
	{
		return await CallWithRetryAsync<DeleteBuildingsResp>((GisBuildingCommandClient client) => async () => await client.DeleteBuildingsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<AddLevelResp> AddLevelAsync(AddLevelParams parameters)
	{
		return await CallWithRetryAsync<AddLevelResp>((GisBuildingCommandClient client) => async () => await client.AddLevelAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<AddLevelsResp> AddLevelsAsync(AddLevelsParams parameters)
	{
		return await CallWithRetryAsync<AddLevelsResp>((GisBuildingCommandClient client) => async () => await client.AddLevelsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateLevelResp> UpdateLevelAsync(UpdateLevelParams parameters)
	{
		return await CallWithRetryAsync<UpdateLevelResp>((GisBuildingCommandClient client) => async () => await client.UpdateLevelAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateLevelsResp> UpdateLevelsAsync(UpdateLevelsParams parameters)
	{
		return await CallWithRetryAsync<UpdateLevelsResp>((GisBuildingCommandClient client) => async () => await client.UpdateLevelsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetLevelsByBuildingResp> GetLevelsByBuildingAsync(GetLevelsByBuildingParams parameters)
	{
		return await CallWithRetryAsync<GetLevelsByBuildingResp>((GisBuildingCommandClient client) => async () => await client.GetLevelsByBuildingAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteLevelResp> DeleteLevelAsync(DeleteLevelParams parameters)
	{
		return await CallWithRetryAsync<DeleteLevelResp>((GisBuildingCommandClient client) => async () => await client.DeleteLevelAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteLevelsResp> DeleteLevelsAsync(DeleteLevelsParams parameters)
	{
		return await CallWithRetryAsync<DeleteLevelsResp>((GisBuildingCommandClient client) => async () => await client.DeleteLevelsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<ReorderLevelsResp> ReorderLevelsAsync(ReorderLevelsParams parameters)
	{
		return await CallWithRetryAsync<ReorderLevelsResp>((GisBuildingCommandClient client) => async () => await client.ReorderLevelsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetLayersByLevelResp> GetLayersByLevelAsync(GetLayersByLevelParams parameters)
	{
		return await CallWithRetryAsync<GetLayersByLevelResp>((GisBuildingCommandClient client) => async () => await client.GetLayersByLevelAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public AddBuildingResp AddBuilding(AddBuildingParams parameters)
	{
		return CallWithRetry<AddBuildingResp>((GisBuildingCommandClient client) => () => client.AddBuilding(parameters));
	}

	public AddBuildingsResp AddBuildings(AddBuildingsParams parameters)
	{
		return CallWithRetry<AddBuildingsResp>((GisBuildingCommandClient client) => () => client.AddBuildings(parameters));
	}

	public UpdateBuildingResp UpdateBuilding(UpdateBuildingParams parameters)
	{
		return CallWithRetry<UpdateBuildingResp>((GisBuildingCommandClient client) => () => client.UpdateBuilding(parameters));
	}

	public UpdateBuildingsResp UpdateBuildings(UpdateBuildingsParams parameters)
	{
		return CallWithRetry<UpdateBuildingsResp>((GisBuildingCommandClient client) => () => client.UpdateBuildings(parameters));
	}

	public GetBuildingResp GetBuilding(GetBuildingParams parameters)
	{
		return CallWithRetry<GetBuildingResp>((GisBuildingCommandClient client) => () => client.GetBuilding(parameters));
	}

	public GetBuildingsByMapResp GetBuildingsByMap(GetBuildingsByMapParams parameters)
	{
		return CallWithRetry<GetBuildingsByMapResp>((GisBuildingCommandClient client) => () => client.GetBuildingsByMap(parameters));
	}

	public DeleteBuildingResp DeleteBuilding(DeleteBuildingParams parameters)
	{
		return CallWithRetry<DeleteBuildingResp>((GisBuildingCommandClient client) => () => client.DeleteBuilding(parameters));
	}

	public DeleteBuildingsResp DeleteBuildings(DeleteBuildingsParams parameters)
	{
		return CallWithRetry<DeleteBuildingsResp>((GisBuildingCommandClient client) => () => client.DeleteBuildings(parameters));
	}

	public AddLevelResp AddLevel(AddLevelParams parameters)
	{
		return CallWithRetry<AddLevelResp>((GisBuildingCommandClient client) => () => client.AddLevel(parameters));
	}

	public AddLevelsResp AddLevels(AddLevelsParams parameters)
	{
		return CallWithRetry<AddLevelsResp>((GisBuildingCommandClient client) => () => client.AddLevels(parameters));
	}

	public UpdateLevelResp UpdateLevel(UpdateLevelParams parameters)
	{
		return CallWithRetry<UpdateLevelResp>((GisBuildingCommandClient client) => () => client.UpdateLevel(parameters));
	}

	public UpdateLevelsResp UpdateLevels(UpdateLevelsParams parameters)
	{
		return CallWithRetry<UpdateLevelsResp>((GisBuildingCommandClient client) => () => client.UpdateLevels(parameters));
	}

	public GetLevelsByBuildingResp GetLevelsByBuilding(GetLevelsByBuildingParams parameters)
	{
		return CallWithRetry<GetLevelsByBuildingResp>((GisBuildingCommandClient client) => () => client.GetLevelsByBuilding(parameters));
	}

	public DeleteLevelResp DeleteLevel(DeleteLevelParams parameters)
	{
		return CallWithRetry<DeleteLevelResp>((GisBuildingCommandClient client) => () => client.DeleteLevel(parameters));
	}

	public DeleteLevelsResp DeleteLevels(DeleteLevelsParams parameters)
	{
		return CallWithRetry<DeleteLevelsResp>((GisBuildingCommandClient client) => () => client.DeleteLevels(parameters));
	}

	public ReorderLevelsResp ReorderLevels(ReorderLevelsParams parameters)
	{
		return CallWithRetry<ReorderLevelsResp>((GisBuildingCommandClient client) => () => client.ReorderLevels(parameters));
	}

	public GetLayersByLevelResp GetLayersByLevel(GetLayersByLevelParams parameters)
	{
		return CallWithRetry<GetLayersByLevelResp>((GisBuildingCommandClient client) => () => client.GetLayersByLevel(parameters));
	}
}

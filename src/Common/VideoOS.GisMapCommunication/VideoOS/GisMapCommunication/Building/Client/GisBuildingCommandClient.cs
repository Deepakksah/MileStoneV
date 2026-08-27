using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Building.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisBuildingCommandClient : ClientBase<IGisBuildingCommand>, IGisBuildingCommand
{
	public GisBuildingCommandClient()
	{
	}

	public GisBuildingCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisBuildingCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisBuildingCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisBuildingCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public AddBuildingResp AddBuilding(AddBuildingParams parameters)
	{
		return base.Channel.AddBuilding(parameters);
	}

	public Task<AddBuildingResp> AddBuildingAsync(AddBuildingParams parameters)
	{
		return base.Channel.AddBuildingAsync(parameters);
	}

	public AddBuildingsResp AddBuildings(AddBuildingsParams parameters)
	{
		return base.Channel.AddBuildings(parameters);
	}

	public Task<AddBuildingsResp> AddBuildingsAsync(AddBuildingsParams parameters)
	{
		return base.Channel.AddBuildingsAsync(parameters);
	}

	public UpdateBuildingResp UpdateBuilding(UpdateBuildingParams parameters)
	{
		return base.Channel.UpdateBuilding(parameters);
	}

	public Task<UpdateBuildingResp> UpdateBuildingAsync(UpdateBuildingParams parameters)
	{
		return base.Channel.UpdateBuildingAsync(parameters);
	}

	public UpdateBuildingsResp UpdateBuildings(UpdateBuildingsParams parameters)
	{
		return base.Channel.UpdateBuildings(parameters);
	}

	public Task<UpdateBuildingsResp> UpdateBuildingsAsync(UpdateBuildingsParams parameters)
	{
		return base.Channel.UpdateBuildingsAsync(parameters);
	}

	public GetBuildingResp GetBuilding(GetBuildingParams parameters)
	{
		return base.Channel.GetBuilding(parameters);
	}

	public Task<GetBuildingResp> GetBuildingAsync(GetBuildingParams parameters)
	{
		return base.Channel.GetBuildingAsync(parameters);
	}

	public GetBuildingsByMapResp GetBuildingsByMap(GetBuildingsByMapParams parameters)
	{
		return base.Channel.GetBuildingsByMap(parameters);
	}

	public Task<GetBuildingsByMapResp> GetBuildingsByMapAsync(GetBuildingsByMapParams parameters)
	{
		return base.Channel.GetBuildingsByMapAsync(parameters);
	}

	public DeleteBuildingResp DeleteBuilding(DeleteBuildingParams parameters)
	{
		return base.Channel.DeleteBuilding(parameters);
	}

	public Task<DeleteBuildingResp> DeleteBuildingAsync(DeleteBuildingParams parameters)
	{
		return base.Channel.DeleteBuildingAsync(parameters);
	}

	public DeleteBuildingsResp DeleteBuildings(DeleteBuildingsParams parameters)
	{
		return base.Channel.DeleteBuildings(parameters);
	}

	public Task<DeleteBuildingsResp> DeleteBuildingsAsync(DeleteBuildingsParams parameters)
	{
		return base.Channel.DeleteBuildingsAsync(parameters);
	}

	public AddLevelResp AddLevel(AddLevelParams parameters)
	{
		return base.Channel.AddLevel(parameters);
	}

	public Task<AddLevelResp> AddLevelAsync(AddLevelParams parameters)
	{
		return base.Channel.AddLevelAsync(parameters);
	}

	public AddLevelsResp AddLevels(AddLevelsParams parameters)
	{
		return base.Channel.AddLevels(parameters);
	}

	public Task<AddLevelsResp> AddLevelsAsync(AddLevelsParams parameters)
	{
		return base.Channel.AddLevelsAsync(parameters);
	}

	public UpdateLevelResp UpdateLevel(UpdateLevelParams parameters)
	{
		return base.Channel.UpdateLevel(parameters);
	}

	public Task<UpdateLevelResp> UpdateLevelAsync(UpdateLevelParams parameters)
	{
		return base.Channel.UpdateLevelAsync(parameters);
	}

	public UpdateLevelsResp UpdateLevels(UpdateLevelsParams parameters)
	{
		return base.Channel.UpdateLevels(parameters);
	}

	public Task<UpdateLevelsResp> UpdateLevelsAsync(UpdateLevelsParams parameters)
	{
		return base.Channel.UpdateLevelsAsync(parameters);
	}

	public GetLevelsByBuildingResp GetLevelsByBuilding(GetLevelsByBuildingParams parameters)
	{
		return base.Channel.GetLevelsByBuilding(parameters);
	}

	public Task<GetLevelsByBuildingResp> GetLevelsByBuildingAsync(GetLevelsByBuildingParams parameters)
	{
		return base.Channel.GetLevelsByBuildingAsync(parameters);
	}

	public DeleteLevelResp DeleteLevel(DeleteLevelParams parameters)
	{
		return base.Channel.DeleteLevel(parameters);
	}

	public Task<DeleteLevelResp> DeleteLevelAsync(DeleteLevelParams parameters)
	{
		return base.Channel.DeleteLevelAsync(parameters);
	}

	public DeleteLevelsResp DeleteLevels(DeleteLevelsParams parameters)
	{
		return base.Channel.DeleteLevels(parameters);
	}

	public Task<DeleteLevelsResp> DeleteLevelsAsync(DeleteLevelsParams parameters)
	{
		return base.Channel.DeleteLevelsAsync(parameters);
	}

	public ReorderLevelsResp ReorderLevels(ReorderLevelsParams parameters)
	{
		return base.Channel.ReorderLevels(parameters);
	}

	public Task<ReorderLevelsResp> ReorderLevelsAsync(ReorderLevelsParams parameters)
	{
		return base.Channel.ReorderLevelsAsync(parameters);
	}

	public GetLayersByLevelResp GetLayersByLevel(GetLayersByLevelParams parameters)
	{
		return base.Channel.GetLayersByLevel(parameters);
	}

	public Task<GetLayersByLevelResp> GetLayersByLevelAsync(GetLayersByLevelParams parameters)
	{
		return base.Channel.GetLayersByLevelAsync(parameters);
	}
}

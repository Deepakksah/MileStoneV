using System;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Layer;

public interface ILayerClient : IClientBase, IDisposable
{
	Task<AddLayerResp> AddLayerAsync(AddLayerParams parameters);

	Task<AddLayersResp> AddLayersAsync(AddLayersParams parameters);

	Task<UpdateLayerResp> UpdateLayerAsync(UpdateLayerParams parameters);

	Task<GetLayerResp> GetLayerAsync(GetLayerParams parameters);

	Task<GetLayersByMapResp> GetLayersByMapAsync(GetLayersByMapParams parameters);

	Task<DeleteLayerResp> DeleteLayerAsync(DeleteLayerParams parameters);

	Task<DeleteLayersResp> DeleteLayersAsync(DeleteLayersParams parameters);

	Task<ReorderLayersResp> ReorderLayersAsync(ReorderLayersParams parameters);

	AddLayerResp AddLayer(AddLayerParams parameters);

	AddLayersResp AddLayers(AddLayersParams parameters);

	UpdateLayerResp UpdateLayer(UpdateLayerParams parameters);

	GetLayerResp GetLayer(GetLayerParams parameters);

	GetLayersByMapResp GetLayersByMap(GetLayersByMapParams parameters);

	DeleteLayerResp DeleteLayer(DeleteLayerParams parameters);

	DeleteLayersResp DeleteLayers(DeleteLayersParams parameters);

	ReorderLayersResp ReorderLayers(ReorderLayersParams parameters);
}

using System;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Layer.Client;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Layer;

public abstract class LayerClientBase : AbstractClient<GisLayerCommandClient, IGisLayerCommand>, ILayerClient, IClientBase, IDisposable
{
	protected override string Name => "GisLayerService";

	protected LayerClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public async Task<AddLayerResp> AddLayerAsync(AddLayerParams parameters)
	{
		return await CallWithRetryAsync<AddLayerResp>((GisLayerCommandClient client) => async () => await client.AddLayerAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<AddLayersResp> AddLayersAsync(AddLayersParams parameters)
	{
		return await CallWithRetryAsync<AddLayersResp>((GisLayerCommandClient client) => async () => await client.AddLayersAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateLayerResp> UpdateLayerAsync(UpdateLayerParams parameters)
	{
		return await CallWithRetryAsync<UpdateLayerResp>((GisLayerCommandClient client) => async () => await client.UpdateLayerAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetLayersByMapResp> GetLayersByMapAsync(GetLayersByMapParams parameters)
	{
		return await CallWithRetryAsync<GetLayersByMapResp>((GisLayerCommandClient client) => async () => await client.GetLayersByMapAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetLayerResp> GetLayerAsync(GetLayerParams parameters)
	{
		return await CallWithRetryAsync<GetLayerResp>((GisLayerCommandClient client) => async () => await client.GetLayerAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteLayerResp> DeleteLayerAsync(DeleteLayerParams parameters)
	{
		return await CallWithRetryAsync<DeleteLayerResp>((GisLayerCommandClient client) => async () => await client.DeleteLayerAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteLayersResp> DeleteLayersAsync(DeleteLayersParams parameters)
	{
		return await CallWithRetryAsync<DeleteLayersResp>((GisLayerCommandClient client) => async () => await client.DeleteLayersAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<ReorderLayersResp> ReorderLayersAsync(ReorderLayersParams parameters)
	{
		return await CallWithRetryAsync<ReorderLayersResp>((GisLayerCommandClient client) => async () => await client.ReorderLayersAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public AddLayerResp AddLayer(AddLayerParams parameters)
	{
		return CallWithRetry<AddLayerResp>((GisLayerCommandClient client) => () => client.AddLayer(parameters));
	}

	public AddLayersResp AddLayers(AddLayersParams parameters)
	{
		return CallWithRetry<AddLayersResp>((GisLayerCommandClient client) => () => client.AddLayers(parameters));
	}

	public UpdateLayerResp UpdateLayer(UpdateLayerParams parameters)
	{
		return CallWithRetry<UpdateLayerResp>((GisLayerCommandClient client) => () => client.UpdateLayer(parameters));
	}

	public GetLayerResp GetLayer(GetLayerParams parameters)
	{
		return CallWithRetry<GetLayerResp>((GisLayerCommandClient client) => () => client.GetLayer(parameters));
	}

	public GetLayersByMapResp GetLayersByMap(GetLayersByMapParams parameters)
	{
		return CallWithRetry<GetLayersByMapResp>((GisLayerCommandClient client) => () => client.GetLayersByMap(parameters));
	}

	public DeleteLayerResp DeleteLayer(DeleteLayerParams parameters)
	{
		return CallWithRetry<DeleteLayerResp>((GisLayerCommandClient client) => () => client.DeleteLayer(parameters));
	}

	public DeleteLayersResp DeleteLayers(DeleteLayersParams parameters)
	{
		return CallWithRetry<DeleteLayersResp>((GisLayerCommandClient client) => () => client.DeleteLayers(parameters));
	}

	public ReorderLayersResp ReorderLayers(ReorderLayersParams parameters)
	{
		return CallWithRetry<ReorderLayersResp>((GisLayerCommandClient client) => () => client.ReorderLayers(parameters));
	}
}

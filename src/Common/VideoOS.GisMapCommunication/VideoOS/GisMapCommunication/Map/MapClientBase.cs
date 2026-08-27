using System;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Map.Client;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Map;

public abstract class MapClientBase : AbstractClient<GisMapCommandClient, IGisMapCommand>, IMapClient, IClientBase, IDisposable
{
	protected override string Name => "GisMapService";

	protected MapClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public async Task<AddMapResp> AddMapAsync(AddMapParams parameters)
	{
		return await CallWithRetryAsync<AddMapResp>((GisMapCommandClient client) => async () => await client.AddMapAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateMapResp> UpdateMapAsync(UpdateMapParams parameters)
	{
		return await CallWithRetryAsync<UpdateMapResp>((GisMapCommandClient client) => async () => await client.UpdateMapAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<BrowseMapsResp> BrowseMapsAsync(BrowseMapsParams parameters)
	{
		return await CallWithRetryAsync<BrowseMapsResp>((GisMapCommandClient client) => async () => await client.BrowseMapsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetMapResp> GetMapAsync(GetMapParams parameters)
	{
		return await CallWithRetryAsync<GetMapResp>((GisMapCommandClient client) => async () => await client.GetMapAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteMapResp> DeleteMapAsync(DeleteMapParams parameters)
	{
		return await CallWithRetryAsync<DeleteMapResp>((GisMapCommandClient client) => async () => await client.DeleteMapAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public AddMapResp AddMap(AddMapParams parameters)
	{
		return CallWithRetry<AddMapResp>((GisMapCommandClient client) => () => client.AddMap(parameters));
	}

	public UpdateMapResp UpdateMap(UpdateMapParams parameters)
	{
		return CallWithRetry<UpdateMapResp>((GisMapCommandClient client) => () => client.UpdateMap(parameters));
	}

	public BrowseMapsResp BrowseMaps(BrowseMapsParams parameters)
	{
		return CallWithRetry<BrowseMapsResp>((GisMapCommandClient client) => () => client.BrowseMaps(parameters));
	}

	public GetMapResp GetMap(GetMapParams parameters)
	{
		return CallWithRetry<GetMapResp>((GisMapCommandClient client) => () => client.GetMap(parameters));
	}

	public DeleteMapResp DeleteMap(DeleteMapParams parameters)
	{
		return CallWithRetry<DeleteMapResp>((GisMapCommandClient client) => () => client.DeleteMap(parameters));
	}
}

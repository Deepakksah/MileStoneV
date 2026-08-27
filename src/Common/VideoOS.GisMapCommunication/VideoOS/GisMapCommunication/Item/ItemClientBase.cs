using System;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Item.Client;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Item;

public abstract class ItemClientBase : AbstractClient<GisItemCommandClient, IGisItemCommand>, IItemClient, IClientBase, IDisposable
{
	protected override string Name => "GisItemService";

	protected ItemClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public async Task<AddItemResp> AddItemAsync(AddItemParams parameters)
	{
		return await CallWithRetryAsync<AddItemResp>((GisItemCommandClient client) => async () => await client.AddItemAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<AddItemsResp> AddItemsAsync(AddItemsParams parameters)
	{
		return await CallWithRetryAsync<AddItemsResp>((GisItemCommandClient client) => async () => await client.AddItemsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateItemResp> UpdateItemAsync(UpdateItemParams parameters)
	{
		return await CallWithRetryAsync<UpdateItemResp>((GisItemCommandClient client) => async () => await client.UpdateItemAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateItemsResp> UpdateItemsAsync(UpdateItemsParams parameters)
	{
		return await CallWithRetryAsync<UpdateItemsResp>((GisItemCommandClient client) => async () => await client.UpdateItemsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetItemResp> GetItemAsync(GetItemParams parameters)
	{
		return await CallWithRetryAsync<GetItemResp>((GisItemCommandClient client) => async () => await client.GetItemAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetItemsByLayerResp> GetItemsByLayerAsync(GetItemsByLayerParams parameters)
	{
		return await CallWithRetryAsync<GetItemsByLayerResp>((GisItemCommandClient client) => async () => await client.GetItemsByLayerAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteItemResp> DeleteItemAsync(DeleteItemParams parameters)
	{
		return await CallWithRetryAsync<DeleteItemResp>((GisItemCommandClient client) => async () => await client.DeleteItemAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<DeleteItemsResp> DeleteItemsAsync(DeleteItemsParams parameters)
	{
		return await CallWithRetryAsync<DeleteItemsResp>((GisItemCommandClient client) => async () => await client.DeleteItemsAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public AddItemResp AddItem(AddItemParams parameters)
	{
		return CallWithRetry<AddItemResp>((GisItemCommandClient client) => () => client.AddItem(parameters));
	}

	public AddItemsResp AddItems(AddItemsParams parameters)
	{
		return CallWithRetry<AddItemsResp>((GisItemCommandClient client) => () => client.AddItems(parameters));
	}

	public UpdateItemResp UpdateItem(UpdateItemParams parameters)
	{
		return CallWithRetry<UpdateItemResp>((GisItemCommandClient client) => () => client.UpdateItem(parameters));
	}

	public UpdateItemsResp UpdateItems(UpdateItemsParams parameters)
	{
		return CallWithRetry<UpdateItemsResp>((GisItemCommandClient client) => () => client.UpdateItems(parameters));
	}

	public GetItemResp GetItem(GetItemParams parameters)
	{
		return CallWithRetry<GetItemResp>((GisItemCommandClient client) => () => client.GetItem(parameters));
	}

	public GetItemsByLayerResp GetItemsByLayer(GetItemsByLayerParams parameters)
	{
		return CallWithRetry<GetItemsByLayerResp>((GisItemCommandClient client) => () => client.GetItemsByLayer(parameters));
	}

	public DeleteItemResp DeleteItem(DeleteItemParams parameters)
	{
		return CallWithRetry<DeleteItemResp>((GisItemCommandClient client) => () => client.DeleteItem(parameters));
	}

	public DeleteItemsResp DeleteItems(DeleteItemsParams parameters)
	{
		return CallWithRetry<DeleteItemsResp>((GisItemCommandClient client) => () => client.DeleteItems(parameters));
	}
}

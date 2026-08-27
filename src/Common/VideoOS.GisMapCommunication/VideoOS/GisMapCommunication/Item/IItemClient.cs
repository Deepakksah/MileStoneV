using System;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Item;

public interface IItemClient : IClientBase, IDisposable
{
	Task<AddItemResp> AddItemAsync(AddItemParams parameters);

	Task<AddItemsResp> AddItemsAsync(AddItemsParams parameters);

	Task<UpdateItemResp> UpdateItemAsync(UpdateItemParams parameters);

	Task<UpdateItemsResp> UpdateItemsAsync(UpdateItemsParams parameters);

	Task<GetItemResp> GetItemAsync(GetItemParams parameters);

	Task<GetItemsByLayerResp> GetItemsByLayerAsync(GetItemsByLayerParams parameters);

	Task<DeleteItemResp> DeleteItemAsync(DeleteItemParams parameters);

	Task<DeleteItemsResp> DeleteItemsAsync(DeleteItemsParams parameters);

	AddItemResp AddItem(AddItemParams parameters);

	AddItemsResp AddItems(AddItemsParams parameters);

	UpdateItemResp UpdateItem(UpdateItemParams parameters);

	UpdateItemsResp UpdateItems(UpdateItemsParams parameters);

	GetItemResp GetItem(GetItemParams parameters);

	GetItemsByLayerResp GetItemsByLayer(GetItemsByLayerParams parameters);

	DeleteItemResp DeleteItem(DeleteItemParams parameters);

	DeleteItemsResp DeleteItems(DeleteItemsParams parameters);
}

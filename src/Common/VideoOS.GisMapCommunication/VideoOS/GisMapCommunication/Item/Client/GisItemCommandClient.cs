using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Item.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisItemCommandClient : ClientBase<IGisItemCommand>, IGisItemCommand
{
	public GisItemCommandClient()
	{
	}

	public GisItemCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisItemCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisItemCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisItemCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public AddItemResp AddItem(AddItemParams parameters)
	{
		return base.Channel.AddItem(parameters);
	}

	public Task<AddItemResp> AddItemAsync(AddItemParams parameters)
	{
		return base.Channel.AddItemAsync(parameters);
	}

	public AddItemsResp AddItems(AddItemsParams parameters)
	{
		return base.Channel.AddItems(parameters);
	}

	public Task<AddItemsResp> AddItemsAsync(AddItemsParams parameters)
	{
		return base.Channel.AddItemsAsync(parameters);
	}

	public UpdateItemResp UpdateItem(UpdateItemParams parameters)
	{
		return base.Channel.UpdateItem(parameters);
	}

	public Task<UpdateItemResp> UpdateItemAsync(UpdateItemParams parameters)
	{
		return base.Channel.UpdateItemAsync(parameters);
	}

	public UpdateItemsResp UpdateItems(UpdateItemsParams parameters)
	{
		return base.Channel.UpdateItems(parameters);
	}

	public Task<UpdateItemsResp> UpdateItemsAsync(UpdateItemsParams parameters)
	{
		return base.Channel.UpdateItemsAsync(parameters);
	}

	public GetItemResp GetItem(GetItemParams parameters)
	{
		return base.Channel.GetItem(parameters);
	}

	public Task<GetItemResp> GetItemAsync(GetItemParams parameters)
	{
		return base.Channel.GetItemAsync(parameters);
	}

	public GetItemsByLayerResp GetItemsByLayer(GetItemsByLayerParams parameters)
	{
		return base.Channel.GetItemsByLayer(parameters);
	}

	public Task<GetItemsByLayerResp> GetItemsByLayerAsync(GetItemsByLayerParams parameters)
	{
		return base.Channel.GetItemsByLayerAsync(parameters);
	}

	public DeleteItemResp DeleteItem(DeleteItemParams parameters)
	{
		return base.Channel.DeleteItem(parameters);
	}

	public Task<DeleteItemResp> DeleteItemAsync(DeleteItemParams parameters)
	{
		return base.Channel.DeleteItemAsync(parameters);
	}

	public DeleteItemsResp DeleteItems(DeleteItemsParams parameters)
	{
		return base.Channel.DeleteItems(parameters);
	}

	public Task<DeleteItemsResp> DeleteItemsAsync(DeleteItemsParams parameters)
	{
		return base.Channel.DeleteItemsAsync(parameters);
	}
}

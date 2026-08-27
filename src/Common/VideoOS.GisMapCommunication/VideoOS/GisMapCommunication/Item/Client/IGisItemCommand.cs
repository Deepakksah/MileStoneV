using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Item.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisItemCommand", ConfigurationName = "VideoOS.GisMapCommunication.Item.Client.IGisItemCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisItemCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItemResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItemGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddItemResp AddItem(AddItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItemResponse")]
	Task<AddItemResp> AddItemAsync(AddItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItems", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItemsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItemsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddItemsResp AddItems(AddItemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItems", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/AddItemsResponse")]
	Task<AddItemsResp> AddItemsAsync(AddItemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItemResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItemGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateItemResp UpdateItem(UpdateItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItemResponse")]
	Task<UpdateItemResp> UpdateItemAsync(UpdateItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItems", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItemsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItemsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateItemsResp UpdateItems(UpdateItemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItems", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/UpdateItemsResponse")]
	Task<UpdateItemsResp> UpdateItemsAsync(UpdateItemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetItemResp GetItem(GetItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemResponse")]
	Task<GetItemResp> GetItemAsync(GetItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemsByLayer", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemsByLayerResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemsByLayerGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetItemsByLayerResp GetItemsByLayer(GetItemsByLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemsByLayer", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/GetItemsByLayerResponse")]
	Task<GetItemsByLayerResp> GetItemsByLayerAsync(GetItemsByLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItemResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItemGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteItemResp DeleteItem(DeleteItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItem", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItemResponse")]
	Task<DeleteItemResp> DeleteItemAsync(DeleteItemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItems", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItemsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItemsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteItemsResp DeleteItems(DeleteItemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItems", ReplyAction = "http://videoos.net/2/GisItemCommand/IGisItemCommand/DeleteItemsResponse")]
	Task<DeleteItemsResp> DeleteItemsAsync(DeleteItemsParams parameters);
}

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Layer.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisLayerCommand", ConfigurationName = "VideoOS.GisMapCommunication.Layer.Client.IGisLayerCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisLayerCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayerResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayerGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddLayerResp AddLayer(AddLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayerResponse")]
	Task<AddLayerResp> AddLayerAsync(AddLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayers", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayersResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayersGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddLayersResp AddLayers(AddLayersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayers", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/AddLayersResponse")]
	Task<AddLayersResp> AddLayersAsync(AddLayersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/UpdateLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/UpdateLayerResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/UpdateLayerGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateLayerResp UpdateLayer(UpdateLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/UpdateLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/UpdateLayerResponse")]
	Task<UpdateLayerResp> UpdateLayerAsync(UpdateLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayerResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayerGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetLayerResp GetLayer(GetLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayerResponse")]
	Task<GetLayerResp> GetLayerAsync(GetLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayersByMap", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayersByMapResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayersByMapGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetLayersByMapResp GetLayersByMap(GetLayersByMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayersByMap", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/GetLayersByMapResponse")]
	Task<GetLayersByMapResp> GetLayersByMapAsync(GetLayersByMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayerResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayerGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteLayerResp DeleteLayer(DeleteLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayer", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayerResponse")]
	Task<DeleteLayerResp> DeleteLayerAsync(DeleteLayerParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayers", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayersResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayersGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	DeleteLayersResp DeleteLayers(DeleteLayersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayers", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/DeleteLayersResponse")]
	Task<DeleteLayersResp> DeleteLayersAsync(DeleteLayersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/ReorderLayers", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/ReorderLayersResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/ReorderLayersGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	ReorderLayersResp ReorderLayers(ReorderLayersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/ReorderLayers", ReplyAction = "http://videoos.net/2/GisLayerCommand/IGisLayerCommand/ReorderLayersResponse")]
	Task<ReorderLayersResp> ReorderLayersAsync(ReorderLayersParams parameters);
}

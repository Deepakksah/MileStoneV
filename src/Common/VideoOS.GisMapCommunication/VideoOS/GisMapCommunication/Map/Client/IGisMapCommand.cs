using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Map.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisMapCommand", ConfigurationName = "VideoOS.GisMapCommunication.Map.Client.IGisMapCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisMapCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/AddMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/AddMapResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/AddMapGisMapServiceFaultFault", Name = "GisMapServiceFault")]
	AddMapResp AddMap(AddMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/AddMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/AddMapResponse")]
	Task<AddMapResp> AddMapAsync(AddMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/UpdateMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/UpdateMapResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/UpdateMapGisMapServiceFaultFault", Name = "GisMapServiceFault")]
	UpdateMapResp UpdateMap(UpdateMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/UpdateMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/UpdateMapResponse")]
	Task<UpdateMapResp> UpdateMapAsync(UpdateMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/BrowseMaps", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/BrowseMapsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/BrowseMapsGisMapServiceFaultFault", Name = "GisMapServiceFault")]
	BrowseMapsResp BrowseMaps(BrowseMapsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/BrowseMaps", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/BrowseMapsResponse")]
	Task<BrowseMapsResp> BrowseMapsAsync(BrowseMapsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/GetMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/GetMapResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/GetMapGisMapServiceFaultFault", Name = "GisMapServiceFault")]
	GetMapResp GetMap(GetMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/GetMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/GetMapResponse")]
	Task<GetMapResp> GetMapAsync(GetMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/DeleteMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/DeleteMapResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/DeleteMapGisMapServiceFaultFault", Name = "GisMapServiceFault")]
	DeleteMapResp DeleteMap(DeleteMapParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMapCommand/IGisMapCommand/DeleteMap", ReplyAction = "http://videoos.net/2/GisMapCommand/IGisMapCommand/DeleteMapResponse")]
	Task<DeleteMapResp> DeleteMapAsync(DeleteMapParams parameters);
}

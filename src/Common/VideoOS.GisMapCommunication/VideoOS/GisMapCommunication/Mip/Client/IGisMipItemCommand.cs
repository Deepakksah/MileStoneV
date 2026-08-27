using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Mip.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisMipItemCommand", ConfigurationName = "VideoOS.GisMapCommunication.Mip.Client.IGisMipItemCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisMipItemCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindAndArea", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindAndAreaResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindAndAreaGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetMipItemsByKindAndAreaResp GetMipItemsByKindAndArea(GetMipItemsByKindAndAreaParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindAndArea", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindAndAreaResponse")]
	Task<GetMipItemsByKindAndAreaResp> GetMipItemsByKindAndAreaAsync(GetMipItemsByKindAndAreaParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKind", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetMipItemsByKindResp GetMipItemsByKind(GetMipItemsByKindParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKind", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemsByKindResponse")]
	Task<GetMipItemsByKindResp> GetMipItemsByKindAsync(GetMipItemsByKindParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipPlugins", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipPluginsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipPluginsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetMipPluginsResp GetMipPlugins(GetMipPluginsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipPlugins", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipPluginsResponse")]
	Task<GetMipPluginsResp> GetMipPluginsAsync(GetMipPluginsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemMenuActions", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemMenuActionsResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemMenuActionsGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetMipItemMenuActionsResp GetMipItemMenuActions(GetMipItemMenuActionsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemMenuActions", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/GetMipItemMenuActionsResponse")]
	Task<GetMipItemMenuActionsResp> GetMipItemMenuActionsAsync(GetMipItemMenuActionsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/ActivatePluginAction", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/ActivatePluginActionResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/ActivatePluginActionGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	ActivatePluginActionResp ActivatePluginAction(ActivatePluginActionParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/ActivatePluginAction", ReplyAction = "http://videoos.net/2/GisMipItemCommand/IGisMipItemCommand/ActivatePluginActionResponse")]
	Task<ActivatePluginActionResp> ActivatePluginActionAsync(ActivatePluginActionParams parameters);
}

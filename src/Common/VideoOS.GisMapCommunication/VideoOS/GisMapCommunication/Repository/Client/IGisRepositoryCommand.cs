using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Repository.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisRepositoryCommand", ConfigurationName = "VideoOS.GisMapCommunication.Repository.Client.IGisRepositoryCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisRepositoryCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/GetRepository", ReplyAction = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/GetRepositoryResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/GetRepositoryGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	GetRepositoryResp GetRepository(GetRepositoryParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/GetRepository", ReplyAction = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/GetRepositoryResponse")]
	Task<GetRepositoryResp> GetRepositoryAsync(GetRepositoryParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/AddRepository", ReplyAction = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/AddRepositoryResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/AddRepositoryGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	AddRepositoryResp AddRepository(AddRepositoryParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/AddRepository", ReplyAction = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/AddRepositoryResponse")]
	Task<AddRepositoryResp> AddRepositoryAsync(AddRepositoryParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/UpdateRepository", ReplyAction = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/UpdateRepositoryResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/UpdateRepositoryGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	UpdateRepositoryResp UpdateRepository(UpdateRepositoryParams parameters);

	[OperationContract(Action = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/UpdateRepository", ReplyAction = "http://videoos.net/2/GisRepositoryCommand/IGisRepositoryCommand/UpdateRepositoryResponse")]
	Task<UpdateRepositoryResp> UpdateRepositoryAsync(UpdateRepositoryParams parameters);
}

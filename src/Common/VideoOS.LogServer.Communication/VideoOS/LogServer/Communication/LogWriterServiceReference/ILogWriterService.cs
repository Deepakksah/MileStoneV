using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LogWriterServiceReference.ILogWriterService")]
public interface ILogWriterService
{
	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterSourceType", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterSourceTypeResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterService/RegisterSourceTypeUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterSourceTypeResp RegisterSourceType(RegisterSourceTypeParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterSourceType", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterSourceTypeResponse")]
	Task<RegisterSourceTypeResp> RegisterSourceTypeAsync(RegisterSourceTypeParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterCategory", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterCategoryResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterService/RegisterCategoryUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterCategoryResp RegisterCategory(RegisterCategoryParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterCategory", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterCategoryResponse")]
	Task<RegisterCategoryResp> RegisterCategoryAsync(RegisterCategoryParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterLogMessages", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterLogMessagesResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterService/RegisterLogMessagesUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterLogMessagesResp RegisterLogMessages(RegisterLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterLogMessages", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterLogMessagesResponse")]
	Task<RegisterLogMessagesResp> RegisterLogMessagesAsync(RegisterLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterLegacyLogMessages", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterLegacyLogMessagesResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterService/RegisterLegacyLogMessagesUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterLogMessagesResp RegisterLegacyLogMessages(RegisterLegacyLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/RegisterLegacyLogMessages", ReplyAction = "http://tempuri.org/ILogWriterService/RegisterLegacyLogMessagesResponse")]
	Task<RegisterLogMessagesResp> RegisterLegacyLogMessagesAsync(RegisterLegacyLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/WriteLogline", ReplyAction = "http://tempuri.org/ILogWriterService/WriteLoglineResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterService/WriteLoglineUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	WriteLoglineResp WriteLogline(WriteLoglineParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/WriteLogline", ReplyAction = "http://tempuri.org/ILogWriterService/WriteLoglineResponse")]
	Task<WriteLoglineResp> WriteLoglineAsync(WriteLoglineParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/WriteLegacyLogline", ReplyAction = "http://tempuri.org/ILogWriterService/WriteLegacyLoglineResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterService/WriteLegacyLoglineUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	WriteLoglineResp WriteLegacyLogline(WriteLegacyLoglineParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterService/WriteLegacyLogline", ReplyAction = "http://tempuri.org/ILogWriterService/WriteLegacyLoglineResponse")]
	Task<WriteLoglineResp> WriteLegacyLoglineAsync(WriteLegacyLoglineParams parameters);
}

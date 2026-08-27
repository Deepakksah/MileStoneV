using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LogWriterServiceReference.ILogWriterTokenService")]
public interface ILogWriterTokenService
{
	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterSourceTypeToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterSourceTypeTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterTokenService/RegisterSourceTypeTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterSourceTypeResp RegisterSourceTypeToken(string token, RegisterSourceTypeParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterSourceTypeToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterSourceTypeTokenResponse")]
	Task<RegisterSourceTypeResp> RegisterSourceTypeTokenAsync(string token, RegisterSourceTypeParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterCategoryToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterCategoryTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterTokenService/RegisterCategoryTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterCategoryResp RegisterCategoryToken(string token, RegisterCategoryParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterCategoryToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterCategoryTokenResponse")]
	Task<RegisterCategoryResp> RegisterCategoryTokenAsync(string token, RegisterCategoryParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterLogMessagesToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterLogMessagesTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterTokenService/RegisterLogMessagesTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterLogMessagesResp RegisterLogMessagesToken(string token, RegisterLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterLogMessagesToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterLogMessagesTokenResponse")]
	Task<RegisterLogMessagesResp> RegisterLogMessagesTokenAsync(string token, RegisterLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterLegacyLogMessagesToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterLegacyLogMessagesTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterTokenService/RegisterLegacyLogMessagesTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	RegisterLogMessagesResp RegisterLegacyLogMessagesToken(string token, RegisterLegacyLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/RegisterLegacyLogMessagesToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/RegisterLegacyLogMessagesTokenResponse")]
	Task<RegisterLogMessagesResp> RegisterLegacyLogMessagesTokenAsync(string token, RegisterLegacyLogMessagesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/WriteLoglineToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/WriteLoglineTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterTokenService/WriteLoglineTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	WriteLoglineResp WriteLoglineToken(string token, WriteLoglineParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/WriteLoglineToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/WriteLoglineTokenResponse")]
	Task<WriteLoglineResp> WriteLoglineTokenAsync(string token, WriteLoglineParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/WriteLegacyLoglineToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/WriteLegacyLoglineTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogWriterTokenService/WriteLegacyLoglineTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	WriteLoglineResp WriteLegacyLoglineToken(string token, WriteLegacyLoglineParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogWriterTokenService/WriteLegacyLoglineToken", ReplyAction = "http://tempuri.org/ILogWriterTokenService/WriteLegacyLoglineTokenResponse")]
	Task<WriteLoglineResp> WriteLegacyLoglineTokenAsync(string token, WriteLegacyLoglineParams parameters);
}

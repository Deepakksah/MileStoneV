using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogConfigurationServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LogConfigurationServiceReference.ILogConfigurationTokenService")]
public interface ILogConfigurationTokenService
{
	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/GetLogRetentionsToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/GetLogRetentionsTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationTokenService/GetLogRetentionsTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetLogRetentionsResp GetLogRetentionsToken(string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/GetLogRetentionsToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/GetLogRetentionsTokenResponse")]
	Task<GetLogRetentionsResp> GetLogRetentionsTokenAsync(string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/GetAnonymousLoggingToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/GetAnonymousLoggingTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationTokenService/GetAnonymousLoggingTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetAnonymousLoggingResp GetAnonymousLoggingToken(string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/GetAnonymousLoggingToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/GetAnonymousLoggingTokenResponse")]
	Task<GetAnonymousLoggingResp> GetAnonymousLoggingTokenAsync(string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/UpdateLogRetentionsToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/UpdateLogRetentionsTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationTokenService/UpdateLogRetentionsTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	UpdateLogRetentionsResp UpdateLogRetentionsToken(UpdateLogRetentionsParams parameters, string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/UpdateLogRetentionsToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/UpdateLogRetentionsTokenResponse")]
	Task<UpdateLogRetentionsResp> UpdateLogRetentionsTokenAsync(UpdateLogRetentionsParams parameters, string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/UpdateAnonymousLoggingToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/UpdateAnonymousLoggingTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationTokenService/UpdateAnonymousLoggingTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	UpdateAnonymousLoggingResp UpdateAnonymousLoggingToken(UpdateAnonymousLoggingParams parameters, string token);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationTokenService/UpdateAnonymousLoggingToken", ReplyAction = "http://tempuri.org/ILogConfigurationTokenService/UpdateAnonymousLoggingTokenResponse")]
	Task<UpdateAnonymousLoggingResp> UpdateAnonymousLoggingTokenAsync(UpdateAnonymousLoggingParams parameters, string token);
}

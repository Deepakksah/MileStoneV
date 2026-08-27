using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogConfigurationServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LogConfigurationServiceReference.ILogConfigurationService")]
public interface ILogConfigurationService
{
	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/GetLogRetentions", ReplyAction = "http://tempuri.org/ILogConfigurationService/GetLogRetentionsResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationService/GetLogRetentionsUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetLogRetentionsResp GetLogRetentions();

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/GetLogRetentions", ReplyAction = "http://tempuri.org/ILogConfigurationService/GetLogRetentionsResponse")]
	Task<GetLogRetentionsResp> GetLogRetentionsAsync();

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/GetAnonymousLogging", ReplyAction = "http://tempuri.org/ILogConfigurationService/GetAnonymousLoggingResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationService/GetAnonymousLoggingUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetAnonymousLoggingResp GetAnonymousLogging();

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/GetAnonymousLogging", ReplyAction = "http://tempuri.org/ILogConfigurationService/GetAnonymousLoggingResponse")]
	Task<GetAnonymousLoggingResp> GetAnonymousLoggingAsync();

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/UpdateLogRetentions", ReplyAction = "http://tempuri.org/ILogConfigurationService/UpdateLogRetentionsResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationService/UpdateLogRetentionsUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	UpdateLogRetentionsResp UpdateLogRetentions(UpdateLogRetentionsParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/UpdateLogRetentions", ReplyAction = "http://tempuri.org/ILogConfigurationService/UpdateLogRetentionsResponse")]
	Task<UpdateLogRetentionsResp> UpdateLogRetentionsAsync(UpdateLogRetentionsParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/UpdateAnonymousLogging", ReplyAction = "http://tempuri.org/ILogConfigurationService/UpdateAnonymousLoggingResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogConfigurationService/UpdateAnonymousLoggingUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	UpdateAnonymousLoggingResp UpdateAnonymousLogging(UpdateAnonymousLoggingParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogConfigurationService/UpdateAnonymousLogging", ReplyAction = "http://tempuri.org/ILogConfigurationService/UpdateAnonymousLoggingResponse")]
	Task<UpdateAnonymousLoggingResp> UpdateAnonymousLoggingAsync(UpdateAnonymousLoggingParams parameters);
}

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LogReaderServiceReference.ILogReaderTokenService")]
public interface ILogReaderTokenService
{
	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/StartQuerySessionToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/StartQuerySessionTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderTokenService/StartQuerySessionTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	StartQuerySessionResp StartQuerySessionToken(string token, StartQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/StartQuerySessionToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/StartQuerySessionTokenResponse")]
	Task<StartQuerySessionResp> StartQuerySessionTokenAsync(string token, StartQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/FetchAuditLoglinesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/FetchAuditLoglinesTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderTokenService/FetchAuditLoglinesTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	FetchAuditLoglinesResp FetchAuditLoglinesToken(string token, FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/FetchAuditLoglinesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/FetchAuditLoglinesTokenResponse")]
	Task<FetchAuditLoglinesResp> FetchAuditLoglinesTokenAsync(string token, FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/FetchSystemLoglinesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/FetchSystemLoglinesTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderTokenService/FetchSystemLoglinesTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	FetchSystemLoglinesResp FetchSystemLoglinesToken(string token, FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/FetchSystemLoglinesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/FetchSystemLoglinesTokenResponse")]
	Task<FetchSystemLoglinesResp> FetchSystemLoglinesTokenAsync(string token, FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/FetchRuleLoglinesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/FetchRuleLoglinesTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderTokenService/FetchRuleLoglinesTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	FetchRuleLoglinesResp FetchRuleLoglinesToken(string token, FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/FetchRuleLoglinesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/FetchRuleLoglinesTokenResponse")]
	Task<FetchRuleLoglinesResp> FetchRuleLoglinesTokenAsync(string token, FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/EndQuerySessionToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/EndQuerySessionTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderTokenService/EndQuerySessionTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	EndQuerySessionResp EndQuerySessionToken(string token, EndQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/EndQuerySessionToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/EndQuerySessionTokenResponse")]
	Task<EndQuerySessionResp> EndQuerySessionTokenAsync(string token, EndQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/GetFilterValuesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/GetFilterValuesTokenResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderTokenService/GetFilterValuesTokenUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetFilterValuesResp GetFilterValuesToken(string token, GetFilterValuesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderTokenService/GetFilterValuesToken", ReplyAction = "http://tempuri.org/ILogReaderTokenService/GetFilterValuesTokenResponse")]
	Task<GetFilterValuesResp> GetFilterValuesTokenAsync(string token, GetFilterValuesParams parameters);
}

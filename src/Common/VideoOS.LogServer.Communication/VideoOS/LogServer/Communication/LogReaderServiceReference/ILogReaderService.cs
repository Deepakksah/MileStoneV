using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LogReaderServiceReference.ILogReaderService")]
public interface ILogReaderService
{
	[OperationContract(Action = "http://tempuri.org/ILogReaderService/StartQuerySession", ReplyAction = "http://tempuri.org/ILogReaderService/StartQuerySessionResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/StartQuerySessionUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/StartQuerySessionUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/StartQuerySessionQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	StartQuerySessionResp StartQuerySession(StartQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/StartQuerySession", ReplyAction = "http://tempuri.org/ILogReaderService/StartQuerySessionResponse")]
	Task<StartQuerySessionResp> StartQuerySessionAsync(StartQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/FetchAuditLoglines", ReplyAction = "http://tempuri.org/ILogReaderService/FetchAuditLoglinesResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/FetchAuditLoglinesUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/FetchAuditLoglinesUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/FetchAuditLoglinesQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	FetchAuditLoglinesResp FetchAuditLoglines(FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/FetchAuditLoglines", ReplyAction = "http://tempuri.org/ILogReaderService/FetchAuditLoglinesResponse")]
	Task<FetchAuditLoglinesResp> FetchAuditLoglinesAsync(FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/FetchSystemLoglines", ReplyAction = "http://tempuri.org/ILogReaderService/FetchSystemLoglinesResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/FetchSystemLoglinesUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/FetchSystemLoglinesUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/FetchSystemLoglinesQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	FetchSystemLoglinesResp FetchSystemLoglines(FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/FetchSystemLoglines", ReplyAction = "http://tempuri.org/ILogReaderService/FetchSystemLoglinesResponse")]
	Task<FetchSystemLoglinesResp> FetchSystemLoglinesAsync(FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/FetchRuleLoglines", ReplyAction = "http://tempuri.org/ILogReaderService/FetchRuleLoglinesResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/FetchRuleLoglinesUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/FetchRuleLoglinesUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/FetchRuleLoglinesQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	FetchRuleLoglinesResp FetchRuleLoglines(FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/FetchRuleLoglines", ReplyAction = "http://tempuri.org/ILogReaderService/FetchRuleLoglinesResponse")]
	Task<FetchRuleLoglinesResp> FetchRuleLoglinesAsync(FetchLoglinesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/EndQuerySession", ReplyAction = "http://tempuri.org/ILogReaderService/EndQuerySessionResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/EndQuerySessionUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/EndQuerySessionUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/EndQuerySessionQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	EndQuerySessionResp EndQuerySession(EndQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/EndQuerySession", ReplyAction = "http://tempuri.org/ILogReaderService/EndQuerySessionResponse")]
	Task<EndQuerySessionResp> EndQuerySessionAsync(EndQuerySessionParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/GetFilterValues", ReplyAction = "http://tempuri.org/ILogReaderService/GetFilterValuesResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/GetFilterValuesUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/GetFilterValuesUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/GetFilterValuesQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetFilterValuesResp GetFilterValues(GetFilterValuesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/GetFilterValues", ReplyAction = "http://tempuri.org/ILogReaderService/GetFilterValuesResponse")]
	Task<GetFilterValuesResp> GetFilterValuesAsync(GetFilterValuesParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestamp", ReplyAction = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestampResponse")]
	[FaultContract(typeof(UserValidationFault), Action = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestampUserValidationFaultFault", Name = "UserValidationFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(UnknownSessionFault), Action = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestampUnknownSessionFaultFault", Name = "UnknownSessionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	[FaultContract(typeof(QueryTimeoutFault), Action = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestampQueryTimeoutFaultFault", Name = "QueryTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
	GetFilterValuesResp GetFilterValuesTimestamp(GetFilterValuesTimestampParams parameters);

	[OperationContract(Action = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestamp", ReplyAction = "http://tempuri.org/ILogReaderService/GetFilterValuesTimestampResponse")]
	Task<GetFilterValuesResp> GetFilterValuesTimestampAsync(GetFilterValuesTimestampParams parameters);
}

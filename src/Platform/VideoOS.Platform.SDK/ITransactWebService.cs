using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Platform.SDK.Proxy.Transact;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[ServiceContract(ConfigurationName = "ITransactWebService")]
public interface ITransactWebService
{
	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetTestMsg", ReplyAction = "http://tempuri.org/ITransactWebService/GetTestMsgResponse")]
	string GetTestMsg();

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetSources", ReplyAction = "http://tempuri.org/ITransactWebService/GetSourcesResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetSourcesWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactSource[] GetSources(string token);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetSourceState", ReplyAction = "http://tempuri.org/ITransactWebService/GetSourceStateResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetSourceStateWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactSourceState GetSourceState(string token, Guid sourceId);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetLiveLines", ReplyAction = "http://tempuri.org/ITransactWebService/GetLiveLinesResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetLiveLinesWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactLine[] GetLiveLines(string token, Guid sourceId, int maxLines);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetLinesBeforeTime", ReplyAction = "http://tempuri.org/ITransactWebService/GetLinesBeforeTimeResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetLinesBeforeTimeWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactLine[] GetLinesBeforeTime(string token, Guid sourceId, DateTime baseTime, int maxLines);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetLinesAfterTime", ReplyAction = "http://tempuri.org/ITransactWebService/GetLinesAfterTimeResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetLinesAfterTimeWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactLine[] GetLinesAfterTime(string token, Guid sourceId, DateTime baseTime, int maxLines);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetLinesBeforeID", ReplyAction = "http://tempuri.org/ITransactWebService/GetLinesBeforeIDResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetLinesBeforeIDWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactLine[] GetLinesBeforeID(string token, Guid sourceId, int lineId, int maxLines);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetLinesAfterID", ReplyAction = "http://tempuri.org/ITransactWebService/GetLinesAfterIDResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetLinesAfterIDWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	WSTransactLine[] GetLinesAfterID(string token, Guid sourceId, int lineId, int maxLines);

	[OperationContract(Action = "http://tempuri.org/ITransactWebService/GetData", ReplyAction = "http://tempuri.org/ITransactWebService/GetDataResponse")]
	[FaultContract(typeof(WSTransactFault), Action = "http://tempuri.org/ITransactWebService/GetDataWSTransactFaultFault", Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
	string GetData(string token, string query, int maxLines);
}

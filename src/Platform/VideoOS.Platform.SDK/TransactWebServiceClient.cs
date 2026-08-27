using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Platform.SDK.Proxy.Transact;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class TransactWebServiceClient : ClientBase<ITransactWebService>, ITransactWebService
{
	public TransactWebServiceClient()
	{
	}

	public TransactWebServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public TransactWebServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public TransactWebServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public TransactWebServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string GetTestMsg()
	{
		return base.Channel.GetTestMsg();
	}

	public WSTransactSource[] GetSources(string token)
	{
		return base.Channel.GetSources(token);
	}

	public WSTransactSourceState GetSourceState(string token, Guid sourceId)
	{
		return base.Channel.GetSourceState(token, sourceId);
	}

	public WSTransactLine[] GetLiveLines(string token, Guid sourceId, int maxLines)
	{
		return base.Channel.GetLiveLines(token, sourceId, maxLines);
	}

	public WSTransactLine[] GetLinesBeforeTime(string token, Guid sourceId, DateTime baseTime, int maxLines)
	{
		return base.Channel.GetLinesBeforeTime(token, sourceId, baseTime, maxLines);
	}

	public WSTransactLine[] GetLinesAfterTime(string token, Guid sourceId, DateTime baseTime, int maxLines)
	{
		return base.Channel.GetLinesAfterTime(token, sourceId, baseTime, maxLines);
	}

	public WSTransactLine[] GetLinesBeforeID(string token, Guid sourceId, int lineId, int maxLines)
	{
		return base.Channel.GetLinesBeforeID(token, sourceId, lineId, maxLines);
	}

	public WSTransactLine[] GetLinesAfterID(string token, Guid sourceId, int lineId, int maxLines)
	{
		return base.Channel.GetLinesAfterID(token, sourceId, lineId, maxLines);
	}

	public string GetData(string token, string query, int maxLines)
	{
		return base.Channel.GetData(token, query, maxLines);
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LogReaderServiceClient : ClientBase<ILogReaderService>, ILogReaderService
{
	public LogReaderServiceClient()
	{
	}

	public LogReaderServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LogReaderServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogReaderServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogReaderServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public StartQuerySessionResp StartQuerySession(StartQuerySessionParams parameters)
	{
		return base.Channel.StartQuerySession(parameters);
	}

	public Task<StartQuerySessionResp> StartQuerySessionAsync(StartQuerySessionParams parameters)
	{
		return base.Channel.StartQuerySessionAsync(parameters);
	}

	public FetchAuditLoglinesResp FetchAuditLoglines(FetchLoglinesParams parameters)
	{
		return base.Channel.FetchAuditLoglines(parameters);
	}

	public Task<FetchAuditLoglinesResp> FetchAuditLoglinesAsync(FetchLoglinesParams parameters)
	{
		return base.Channel.FetchAuditLoglinesAsync(parameters);
	}

	public FetchSystemLoglinesResp FetchSystemLoglines(FetchLoglinesParams parameters)
	{
		return base.Channel.FetchSystemLoglines(parameters);
	}

	public Task<FetchSystemLoglinesResp> FetchSystemLoglinesAsync(FetchLoglinesParams parameters)
	{
		return base.Channel.FetchSystemLoglinesAsync(parameters);
	}

	public FetchRuleLoglinesResp FetchRuleLoglines(FetchLoglinesParams parameters)
	{
		return base.Channel.FetchRuleLoglines(parameters);
	}

	public Task<FetchRuleLoglinesResp> FetchRuleLoglinesAsync(FetchLoglinesParams parameters)
	{
		return base.Channel.FetchRuleLoglinesAsync(parameters);
	}

	public EndQuerySessionResp EndQuerySession(EndQuerySessionParams parameters)
	{
		return base.Channel.EndQuerySession(parameters);
	}

	public Task<EndQuerySessionResp> EndQuerySessionAsync(EndQuerySessionParams parameters)
	{
		return base.Channel.EndQuerySessionAsync(parameters);
	}

	public GetFilterValuesResp GetFilterValues(GetFilterValuesParams parameters)
	{
		return base.Channel.GetFilterValues(parameters);
	}

	public Task<GetFilterValuesResp> GetFilterValuesAsync(GetFilterValuesParams parameters)
	{
		return base.Channel.GetFilterValuesAsync(parameters);
	}

	public GetFilterValuesResp GetFilterValuesTimestamp(GetFilterValuesTimestampParams parameters)
	{
		return base.Channel.GetFilterValuesTimestamp(parameters);
	}

	public Task<GetFilterValuesResp> GetFilterValuesTimestampAsync(GetFilterValuesTimestampParams parameters)
	{
		return base.Channel.GetFilterValuesTimestampAsync(parameters);
	}
}

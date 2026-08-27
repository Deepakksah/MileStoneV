using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LogReaderTokenServiceClient : ClientBase<ILogReaderTokenService>, ILogReaderTokenService
{
	public LogReaderTokenServiceClient()
	{
	}

	public LogReaderTokenServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LogReaderTokenServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogReaderTokenServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogReaderTokenServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public StartQuerySessionResp StartQuerySessionToken(string token, StartQuerySessionParams parameters)
	{
		return base.Channel.StartQuerySessionToken(token, parameters);
	}

	public Task<StartQuerySessionResp> StartQuerySessionTokenAsync(string token, StartQuerySessionParams parameters)
	{
		return base.Channel.StartQuerySessionTokenAsync(token, parameters);
	}

	public FetchAuditLoglinesResp FetchAuditLoglinesToken(string token, FetchLoglinesParams parameters)
	{
		return base.Channel.FetchAuditLoglinesToken(token, parameters);
	}

	public Task<FetchAuditLoglinesResp> FetchAuditLoglinesTokenAsync(string token, FetchLoglinesParams parameters)
	{
		return base.Channel.FetchAuditLoglinesTokenAsync(token, parameters);
	}

	public FetchSystemLoglinesResp FetchSystemLoglinesToken(string token, FetchLoglinesParams parameters)
	{
		return base.Channel.FetchSystemLoglinesToken(token, parameters);
	}

	public Task<FetchSystemLoglinesResp> FetchSystemLoglinesTokenAsync(string token, FetchLoglinesParams parameters)
	{
		return base.Channel.FetchSystemLoglinesTokenAsync(token, parameters);
	}

	public FetchRuleLoglinesResp FetchRuleLoglinesToken(string token, FetchLoglinesParams parameters)
	{
		return base.Channel.FetchRuleLoglinesToken(token, parameters);
	}

	public Task<FetchRuleLoglinesResp> FetchRuleLoglinesTokenAsync(string token, FetchLoglinesParams parameters)
	{
		return base.Channel.FetchRuleLoglinesTokenAsync(token, parameters);
	}

	public EndQuerySessionResp EndQuerySessionToken(string token, EndQuerySessionParams parameters)
	{
		return base.Channel.EndQuerySessionToken(token, parameters);
	}

	public Task<EndQuerySessionResp> EndQuerySessionTokenAsync(string token, EndQuerySessionParams parameters)
	{
		return base.Channel.EndQuerySessionTokenAsync(token, parameters);
	}

	public GetFilterValuesResp GetFilterValuesToken(string token, GetFilterValuesParams parameters)
	{
		return base.Channel.GetFilterValuesToken(token, parameters);
	}

	public Task<GetFilterValuesResp> GetFilterValuesTokenAsync(string token, GetFilterValuesParams parameters)
	{
		return base.Channel.GetFilterValuesTokenAsync(token, parameters);
	}
}

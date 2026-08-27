using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogConfigurationServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LogConfigurationTokenServiceClient : ClientBase<ILogConfigurationTokenService>, ILogConfigurationTokenService
{
	public LogConfigurationTokenServiceClient()
	{
	}

	public LogConfigurationTokenServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LogConfigurationTokenServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogConfigurationTokenServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogConfigurationTokenServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetLogRetentionsResp GetLogRetentionsToken(string token)
	{
		return base.Channel.GetLogRetentionsToken(token);
	}

	public Task<GetLogRetentionsResp> GetLogRetentionsTokenAsync(string token)
	{
		return base.Channel.GetLogRetentionsTokenAsync(token);
	}

	public GetAnonymousLoggingResp GetAnonymousLoggingToken(string token)
	{
		return base.Channel.GetAnonymousLoggingToken(token);
	}

	public Task<GetAnonymousLoggingResp> GetAnonymousLoggingTokenAsync(string token)
	{
		return base.Channel.GetAnonymousLoggingTokenAsync(token);
	}

	public UpdateLogRetentionsResp UpdateLogRetentionsToken(UpdateLogRetentionsParams parameters, string token)
	{
		return base.Channel.UpdateLogRetentionsToken(parameters, token);
	}

	public Task<UpdateLogRetentionsResp> UpdateLogRetentionsTokenAsync(UpdateLogRetentionsParams parameters, string token)
	{
		return base.Channel.UpdateLogRetentionsTokenAsync(parameters, token);
	}

	public UpdateAnonymousLoggingResp UpdateAnonymousLoggingToken(UpdateAnonymousLoggingParams parameters, string token)
	{
		return base.Channel.UpdateAnonymousLoggingToken(parameters, token);
	}

	public Task<UpdateAnonymousLoggingResp> UpdateAnonymousLoggingTokenAsync(UpdateAnonymousLoggingParams parameters, string token)
	{
		return base.Channel.UpdateAnonymousLoggingTokenAsync(parameters, token);
	}
}

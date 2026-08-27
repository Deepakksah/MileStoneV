using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogConfigurationServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LogConfigurationServiceClient : ClientBase<ILogConfigurationService>, ILogConfigurationService
{
	public LogConfigurationServiceClient()
	{
	}

	public LogConfigurationServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LogConfigurationServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogConfigurationServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogConfigurationServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetLogRetentionsResp GetLogRetentions()
	{
		return base.Channel.GetLogRetentions();
	}

	public Task<GetLogRetentionsResp> GetLogRetentionsAsync()
	{
		return base.Channel.GetLogRetentionsAsync();
	}

	public GetAnonymousLoggingResp GetAnonymousLogging()
	{
		return base.Channel.GetAnonymousLogging();
	}

	public Task<GetAnonymousLoggingResp> GetAnonymousLoggingAsync()
	{
		return base.Channel.GetAnonymousLoggingAsync();
	}

	public UpdateLogRetentionsResp UpdateLogRetentions(UpdateLogRetentionsParams parameters)
	{
		return base.Channel.UpdateLogRetentions(parameters);
	}

	public Task<UpdateLogRetentionsResp> UpdateLogRetentionsAsync(UpdateLogRetentionsParams parameters)
	{
		return base.Channel.UpdateLogRetentionsAsync(parameters);
	}

	public UpdateAnonymousLoggingResp UpdateAnonymousLogging(UpdateAnonymousLoggingParams parameters)
	{
		return base.Channel.UpdateAnonymousLogging(parameters);
	}

	public Task<UpdateAnonymousLoggingResp> UpdateAnonymousLoggingAsync(UpdateAnonymousLoggingParams parameters)
	{
		return base.Channel.UpdateAnonymousLoggingAsync(parameters);
	}
}

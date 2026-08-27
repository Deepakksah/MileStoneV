using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LogWriterTokenServiceClient : ClientBase<ILogWriterTokenService>, ILogWriterTokenService
{
	public LogWriterTokenServiceClient()
	{
	}

	public LogWriterTokenServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LogWriterTokenServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogWriterTokenServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogWriterTokenServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public RegisterSourceTypeResp RegisterSourceTypeToken(string token, RegisterSourceTypeParams parameters)
	{
		return base.Channel.RegisterSourceTypeToken(token, parameters);
	}

	public Task<RegisterSourceTypeResp> RegisterSourceTypeTokenAsync(string token, RegisterSourceTypeParams parameters)
	{
		return base.Channel.RegisterSourceTypeTokenAsync(token, parameters);
	}

	public RegisterCategoryResp RegisterCategoryToken(string token, RegisterCategoryParams parameters)
	{
		return base.Channel.RegisterCategoryToken(token, parameters);
	}

	public Task<RegisterCategoryResp> RegisterCategoryTokenAsync(string token, RegisterCategoryParams parameters)
	{
		return base.Channel.RegisterCategoryTokenAsync(token, parameters);
	}

	public RegisterLogMessagesResp RegisterLogMessagesToken(string token, RegisterLogMessagesParams parameters)
	{
		return base.Channel.RegisterLogMessagesToken(token, parameters);
	}

	public Task<RegisterLogMessagesResp> RegisterLogMessagesTokenAsync(string token, RegisterLogMessagesParams parameters)
	{
		return base.Channel.RegisterLogMessagesTokenAsync(token, parameters);
	}

	public RegisterLogMessagesResp RegisterLegacyLogMessagesToken(string token, RegisterLegacyLogMessagesParams parameters)
	{
		return base.Channel.RegisterLegacyLogMessagesToken(token, parameters);
	}

	public Task<RegisterLogMessagesResp> RegisterLegacyLogMessagesTokenAsync(string token, RegisterLegacyLogMessagesParams parameters)
	{
		return base.Channel.RegisterLegacyLogMessagesTokenAsync(token, parameters);
	}

	public WriteLoglineResp WriteLoglineToken(string token, WriteLoglineParams parameters)
	{
		return base.Channel.WriteLoglineToken(token, parameters);
	}

	public Task<WriteLoglineResp> WriteLoglineTokenAsync(string token, WriteLoglineParams parameters)
	{
		return base.Channel.WriteLoglineTokenAsync(token, parameters);
	}

	public WriteLoglineResp WriteLegacyLoglineToken(string token, WriteLegacyLoglineParams parameters)
	{
		return base.Channel.WriteLegacyLoglineToken(token, parameters);
	}

	public Task<WriteLoglineResp> WriteLegacyLoglineTokenAsync(string token, WriteLegacyLoglineParams parameters)
	{
		return base.Channel.WriteLegacyLoglineTokenAsync(token, parameters);
	}
}

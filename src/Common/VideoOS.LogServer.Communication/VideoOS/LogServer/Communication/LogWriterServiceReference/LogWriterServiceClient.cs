using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LogWriterServiceClient : ClientBase<ILogWriterService>, ILogWriterService
{
	public LogWriterServiceClient()
	{
	}

	public LogWriterServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LogWriterServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogWriterServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LogWriterServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public RegisterSourceTypeResp RegisterSourceType(RegisterSourceTypeParams parameters)
	{
		return base.Channel.RegisterSourceType(parameters);
	}

	public Task<RegisterSourceTypeResp> RegisterSourceTypeAsync(RegisterSourceTypeParams parameters)
	{
		return base.Channel.RegisterSourceTypeAsync(parameters);
	}

	public RegisterCategoryResp RegisterCategory(RegisterCategoryParams parameters)
	{
		return base.Channel.RegisterCategory(parameters);
	}

	public Task<RegisterCategoryResp> RegisterCategoryAsync(RegisterCategoryParams parameters)
	{
		return base.Channel.RegisterCategoryAsync(parameters);
	}

	public RegisterLogMessagesResp RegisterLogMessages(RegisterLogMessagesParams parameters)
	{
		return base.Channel.RegisterLogMessages(parameters);
	}

	public Task<RegisterLogMessagesResp> RegisterLogMessagesAsync(RegisterLogMessagesParams parameters)
	{
		return base.Channel.RegisterLogMessagesAsync(parameters);
	}

	public RegisterLogMessagesResp RegisterLegacyLogMessages(RegisterLegacyLogMessagesParams parameters)
	{
		return base.Channel.RegisterLegacyLogMessages(parameters);
	}

	public Task<RegisterLogMessagesResp> RegisterLegacyLogMessagesAsync(RegisterLegacyLogMessagesParams parameters)
	{
		return base.Channel.RegisterLegacyLogMessagesAsync(parameters);
	}

	public WriteLoglineResp WriteLogline(WriteLoglineParams parameters)
	{
		return base.Channel.WriteLogline(parameters);
	}

	public Task<WriteLoglineResp> WriteLoglineAsync(WriteLoglineParams parameters)
	{
		return base.Channel.WriteLoglineAsync(parameters);
	}

	public WriteLoglineResp WriteLegacyLogline(WriteLegacyLoglineParams parameters)
	{
		return base.Channel.WriteLegacyLogline(parameters);
	}

	public Task<WriteLoglineResp> WriteLegacyLoglineAsync(WriteLegacyLoglineParams parameters)
	{
		return base.Channel.WriteLegacyLoglineAsync(parameters);
	}
}

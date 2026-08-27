using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class InterCommunicationCommandTokenClient : ClientBase<IInterCommunicationCommandToken>, IInterCommunicationCommandToken
{
	public InterCommunicationCommandTokenClient()
	{
	}

	public InterCommunicationCommandTokenClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public InterCommunicationCommandTokenClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public InterCommunicationCommandTokenClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public InterCommunicationCommandTokenClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public IntercommunicationReport GenerateReportToken(string token, string culture, string timeZoneId, IntercommunicationReportRequest request, int timeout)
	{
		return base.Channel.GenerateReportToken(token, culture, timeZoneId, request, timeout);
	}

	public IntercommunicationReport GenerateReportTokenUserId(string token, InterCommunicationUserID userId, string culture, string timeZoneId, IntercommunicationReportRequest request, Guid[] serverIds, int timeout)
	{
		return base.Channel.GenerateReportTokenUserId(token, userId, culture, timeZoneId, request, serverIds, timeout);
	}
}

using System;
using VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

namespace VideoOS.Reporting.Web.Plugin.Business.Alarm;

public class IntercommunicationCommandAgentWrapper : IIntercommunicationCommandAgent
{
	private IntercommunicationCommandAgent _agent;

	public IntercommunicationCommandAgentWrapper()
	{
		_agent = new IntercommunicationCommandAgent();
	}

	public IntercommunicationReport GenerateReport(string serverAddress, string token, string culture, TimeZoneInfo timeZone, Guid wcfSessionId, IntercommunicationReportRequest request)
	{
		return _agent.GenerateReport(serverAddress, token, culture, timeZone, wcfSessionId, request);
	}
}

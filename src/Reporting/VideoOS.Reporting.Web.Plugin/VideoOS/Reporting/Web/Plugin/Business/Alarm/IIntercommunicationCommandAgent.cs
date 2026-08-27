using System;
using VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

namespace VideoOS.Reporting.Web.Plugin.Business.Alarm;

public interface IIntercommunicationCommandAgent
{
	IntercommunicationReport GenerateReport(string serverAddress, string token, string culture, TimeZoneInfo timeZone, Guid wcfSessionId, IntercommunicationReportRequest request);
}

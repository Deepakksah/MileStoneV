using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommandToken", ConfigurationName = "VideoOS.Event.Server.WebService.InterCommunication.Client.IInterCommunicationCommandToken", SessionMode = SessionMode.NotAllowed)]
public interface IInterCommunicationCommandToken
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommandToken/IInterCommunicationCommandToken/GenerateReportToken", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommandToken/IInterCommunicationCommandToken/GenerateReportTokenResponse")]
	[FaultContract(typeof(IntercommunicationServiceFault), Action = "http://videoos.net/2/CentralServerInterCommunicationCommandToken/IInterCommunicationCommandToken/GenerateReportTokenIntercommunicationServiceFaultFault", Name = "IntercommunicationServiceFault")]
	IntercommunicationReport GenerateReportToken(string token, string culture, string timeZoneId, IntercommunicationReportRequest request, int timeout);

	[OperationContract(Action = "http://videoos.net/2/CentralServerInterCommunicationCommandToken/IInterCommunicationCommandToken/GenerateReportTokenUserId", ReplyAction = "http://videoos.net/2/CentralServerInterCommunicationCommandToken/IInterCommunicationCommandToken/GenerateReportTokenUserIdResponse")]
	[FaultContract(typeof(IntercommunicationServiceFault), Action = "http://videoos.net/2/CentralServerInterCommunicationCommandToken/IInterCommunicationCommandToken/GenerateReportTokenUserIdIntercommunicationServiceFaultFault", Name = "IntercommunicationServiceFault")]
	IntercommunicationReport GenerateReportTokenUserId(string token, InterCommunicationUserID userId, string culture, string timeZoneId, IntercommunicationReportRequest request, Guid[] serverIds, int timeout);
}

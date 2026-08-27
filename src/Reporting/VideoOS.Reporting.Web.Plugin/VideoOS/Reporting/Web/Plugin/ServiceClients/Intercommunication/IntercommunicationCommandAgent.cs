using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

public class IntercommunicationCommandAgent
{
	private const int EVENT_SERVER_TIMEOUT = 120000;

	private readonly int DEFAULT_RETRY_COUNT = 2;

	private const int MAX_ITEMS_IN_OBJECT_GRAPH = 1000000;

	private const int MESSAGE_BUFFER_SIZE = 500000000;

	private static InterCommunicationCommandTokenClient CreateIntercommunicationCommandTokenClient(string serverAddress, TimeSpan timeout)
	{
		EndpointIdentity identity = EndpointIdentity.CreateSpnIdentity("MilestoneCentralServerSide");
		UriBuilder uriBuilder = new UriBuilder(serverAddress);
		uriBuilder.Path = "Central/InterCommCommandToken";
		EndpointAddress remoteAddress = new EndpointAddress(uriBuilder.Uri, identity);
		InterCommunicationCommandTokenClient interCommunicationCommandTokenClient = new InterCommunicationCommandTokenClient(new BasicHttpBinding
		{
			CloseTimeout = timeout,
			OpenTimeout = timeout,
			ReceiveTimeout = timeout,
			SendTimeout = timeout,
			MaxReceivedMessageSize = 500000000L,
			Security = 
			{
				Mode = ((!(uriBuilder.Uri.Scheme == Uri.UriSchemeHttp)) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None)
			}
		}, remoteAddress);
		foreach (OperationDescription operation in interCommunicationCommandTokenClient.Endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 1000000;
			}
		}
		return interCommunicationCommandTokenClient;
	}

	public IntercommunicationReport GenerateReport(string serverAddress, string token, string culture, TimeZoneInfo timeZone, Guid wcfSessionId, IntercommunicationReportRequest request)
	{
		if (timeZone == null)
		{
			throw new ArgumentNullException("timeZone");
		}
		int num = 140000;
		int num2 = DEFAULT_RETRY_COUNT;
		while (true)
		{
			InterCommunicationCommandTokenClient interCommunicationCommandTokenClient = CreateIntercommunicationCommandTokenClient(serverAddress, TimeSpan.FromMilliseconds(num));
			if (wcfSessionId != Guid.Empty)
			{
				interCommunicationCommandTokenClient.ChannelFactory.Endpoint.Behaviors.Add(new CustomHeaderEndpointBehavior(() => new MessageHeader<string>(wcfSessionId.ToString()).GetUntypedHeader("client-session-id", "csid")));
			}
			try
			{
				return interCommunicationCommandTokenClient.GenerateReportToken(token, culture, timeZone.Id, request, 120000);
			}
			catch (EndpointNotFoundException)
			{
				throw;
			}
			catch (FaultException<IntercommunicationServiceFault>)
			{
				throw;
			}
			catch (Exception)
			{
				num2--;
				if (num2 <= 0)
				{
					throw;
				}
			}
			finally
			{
				try
				{
					if (interCommunicationCommandTokenClient.State == CommunicationState.Opened || interCommunicationCommandTokenClient.State == CommunicationState.Created)
					{
						interCommunicationCommandTokenClient.Close();
					}
					else
					{
						interCommunicationCommandTokenClient.Abort();
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}
}

using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config.Shared;

public class LoginServerOauth : LoginServerSvc, ILoginServer
{
	private const string CommandServiceOAuthPath = "ManagementServer/ServerCommandServiceOAuth.svc";

	public LoginServerOauth(UserContext userContext, ISystemInfo systemInfo)
		: base(userContext, systemInfo)
	{
	}

	public override void SetCredentials(ClientCredentials clientCredentials, NetworkCredential nc)
	{
	}

	public override Uri CalculateServiceUrl(Uri uri)
	{
		return new UriBuilder(uri.Scheme, uri.DnsSafeHost, uri.Port, "ManagementServer/ServerCommandServiceOAuth.svc").Uri;
	}

	public override ServerCommandServiceClient CreateClient(Uri uri, Binding binding, IMipTokenCache mipTokenCache)
	{
		if (uri.Scheme == Uri.UriSchemeHttp)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginServerOauth.CreateClient", "Creating an HTTP client using uri: " + uri.ToString());
		}
		SpnEndpointIdentity identity = new SpnEndpointIdentity(SpnFactory.GetSpn(uri));
		ServerCommandServiceClient serverCommandServiceClient = new ServerCommandServiceClient(binding, new EndpointAddress(uri, identity));
		ManagementServerWcfHelper.SetupCertificateValidation(serverCommandServiceClient);
		ManagementServerWcfHelper.ConfigureEndpoint(serverCommandServiceClient.Endpoint, mipTokenCache);
		foreach (OperationDescription operation in serverCommandServiceClient.Endpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
		return serverCommandServiceClient;
	}

	public override Binding GetBinding(Uri uri)
	{
		if (Environment.Properties.LoginServerSvcTimeoutInSeconds == 0)
		{
			return ManagementServerWcfHelper.GetOAuthBinding(uri.Scheme == Uri.UriSchemeHttps);
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(Environment.Properties.LoginServerSvcTimeoutInSeconds);
		return ManagementServerWcfHelper.GetOAuthBinding(uri.Scheme == Uri.UriSchemeHttps, timeSpan, timeSpan, timeSpan, timeSpan);
	}
}

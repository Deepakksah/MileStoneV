using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util.Svc;

namespace VideoOS.Platform.Util.Security;

public class SecurityClientXPCOOAuth : SecurityClientXPCO, IDisposable
{
	public override string ServiceName => "SecurityServiceOAuth.svc";

	public SecurityClientXPCOOAuth(LoginSettings loginSettings)
		: base(loginSettings)
	{
	}

	public override Binding GetBinding()
	{
		Binding oAuthBinding = ServiceApiSvcHelper.GetOAuthBinding(base.LoginSettings.Uri.Scheme == Uri.UriSchemeHttps);
		oAuthBinding.SendTimeout = TimeSpan.FromMinutes(10.0);
		oAuthBinding.OpenTimeout = TimeSpan.FromMinutes(1.0);
		oAuthBinding.CloseTimeout = TimeSpan.FromMinutes(1.0);
		oAuthBinding.ReceiveTimeout = TimeSpan.FromMinutes(10.0);
		return oAuthBinding;
	}

	public override SecurityServiceClient CreateClient()
	{
		SecurityServiceClient securityServiceClient = new SecurityServiceClient(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(securityServiceClient.Endpoint);
		return securityServiceClient;
	}

	public override void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint)
	{
		serviceEndpoint.EndpointBehaviors.Add(new ServiceUtil.AddTokenBehavior(base.LoginSettings));
		foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
	}
}

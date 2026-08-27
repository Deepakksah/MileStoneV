using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Management.Server;

public class BasicAuthenticationMessageInspector : IDispatchMessageInspector
{
	private IBasicAuthenticationModule _authModule;

	public const string BasicAuthenticationResultKey = "BasicAuthenticationResult";

	public BasicAuthenticationMessageInspector(IBasicAuthenticationModule basicAuthenticationModule)
	{
		if (basicAuthenticationModule == null)
		{
			throw new ArgumentNullException("basicAuthenticationModule");
		}
		_authModule = basicAuthenticationModule;
	}

	public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		MessagePropertiesManager.IncomingMessageProperties.Add("BasicAuthenticationResult", false);
		if (!(request.Properties["httpRequest"] is HttpRequestMessageProperty httpRequestMessageProperty))
		{
			return UnauthorizedResponse();
		}
		string text = httpRequestMessageProperty.Headers["Authorization"];
		if (string.IsNullOrEmpty(text))
		{
			return UnauthorizedResponse();
		}
		if (!_authModule.AuthenticateUser(text, out var principal))
		{
			return UnauthorizedResponse();
		}
		MessagePropertiesManager.IncomingMessageProperties["BasicAuthenticationResult"] = true;
		HttpContextManager.Current.User = principal;
		return null;
	}

	private static BasicAuthenticationErrorResponseData UnauthorizedResponse()
	{
		return new BasicAuthenticationErrorResponseData(HttpStatusCode.Unauthorized, new KeyValuePair<string, string>("WWW-Authenticate", "Basic realm=\"localhost\""));
	}

	public void BeforeSendReply(ref Message reply, object correlationState)
	{
		if (!(correlationState is BasicAuthenticationErrorResponseData basicAuthenticationErrorResponseData))
		{
			return;
		}
		if (reply == null)
		{
			throw new ArgumentNullException("reply");
		}
		HttpResponseMessageProperty httpResponseMessageProperty = new HttpResponseMessageProperty();
		reply.Properties["httpResponse"] = httpResponseMessageProperty;
		httpResponseMessageProperty.StatusCode = basicAuthenticationErrorResponseData.StatusCode;
		IReadOnlyCollection<KeyValuePair<string, string>> headers = basicAuthenticationErrorResponseData.Headers;
		if (headers == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> item in headers)
		{
			httpResponseMessageProperty.Headers.Add(item.Key, item.Value);
		}
	}
}

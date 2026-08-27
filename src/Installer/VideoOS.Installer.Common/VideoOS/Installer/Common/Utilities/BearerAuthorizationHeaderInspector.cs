using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Installer.Common.Utilities;

public class BearerAuthorizationHeaderInspector : IClientMessageInspector
{
	private readonly string _accessToken;

	private const HttpRequestHeader AuthorizationHeader = HttpRequestHeader.Authorization;

	public BearerAuthorizationHeaderInspector(string accessToken)
	{
		_accessToken = accessToken;
	}

	public object BeforeSendRequest(ref Message request, IClientChannel channel)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request", "Invalid Argument");
		}
		if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out var value))
		{
			HttpRequestMessageProperty httpRequestMessageProperty = (HttpRequestMessageProperty)value;
			if (string.IsNullOrEmpty(httpRequestMessageProperty.Headers[HttpRequestHeader.Authorization]))
			{
				httpRequestMessageProperty.Headers[HttpRequestHeader.Authorization] = FormatToken();
			}
		}
		else
		{
			HttpRequestMessageProperty httpRequestMessageProperty2 = new HttpRequestMessageProperty();
			httpRequestMessageProperty2.Headers.Add(HttpRequestHeader.Authorization, FormatToken());
			request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty2);
		}
		return null;
	}

	public void AfterReceiveReply(ref Message reply, object correlationState)
	{
	}

	private string FormatToken()
	{
		return FormattableString.Invariant($"Bearer {_accessToken}");
	}
}

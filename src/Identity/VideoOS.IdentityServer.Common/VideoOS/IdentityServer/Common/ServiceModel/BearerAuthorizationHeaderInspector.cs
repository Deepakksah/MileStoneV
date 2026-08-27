using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public class BearerAuthorizationHeaderInspector : IClientMessageInspector
{
	private readonly ITokenCache _tokenCache;

	private const HttpRequestHeader AuthorizationHeader = HttpRequestHeader.Authorization;

	public BearerAuthorizationHeaderInspector(ITokenCache tokenCache)
	{
		if (tokenCache == null)
		{
			throw new ArgumentNullException("tokenCache");
		}
		_tokenCache = tokenCache;
	}

	public object BeforeSendRequest(ref Message request, IClientChannel channel)
	{
		if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out var value))
		{
			HttpRequestMessageProperty httpRequestMessageProperty = value as HttpRequestMessageProperty;
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

	private string FormatToken()
	{
		//IL_0071: Expected O, but got Unknown
		try
		{
			return FormattableString.Invariant($"Bearer {_tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult().RawData}");
		}
		catch (IdentityServerTokenException innerException)
		{
			throw new CommunicationException("Failed to receive token from IDP", innerException);
		}
		catch (UnauthorizedException innerException2)
		{
			throw new MessageSecurityException("The credentials used to get a token is invalid.", innerException2);
		}
		catch (IdpException innerException3)
		{
			throw new CommunicationException("Failed to receive token from IDP", innerException3);
		}
		catch (HttpRequestException ex)
		{
			HttpRequestException ex2 = ex;
			if (((Exception)(object)ex2).InnerException is WebException)
			{
				WebException ex3 = ((Exception)(object)ex2).InnerException as WebException;
				if (ex3.Status == WebExceptionStatus.ConnectFailure || ex3.Status == WebExceptionStatus.NameResolutionFailure)
				{
					throw new EndpointNotFoundException("Could not find the token endpoint.", ex3);
				}
				throw new CommunicationException("Failed to receive token from IDP", ex3);
			}
			throw new CommunicationException("Failed to receive token from IDP", (Exception)(object)ex2);
		}
		catch (TaskCanceledException innerException4)
		{
			throw new CommunicationException("The request to get a token timed out.", innerException4);
		}
	}

	public void AfterReceiveReply(ref Message reply, object correlationState)
	{
	}
}

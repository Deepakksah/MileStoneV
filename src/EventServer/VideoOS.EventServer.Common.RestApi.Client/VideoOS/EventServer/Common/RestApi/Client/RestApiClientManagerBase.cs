using System;
using System.Collections.Generic;
using System.Net.Http;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.EventServer.Common.RestApi.Client;

public class RestApiClientManagerBase<TIClient, TClient> where TClient : IClientWithBearerToken, TIClient
{
	private readonly Dictionary<ServerId, TClient> _clients = new Dictionary<ServerId, TClient>();

	private readonly Func<string, HttpClient, TClient> _clientFactory;

	public RestApiClientManagerBase(Func<string, HttpClient, TClient> clientFactory)
	{
		_clientFactory = clientFactory ?? throw new ArgumentNullException("clientFactory");
	}

	public TIClient Get(ServerId serverId)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		if (_clients.TryGetValue(serverId, out var value))
		{
			return (TIClient)(object)value;
		}
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
		Uri uri = WellKnownInformation.GetApiGatewayUri(loginSettings) ?? throw new UriFormatException("The API Gateway URI is null.");
		TClient val = (_clients[serverId] = _clientFactory(uri.ToString(), new HttpClient()));
		TClient val3 = val;
		ref TClient reference = ref val3;
		val = default(TClient);
		if (val == null)
		{
			val = reference;
			reference = ref val;
		}
		Func<string> bearerTokenProvider = () => loginSettings.IdentityTokenCache.Token;
		reference.BearerTokenProvider = bearerTokenProvider;
		return (TIClient)(object)val3;
	}
}

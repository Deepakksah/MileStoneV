using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using VideoOS.EventServer.Common.RestApi.Client;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server.MIP.Communication.Client;

public class RestApiCommunicationManager<TInterface, TClass> where TClass : IClientWithBearerToken, TInterface
{
	private TClass _restClient;

	private DateTime _tokenTimeout = DateTime.MinValue;

	private readonly Func<string, HttpClient, TClass> _factory;

	private readonly ITokenCache _tokenCache;

	private readonly RegisteredServer _serverAddress;

	private readonly ILogger _logger;

	private readonly string _restApiPathExtension;

	private readonly object _clientLock = new object();

	public TInterface RestClient
	{
		get
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			lock (_clientLock)
			{
				if (_restClient == null && _serverAddress != null && _serverAddress.Addresses.Length != 0)
				{
					string text = _serverAddress.Addresses[0].URL + _restApiPathExtension;
					_restClient = _factory(text, new HttpClient());
					_tokenTimeout = DateTime.MinValue;
					_logger.LogInformation("RestClient created: " + text);
				}
				if (ShouldRefreshEventsClientToken())
				{
					JwtSecurityToken result = _tokenCache.RefreshToken().GetAwaiter().GetResult();
					_tokenTimeout = result.ValidTo;
					_restClient.BearerToken = result.RawData;
					_logger.LogInformation("RestClient token updated");
				}
				return (TInterface)(object)_restClient;
			}
		}
	}

	public RestApiCommunicationManager(Func<string, HttpClient, TClass> factory, ITokenCache tokenCache, RegisteredServer serverAddress, ILogger logger, string restApiPathExtension)
	{
		_factory = factory ?? throw new ArgumentNullException("factory");
		_tokenCache = tokenCache;
		_serverAddress = serverAddress;
		_logger = logger;
		_restApiPathExtension = restApiPathExtension;
	}

	private bool ShouldRefreshEventsClientToken()
	{
		if (_restClient == null)
		{
			return false;
		}
		if (_restClient.BearerToken == string.Empty)
		{
			return true;
		}
		if (_tokenTimeout == DateTime.MinValue)
		{
			return true;
		}
		if (_tokenTimeout.AddSeconds(-10.0) < DateTime.UtcNow)
		{
			return true;
		}
		return false;
	}
}

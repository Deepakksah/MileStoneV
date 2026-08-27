using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers;

public class JwtSecurityTokenHandlerWithConfigurationManager : JwtSecurityTokenHandler, IDisposable
{
	private static readonly TimeSpan DefaultRefreshInterval = new TimeSpan(1, 0, 0, 0);

	private static readonly TimeSpan DefaultMinimumRefreshInterval = new TimeSpan(0, 0, 0, 30);

	private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

	private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();

	private OpenIdConnectConfiguration _configuration;

	private DateTimeOffset _syncAfter = DateTimeOffset.MinValue;

	private DateTimeOffset _lastRefresh = DateTimeOffset.MinValue;

	public TimeSpan MinimumRefreshInterval { get; set; } = DefaultMinimumRefreshInterval;

	public TimeSpan RefreshInterval { get; set; } = DefaultRefreshInterval;

	public JwtSecurityTokenHandlerWithConfigurationManager(string address, bool requireHttps, bool delayConfigurationLoad)
		: this(null, address, requireHttps, delayConfigurationLoad)
	{
	}

	public JwtSecurityTokenHandlerWithConfigurationManager(IConfigurationManager<OpenIdConnectConfiguration> configurationManager, string address = null, bool requireHttps = true, bool delayConfigurationLoad = false)
	{
		_configurationManager = configurationManager ?? CreateConfigurationManager(address, requireHttps);
		if (!delayConfigurationLoad)
		{
			PrefetchConfiguration();
		}
	}

	public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
	{
		if (token == null)
		{
			throw new ArgumentNullException("token");
		}
		if (validationParameters == null)
		{
			throw new ArgumentNullException("validationParameters");
		}
		OpenIdConnectConfiguration openIdConnectConfiguration = GetOpenIdConnectConfiguration(prefetch: false);
		validationParameters.IssuerSigningKeys = ((BaseConfiguration)openIdConnectConfiguration).SigningKeys;
		validationParameters.ValidIssuer = ((BaseConfiguration)openIdConnectConfiguration).Issuer;
		try
		{
			return ((JwtSecurityTokenHandler)this).ValidateToken(token, validationParameters, ref validatedToken);
		}
		catch (SecurityTokenSignatureKeyNotFoundException)
		{
			DateTimeOffset utcNow = DateTimeOffset.UtcNow;
			if (utcNow >= DateTimeUtil.Add(_lastRefresh.UtcDateTime, MinimumRefreshInterval))
			{
				_sync.EnterWriteLock();
				try
				{
					_configurationManager.RequestRefresh();
					_syncAfter = utcNow;
				}
				finally
				{
					_sync.ExitWriteLock();
				}
			}
			throw;
		}
	}

	private void PrefetchConfiguration()
	{
		GetOpenIdConnectConfiguration(prefetch: true);
	}

	private OpenIdConnectConfiguration GetOpenIdConnectConfiguration(bool prefetch)
	{
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		if (_configuration != null && _syncAfter > utcNow)
		{
			return _configuration;
		}
		_sync.EnterWriteLock();
		try
		{
			if (_configuration != null && _syncAfter > utcNow)
			{
				return _configuration;
			}
			OpenIdConnectConfiguration result = (_configuration = Task.Run<OpenIdConnectConfiguration>(async () => await _configurationManager.GetConfigurationAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false)).Result);
			if (prefetch)
			{
				return result;
			}
			_syncAfter = DateTimeUtil.Add(utcNow.UtcDateTime, RefreshInterval);
			_lastRefresh = utcNow;
			return result;
		}
		finally
		{
			_sync.ExitWriteLock();
		}
	}

	private IConfigurationManager<OpenIdConnectConfiguration> CreateConfigurationManager(string idpBaseAddress, bool requireHttps)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Expected O, but got Unknown
		if (idpBaseAddress == null)
		{
			throw new ArgumentNullException("idpBaseAddress");
		}
		ConfigurationManager<OpenIdConnectConfiguration> obj = new ConfigurationManager<OpenIdConnectConfiguration>(idpBaseAddress.TrimEnd('/') + "/.well-known/openid-configuration", (IConfigurationRetriever<OpenIdConnectConfiguration>)new OpenIdConnectConfigurationRetriever(), (IDocumentRetriever)new HttpDocumentRetriever
		{
			RequireHttps = requireHttps
		});
		((BaseConfigurationManager)obj).AutomaticRefreshInterval = new TimeSpan(RefreshInterval.Ticks / 2);
		((BaseConfigurationManager)obj).RefreshInterval = MinimumRefreshInterval;
		return (IConfigurationManager<OpenIdConnectConfiguration>)(object)obj;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_sync?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

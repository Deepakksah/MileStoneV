using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Extensions;

namespace VideoOS.IdentityServer.Repositories;

public class ExternalProviderManager : IExternalProviderManager
{
	private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

	private readonly IPostConfigureOptions<OpenIdConnectOptions> _postConfigureOptions;

	private readonly IOptionsMonitorCache<OpenIdConnectOptions> _monitorCache;

	private readonly ILogger _logger;

	public ExternalProviderManager(IAuthenticationSchemeProvider authenticationSchemeProvider, IOptionsMonitorCache<OpenIdConnectOptions> monitorCache, IPostConfigureOptions<OpenIdConnectOptions> postConfigureOptions, ILogger<IExternalProviderManager> logger)
	{
		_authenticationSchemeProvider = authenticationSchemeProvider;
		_postConfigureOptions = postConfigureOptions;
		_monitorCache = monitorCache;
		_logger = logger;
	}

	public void AddProvider(LoginProvider loginProvider)
	{
		if (loginProvider == null)
		{
			throw new ArgumentNullException("loginProvider");
		}
		if (!loginProvider.Enabled)
		{
			_logger.LogInformation("Login provider '" + loginProvider.DisplayName + "' not configured because it is disabled.");
			return;
		}
		if (string.IsNullOrEmpty(loginProvider.ClientSecret))
		{
			_logger.LogError("Unable to configure the login provider '" + loginProvider.DisplayName + "' due to missing client secret.");
			return;
		}
		OpenIdConnectOptions options = loginProvider.BuildOptions(_logger);
		string name = loginProvider.Id.ToString();
		_postConfigureOptions.PostConfigure(name, options);
		_monitorCache.TryAdd(name, options);
		_authenticationSchemeProvider.AddScheme(new AuthenticationScheme(name, loginProvider.DisplayName, typeof(OpenIdConnectHandler)));
		_logger.LogInformation("Login provider '" + loginProvider.DisplayName + "' configured successfully.");
	}

	public void UpdateProvider(LoginProvider loginProvider)
	{
		if (loginProvider == null)
		{
			throw new ArgumentNullException("loginProvider");
		}
		string name = loginProvider.Id.ToString();
		if (!loginProvider.Enabled)
		{
			if (_authenticationSchemeProvider.SchemeExists(name))
			{
				_authenticationSchemeProvider.RemoveScheme(name);
				_monitorCache.TryRemove(name);
				_logger.LogInformation("Configuration of login provider '" + loginProvider.DisplayName + "' removed because it is disabled.");
			}
			return;
		}
		if (string.IsNullOrEmpty(loginProvider.ClientSecret))
		{
			if (_authenticationSchemeProvider.SchemeExists(name))
			{
				_authenticationSchemeProvider.RemoveScheme(name);
				_monitorCache.TryRemove(name);
				_logger.LogError("Configuration of login provider '" + loginProvider.DisplayName + "' removed due to missing client secret.");
			}
			else
			{
				_logger.LogError("Unable to configure the login provider '" + loginProvider.DisplayName + "' due to missing client secret.");
			}
			return;
		}
		bool flag = false;
		if (_authenticationSchemeProvider.SchemeExists(name))
		{
			_authenticationSchemeProvider.RemoveScheme(name);
			_monitorCache.TryRemove(name);
			flag = true;
		}
		OpenIdConnectOptions options = loginProvider.BuildOptions(_logger);
		_postConfigureOptions.PostConfigure(name, options);
		_monitorCache.TryAdd(name, options);
		_authenticationSchemeProvider.AddScheme(new AuthenticationScheme(name, loginProvider.DisplayName, typeof(OpenIdConnectHandler)));
		if (flag)
		{
			_logger.LogInformation("Configuration of login provider '" + loginProvider.DisplayName + "' updated successfully.");
		}
		else
		{
			_logger.LogInformation("Login provider '" + loginProvider.DisplayName + "' configured successfully.");
		}
	}

	public void RemoveProvider(string name, string displayName)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (displayName == null)
		{
			throw new ArgumentNullException("displayName");
		}
		if (_authenticationSchemeProvider.SchemeExists(name))
		{
			_authenticationSchemeProvider.RemoveScheme(name);
			_monitorCache.TryRemove(name);
			_logger.LogInformation("Configuration of login provider '" + displayName + "' successfully removed.");
		}
	}
}

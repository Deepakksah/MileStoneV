using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.Management.Server;

public class BasicAuthenticationModule : IBasicAuthenticationModule
{
	public static readonly string CachePrefix = "_cache_GeminiBasicAuthentication_";

	private Func<IGeminiRequestContext> _createGeminiRequestContext;

	public BasicAuthenticationModule()
		: this(() => GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext())
	{
	}

	public BasicAuthenticationModule(Func<IGeminiRequestContext> createGeminiRequestContext)
	{
		if (createGeminiRequestContext == null)
		{
			throw new ArgumentNullException("createGeminiRequestContext");
		}
		_createGeminiRequestContext = createGeminiRequestContext;
	}

	public bool AuthenticateUser(AuthenticationHeaderValue authenticationHeaderValue, out ClaimsPrincipal principal)
	{
		return AuthenticateUser(((object)authenticationHeaderValue).ToString(), out principal);
	}

	public bool AuthenticateUser(string authenticationHeaderValue, out ClaimsPrincipal principal)
	{
		string username = string.Empty;
		principal = null;
		if (!authenticationHeaderValue.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
		{
			new AuthenticationFailureEvent(this, username).Raise();
			return false;
		}
		if (!ExtractCredentials(authenticationHeaderValue, out username, out var password))
		{
			new AuthenticationFailureEvent(this, username).Raise();
			return false;
		}
		string[] array = username.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
		username = ((array.Length > 1) ? array[1] : username);
		if (IsCredentialCached(username, password, out principal))
		{
			new AuthenticationSuccessEvent(this, username).Raise();
			return true;
		}
		if (!ValidateUser(username, password, out principal))
		{
			return false;
		}
		CacheCredential(username, password, principal);
		new AuthenticationSuccessEvent(this, username).Raise();
		return true;
	}

	private bool ValidateUser(string username, string password, out ClaimsPrincipal principalOut)
	{
		principalOut = null;
		ClaimsPrincipal claimsPrincipal = null;
		try
		{
			IGeminiRequestContext geminiRequestContext = _createGeminiRequestContext();
			try
			{
				IIdpTokenService idpTokenService = geminiRequestContext.GetService<IdpTokenService, IIdpTokenService>();
				System.Threading.Tasks.Task.Run(async delegate
				{
					TokenResponse tokenResponse = await idpTokenService.ResourceOwnerAuthenticationAsync(username, password);
					claimsPrincipal = await idpTokenService.ValidateTokenAsync(tokenResponse?.Access_Token);
				}).GetAwaiter().GetResult();
				principalOut = claimsPrincipal;
			}
			finally
			{
				if (geminiRequestContext is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			if (ex is IdpException)
			{
				return false;
			}
			throw;
		}
		return true;
	}

	private static bool ExtractCredentials(string authenticationHeader, out string username, out string password)
	{
		username = string.Empty;
		password = string.Empty;
		string text = authenticationHeader.Substring(6).Trim();
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		Encoding encoding = Encoding.GetEncoding("iso-8859-1");
		string text2;
		try
		{
			text2 = encoding.GetString(Convert.FromBase64String(text));
		}
		catch (FormatException)
		{
			return false;
		}
		int num = text2.IndexOf(':');
		username = text2.Substring(0, num);
		password = text2.Substring(num + 1);
		return true;
	}

	private bool IsCredentialCached(string username, string password, out ClaimsPrincipal claimsPrincipal)
	{
		claimsPrincipal = null;
		string identifier = GetIdentifier(username.ToUpperInvariant(), password);
		KeyValuePair<string, ClaimsPrincipal>? keyValuePair = HttpContextManager.Current.Cache[CachePrefix + username.ToUpperInvariant()] as KeyValuePair<string, ClaimsPrincipal>?;
		if (!keyValuePair.HasValue || keyValuePair.Value.Key != identifier)
		{
			new CredentialCacheMissEvent(this, username).Raise();
			return false;
		}
		claimsPrincipal = keyValuePair.Value.Value;
		new CredentialCacheHitEvent(this, username).Raise();
		return true;
	}

	private void CacheCredential(string username, string password, ClaimsPrincipal claimsPrincipal)
	{
		if (username == null)
		{
			throw new ArgumentNullException("username");
		}
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claimsPrincipal.Claims.Single((System.Security.Claims.Claim c) => c.Type == "exp").Value, CultureInfo.InvariantCulture));
		KeyValuePair<string, ClaimsPrincipal> keyValuePair = new KeyValuePair<string, ClaimsPrincipal>(GetIdentifier(username.ToUpperInvariant(), password), claimsPrincipal);
		try
		{
			HttpContextManager.Current.Cache.Insert(CachePrefix + username.ToUpperInvariant(), keyValuePair, null, dateTimeOffset.UtcDateTime, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
		}
		catch (ArgumentOutOfRangeException exception)
		{
			new CredentialCacheAddErrorEvent(this, username, exception).Raise();
		}
		catch (ArgumentException exception2)
		{
			new CredentialCacheAddErrorEvent(this, username, exception2).Raise();
		}
		new CredentialCacheAddEvent(this, username).Raise();
	}

	private static string GetIdentifier(string username, string password)
	{
		byte[] inArray;
		using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create())
		{
			string s = username + password;
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			inArray = hashAlgorithm.ComputeHash(bytes);
		}
		return CachePrefix + Convert.ToBase64String(inArray);
	}
}

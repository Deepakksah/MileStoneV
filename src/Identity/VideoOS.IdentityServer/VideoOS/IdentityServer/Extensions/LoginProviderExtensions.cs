using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Services;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Extensions;

public static class LoginProviderExtensions
{
	private static readonly Action<ILogger, string, object, string, Exception> _logTokenError = LoggerMessage.Define<string, object, string>(LogLevel.Error, 0, "Token request to {TokenEndpoint} failed with status code {StatusCode} and content {Content}");

	public static OpenIdConnectOptions BuildOptions(this LoginProvider loginProvider, ILogger logger)
	{
		OpenIdConnectOptions openIdConnectOptions = new OpenIdConnectOptions
		{
			RequireHttpsMetadata = true,
			SignInScheme = IdentityConstants.ExternalScheme,
			Prompt = (loginProvider.PromptForLogin ? "login" : null),
			SignOutScheme = IdentityConstants.ApplicationScheme,
			Authority = loginProvider.Authority,
			CallbackPath = loginProvider.CallBackPath,
			ClientId = loginProvider.ClientId,
			ClientSecret = ((loginProvider.ClientSecretType == "SharedSecret") ? loginProvider.ClientSecret : null),
			ResponseType = "code",
			ResponseMode = "query",
			GetClaimsFromUserInfoEndpoint = true
		};
		openIdConnectOptions.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash", "auth_time", "jti", "ver", "at_hash", "updated_at");
		openIdConnectOptions.Scope.Clear();
		openIdConnectOptions.Scope.Add("openid");
		foreach (string item in ScopesAsEnumerable(loginProvider))
		{
			openIdConnectOptions.Scope.Add(item);
		}
		if (loginProvider.ClientSecretType == "X509Thumbprint")
		{
			openIdConnectOptions.Events = new OpenIdConnectEvents
			{
				OnAuthorizationCodeReceived = (AuthorizationCodeReceivedContext context) => OnAuthorizationCodeReceivedAsync(context, loginProvider, logger, new CertificateStore())
			};
		}
		return openIdConnectOptions;
	}

	public static async Task OnAuthorizationCodeReceivedAsync(AuthorizationCodeReceivedContext context, LoginProvider loginProvider, ILogger logger, ICertificateStore certificateStore)
	{
		context.HandleCodeRedemption();
		IHttpClientFactory httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
		string tokenEndpoint = await GetTokenEndpointAsync(context.Options.Authority, httpClientFactory);
		X509Certificate2 certificate = LoadCertificateByThumbprint(loginProvider.ClientSecret, certificateStore);
		string value = CreateClientAssertionJwt(context.Options.ClientId, tokenEndpoint, certificate);
		string value2 = context.ProtocolMessage.RedirectUri;
		if (string.IsNullOrEmpty(value2))
		{
			context.TokenEndpointRequest?.Parameters.TryGetValue("redirect_uri", out value2);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "grant_type", "authorization_code" },
			{
				"code",
				context.ProtocolMessage.Code
			},
			{ "redirect_uri", value2 },
			{
				"client_id",
				context.Options.ClientId
			},
			{ "client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
			{ "client_assertion", value }
		};
		string verifierParam = default(string);
		if (context.TokenEndpointRequest?.Parameters.TryGetValue("code_verifier", out verifierParam) ?? false)
		{
			dictionary.Add("code_verifier", verifierParam);
		}
		HttpClient httpClient = httpClientFactory.CreateClient("externalProvider");
		using FormUrlEncodedContent content = new FormUrlEncodedContent(dictionary);
		HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(tokenEndpoint, content);
		if (httpResponseMessage.IsSuccessStatusCode)
		{
			JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(await httpResponseMessage.Content.ReadAsStringAsync());
			string accessToken = null;
			string idToken = null;
			if (jsonElement.TryGetProperty("access_token", out var value3))
			{
				accessToken = value3.GetString();
			}
			if (jsonElement.TryGetProperty("id_token", out var value4))
			{
				idToken = value4.GetString();
			}
			context.HandleCodeRedemption(accessToken, idToken);
		}
		else
		{
			string arg = tokenEndpoint;
			object arg2 = httpResponseMessage.StatusCode;
			string arg3 = await (httpResponseMessage?.Content?.ReadAsStringAsync());
			_logTokenError(logger, arg, arg2, arg3, null);
		}
	}

	public static IEnumerable<string> ScopesAsEnumerable(LoginProvider loginProvider)
	{
		IEnumerable<string> result = Enumerable.Empty<string>();
		try
		{
			if (!string.IsNullOrEmpty(loginProvider.Scopes))
			{
				result = JsonConvert.DeserializeObject<List<string>>(loginProvider.Scopes);
			}
			return result;
		}
		catch (JsonReaderException inner)
		{
			throw new ScopesFormatInvalidException("The 'scopes' value is invalid for the login provider.", inner);
		}
	}

	public static X509Certificate2 LoadCertificateByThumbprint(string thumbprint, ICertificateStore certificateStore)
	{
		X509Certificate2 x509Certificate = certificateStore.FindByThumbprint(thumbprint);
		if (x509Certificate == null)
		{
			throw new InvalidOperationException("Certificate with thumbprint " + thumbprint + " not found.");
		}
		return x509Certificate;
	}

	public static string CreateClientAssertionJwt(string clientId, string tokenEndpoint, X509Certificate2 certificate)
	{
		RSA rSAPrivateKey = certificate.GetRSAPrivateKey();
		ECDsa eCDsaPrivateKey = certificate.GetECDsaPrivateKey();
		if (rSAPrivateKey == null && eCDsaPrivateKey == null)
		{
			throw new NotSupportedException("The provided certificate does not contain a supported private key. Supported algorithms are RS256 (RSA) and ES256/ES384/ES512 (ECDsa). Please provide a certificate with an RSA or ECDsa key for JWT signing.");
		}
		DateTime utcNow = DateTime.UtcNow;
		JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
		SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
		securityTokenDescriptor.Issuer = clientId;
		securityTokenDescriptor.Audience = tokenEndpoint;
		securityTokenDescriptor.Subject = new ClaimsIdentity(new Claim[1]
		{
			new Claim("sub", clientId)
		});
		securityTokenDescriptor.Expires = utcNow.AddMinutes(5.0);
		securityTokenDescriptor.SigningCredentials = ((rSAPrivateKey != null) ? new X509SigningCredentials(certificate, "RS256") : new SigningCredentials(new ECDsaSecurityKey(eCDsaPrivateKey), GetEcdsaAlgorithm(eCDsaPrivateKey)));
		SecurityTokenDescriptor tokenDescriptor = securityTokenDescriptor;
		SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
		return jwtSecurityTokenHandler.WriteToken(token);
	}

	private static string GetEcdsaAlgorithm(ECDsa ecdsaKey)
	{
		int keySize = ecdsaKey.KeySize;
		if (keySize < 512)
		{
			if (keySize >= 384)
			{
				return "ES384";
			}
			return "ES256";
		}
		return "ES512";
	}

	public static async Task<string> GetTokenEndpointAsync(string authority, IHttpClientFactory httpClientFactory)
	{
		string requestUri = authority.TrimEnd('/') + "/.well-known/openid-configuration";
		HttpClient httpClient = httpClientFactory.CreateClient("externalProvider");
		HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
		httpResponseMessage.EnsureSuccessStatusCode();
		using JsonDocument jsonDocument = JsonDocument.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
		if (jsonDocument.RootElement.TryGetProperty("token_endpoint", out var value))
		{
			return value.GetString();
		}
		throw new InvalidOperationException("token_endpoint not found in openid-configuration.");
	}
}

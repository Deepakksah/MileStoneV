using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VideoOS.Common.Integration.WebApi;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Server.RecorderCommunication;

public sealed class ServerAccessTokenHandler : IAccessTokenHandler, IDisposable
{
	private readonly ITokenCache _tokenCache;

	private bool disposed;

	public ServerAccessTokenHandler(ITokenCache serverTokenCache)
	{
		if (serverTokenCache == null)
		{
			throw new ArgumentNullException("serverTokenCache");
		}
		_tokenCache = serverTokenCache;
	}

	public async Task PopulateAuthorizationHeaderAsync(HttpRequestMessage httpRequestMessage)
	{
		if (httpRequestMessage == null)
		{
			throw new ArgumentNullException("httpRequestMessage");
		}
		try
		{
			JwtSecurityToken jwtSecurityToken = await _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false);
			httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtSecurityToken.RawData);
		}
		catch (IdpException ex)
		{
			if (ex.StatusCode == 400)
			{
				throw new AccessTokenException(ex.Message, ex);
			}
			throw WebApiExceptionHelper.CreateResponseException(httpRequestMessage.RequestUri?.ToString(), (HttpStatusCode)ex.StatusCode, ex);
		}
		catch (IdentityServerTokenException ex2)
		{
			throw new AccessTokenException(ex2.Message, ex2);
		}
	}

	public async Task RestartAsync()
	{
		await _tokenCache.Stop();
		await _tokenCache.Start();
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				_tokenCache?.Dispose();
			}
			disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

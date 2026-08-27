using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Utilities.Telemetry;

public class TokenCacheDelegatingHandler : DelegatingHandler
{
	private readonly Func<CancellationToken, Task<JwtSecurityToken>> _tokenProvider;

	public TokenCacheDelegatingHandler(Func<CancellationToken, Task<JwtSecurityToken>> tokenProvider, HttpMessageHandler innerHandler = null)
		: base((HttpMessageHandler)(((object)innerHandler) ?? ((object)new HttpClientHandler())))
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_tokenProvider = tokenProvider ?? throw new ArgumentNullException("tokenProvider");
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		JwtSecurityToken val = await _tokenProvider(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", val.RawData);
		return await _003C_003En__0(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _003C_003En__0(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return ((DelegatingHandler)this).SendAsync(request, cancellationToken);
	}
}

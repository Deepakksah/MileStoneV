using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.IM.Common.Service.Builders.HttpRequestMessages;
using VideoOS.IM.Common.Service.ExternalAgents;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.OidcTokens;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.ExternalAgents;

public sealed class WebApiClient : IWebApiClient
{
	private static readonly HttpClient Client = new HttpClient();

	private static readonly Lazy<IWebApiClient> LazyInstance = new Lazy<IWebApiClient>(() => new WebApiClient());

	public static IWebApiClient Instance => LazyInstance.Value;

	private WebApiClient()
	{
	}

	public async Task<TResponse> ExecuteGetRequestAsync<TResponse>([NotNull] Uri apiHostName, [NotNull] string path, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : class
	{
		apiHostName.NullCheck("", "ExecuteGetRequestAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 30);
		path.NullCheck("", "ExecuteGetRequestAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 31);
		return await SendRequestAsync<TResponse>(await BuildHttpRequestMessageAsync(apiHostName, path, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public Task<TResponse> ExecutePostRequestAsync<TRequestBody, TResponse>([NotNull] Uri apiHostName, [NotNull] string path, [CanBeNull] TRequestBody requestBody = null, CancellationToken cancellationToken = default(CancellationToken)) where TRequestBody : class where TResponse : class
	{
		throw new NotImplementedException();
	}

	public Task<TResponse> ExecutePutRequestAsync<TRequestBody, TResponse>([NotNull] Uri apiHostName, [NotNull] string path, [CanBeNull] TRequestBody requestBody = null, CancellationToken cancellationToken = default(CancellationToken)) where TRequestBody : class where TResponse : class
	{
		throw new NotImplementedException();
	}

	public Task<TResponse> ExecuteDeleteRequestAsync<TResponse>([NotNull] Uri apiHostName, [NotNull] string path, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : class
	{
		throw new NotImplementedException();
	}

	private static async Task<HttpRequestMessage> BuildHttpRequestMessageAsync([NotNull] Uri apiHostName, [NotNull] string path, [NotNull] HttpMethod httpMethod, [CanBeNull] object requestBody = null)
	{
		apiHostName.NullCheck("", "BuildHttpRequestMessageAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 70);
		path.NullCheck("", "BuildHttpRequestMessageAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 71);
		httpMethod.NullCheck<HttpMethod>("", "BuildHttpRequestMessageAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 72);
		string accessToken = await GetIncidentManagerS2SAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
		IHttpRequestMessageBuilder httpRequestMessageBuilder = new HttpRequestMessageBuilder().WithHttpUri(new Uri(apiHostName, path)).WithHttpMethod(httpMethod).WithBearerToken(accessToken);
		if (requestBody == null)
		{
			return httpRequestMessageBuilder.Build();
		}
		httpRequestMessageBuilder.WithHttpContent(requestBody);
		return httpRequestMessageBuilder.Build();
	}

	private static async Task<TResponse> SendRequestAsync<TResponse>([NotNull] HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : class
	{
		httpRequestMessage.NullCheck<HttpRequestMessage>("", "SendRequestAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 97);
		HttpResponseMessage val = await ((HttpMessageInvoker)Client).SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (val.StatusCode != HttpStatusCode.OK)
		{
			string message = $"Server returned status code '{val.StatusCode}' with reason phrase '{val.ReasonPhrase}'";
			if (val.StatusCode == HttpStatusCode.Forbidden)
			{
				throw new UnauthorizedAccessException(message);
			}
			throw new InvalidOperationException(message);
		}
		return DeserializeResponse<TResponse>(await val.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
	}

	private static TResponse DeserializeResponse<TResponse>([NotNull] string webApiContentResponse) where TResponse : class
	{
		webApiContentResponse.NullCheck("", "DeserializeResponse", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\ExternalAgents\\WebApiClient.cs", 125);
		return JsonConvert.DeserializeObject<TResponse>(webApiContentResponse);
	}

	private static async Task<string> GetIncidentManagerS2SAccessTokenAsync()
	{
		return await S2STokenService.Instance.GetAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
	}
}

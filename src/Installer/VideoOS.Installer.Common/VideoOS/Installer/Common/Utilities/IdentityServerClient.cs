using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public static class IdentityServerClient
{
	public static async Task<AccessTokenResponse> GetTokenAsync(Uri identityServerAddress, ICredentials credentials)
	{
		HttpClientHandler clientHandler = CreateClientHandler(credentials);
		try
		{
			HttpClient client = new HttpClient((HttpMessageHandler)(object)clientHandler);
			try
			{
				client.BaseAddress = identityServerAddress;
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				AccessTokenResponse accessTokenResponse = null;
				HttpContent content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("grant_type", "windows_credentials"),
					new KeyValuePair<string, string>("client_id", "GrantValidatorClient")
				});
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "connect/token")
				{
					Content = content
				};
				try
				{
					HttpResponseMessage val = await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
					if (val.IsSuccessStatusCode)
					{
						accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(await val.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
					}
					else
					{
						LogManager.Write($"Failed response status code = {val.StatusCode}, Reason =  {val.ReasonPhrase}");
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
				return accessTokenResponse;
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)clientHandler)?.Dispose();
		}
	}

	private static HttpClientHandler CreateClientHandler(ICredentials credentials = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		return new HttpClientHandler
		{
			Credentials = credentials,
			UseDefaultCredentials = (credentials == null)
		};
	}
}

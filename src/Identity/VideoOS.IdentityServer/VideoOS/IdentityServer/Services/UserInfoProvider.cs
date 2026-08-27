using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class UserInfoProvider : IUserInfoProvider
{
	private readonly IHttpClientFactory _clientFactory;

	public UserInfoProvider(IHttpClientFactory clientFactory)
	{
		_clientFactory = clientFactory;
	}

	public async Task<UserInfoResponse> GetUserInformation(string subjectToken, DiscoveryDocumentResponse discoResponse)
	{
		HttpClient client = _clientFactory.CreateClient();
		UserInfoRequest request = new UserInfoRequest
		{
			RequestUri = new Uri(discoResponse.UserInfoEndpoint),
			Token = subjectToken
		};
		return await client.GetUserInfoAsync(request);
	}
}

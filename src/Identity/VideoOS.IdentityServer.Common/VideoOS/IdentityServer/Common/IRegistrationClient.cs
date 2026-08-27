using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public interface IRegistrationClient : IDisposable
{
	Task<ClientGetModel> GetClient(string accessToken, string clientId);

	Task<ClientGetModel> GetClient(string clientId);

	Task<ClientGetModel[]> GetClients(string accessToken);

	Task<ClientGetModel[]> GetClients();

	Task<RegisterClientResponse> RegisterClient(string clientId, string clientName, IEnumerable<string> scopes);

	Task<RegisterClientResponse> RegisterClient(string accessToken, string clientId, string clientName, IEnumerable<string> scopes);

	Task<RegisterClientResponse> RegisterAuthorizationCodeClient(string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris);

	Task<RegisterClientResponse> RegisterAuthorizationCodeClient(string accessToken, string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris);

	Task<RegisterClientResponse> RegisterClientCredentialsAndAuthCodeClient(string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris, IEnumerable<string> postLogoutRedirectUris);

	Task<RegisterClientResponse> RegisterClientCredentialsAndAuthCodeClient(string accessToken, string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris, IEnumerable<string> postLogoutRedirectUris);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Model.Requests;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public class IdpAdministrationClient : IdpClientBase, IIdpAdministrationClient, IDisposable
{
	public IdpAdministrationClient(Uri baseAddress, IHttpClient client)
		: base(baseAddress, client)
	{
	}

	public IdpAdministrationClient(Uri baseAddress)
		: base(baseAddress, new HttpClientFactory())
	{
	}

	public Task<GetUserModel> GetUser(string accessToken, string id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Value cannot be null or empty.", "id");
		}
		return RequestIdp<GetUserModel>(accessToken, HttpMethod.Get, $"api/users/{id}");
	}

	public Task<GetUserModel[]> GetUsers(string accessToken)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp<GetUserModel[]>(accessToken, HttpMethod.Get, $"api/users");
	}

	public async Task<GetUserModel> GetUserByName(string accessToken, string username)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(username))
		{
			throw new ArgumentException("Value cannot be null or empty.", "username");
		}
		return (await RequestIdp<IEnumerable<GetUserModel>>(accessToken, HttpMethod.Get, $"api/users?username={username}").ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
	}

	public Task<GetUserModel[]> GetUsersAfterModifiedDate(string accessToken, long unixTime)
	{
		if (accessToken == null)
		{
			throw new ArgumentNullException("accessToken");
		}
		if (unixTime <= 0)
		{
			throw new ArgumentOutOfRangeException("unixTime");
		}
		DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
		return GetUsersAfterModifiedDate(accessToken, dateTime);
	}

	public async Task<GetUserModel[]> GetUsersAfterModifiedDate(string accessToken, DateTime modifiedDate)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return await RequestIdp<GetUserModel[]>(accessToken, HttpMethod.Get, FormattableStringFactory.Create("api/users?filter=ModifiedDate gt {0}", modifiedDate.ToString("O"))).ConfigureAwait(continueOnCapturedContext: false);
	}

	public Task<GetUserModel> CreateUser(string accessToken, CreateUserModel createUserModel)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (createUserModel == null)
		{
			throw new ArgumentNullException("createUserModel");
		}
		return RequestIdp<GetUserModel>(accessToken, HttpMethod.Post, $"api/users", createUserModel);
	}

	public Task<GetUserModel> UpdateUser(string accessToken, string id, UpdateUserModel updateUserModel)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Value cannot be null or empty.", "id");
		}
		if (updateUserModel == null)
		{
			throw new ArgumentNullException("updateUserModel");
		}
		return RequestIdp<GetUserModel>(accessToken, new HttpMethod("PATCH"), $"api/users/{id}", updateUserModel);
	}

	public Task DeleteUser(string accessToken, string id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Value cannot be null or empty.", "id");
		}
		return RequestIdp(accessToken, HttpMethod.Delete, $"api/users/{id}");
	}

	public async Task<GetUserModel[]> GetUserCollection(string accessToken, ICollection<string> ids)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (ids == null)
		{
			throw new ArgumentNullException("ids");
		}
		if (!ids.Any())
		{
			throw new ArgumentException("Must contain ids to query with.", "ids");
		}
		string text = string.Join(",", ids.Select(HttpUtility.UrlEncode));
		if ("ids=".Length + text.Length <= 2048)
		{
			return await RequestIdp<GetUserModel[]>(accessToken, HttpMethod.Get, FormattableStringFactory.Create("api/user-collections/?ids=" + text)).ConfigureAwait(continueOnCapturedContext: false);
		}
		GetUserModel[] source = await GetUsers(accessToken).ConfigureAwait(continueOnCapturedContext: false);
		HashSet<string> hashset = new HashSet<string>(ids);
		return source.Where((GetUserModel u) => hashset.Contains(u.Id)).ToArray();
	}

	public Task<GetUserModel[]> CreateUserCollection(string accessToken, ICollection<CreateUserModel> createUserModels)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (createUserModels == null)
		{
			throw new ArgumentNullException("createUserModels");
		}
		return RequestIdp<GetUserModel[]>(accessToken, HttpMethod.Post, $"api/user-collections", createUserModels);
	}

	public Task CreateAdmin(string accessToken, CreateAdminModel createAdminModel)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (createAdminModel == null)
		{
			throw new ArgumentNullException("createAdminModel");
		}
		return RequestIdp(accessToken, HttpMethod.Post, $"api/adminidentities", createAdminModel);
	}

	public Task DeleteAdmin(string accessToken, string id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Value cannot be null or empty.", "id");
		}
		return RequestIdp(accessToken, HttpMethod.Delete, $"api/adminidentities/{id}");
	}

	public async Task<GetLoginModel> GetLogin(string accessToken, string provider, string key)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(provider))
		{
			throw new ArgumentException("Value cannot be null or empty.", "provider");
		}
		if (string.IsNullOrEmpty(key))
		{
			throw new ArgumentException("Value cannot be null or empty.", "key");
		}
		return (await RequestIdp<IEnumerable<GetLoginModel>>(accessToken, HttpMethod.Get, $"api/logins?loginproviderId={provider}&providerkey={key}").ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
	}

	public Task<GetLoginModel> CreateLogin(string accessToken, CreateLoginModel createLoginModel)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (createLoginModel == null)
		{
			throw new ArgumentNullException("createLoginModel");
		}
		return RequestIdp<GetLoginModel>(accessToken, HttpMethod.Post, $"api/logins", createLoginModel);
	}

	public Task DeleteLogin(string accessToken, string provider, string key)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(provider))
		{
			throw new ArgumentException("Value cannot be null or empty.", "provider");
		}
		if (string.IsNullOrEmpty(key))
		{
			throw new ArgumentException("Value cannot be null or empty.", "key");
		}
		return RequestIdp(accessToken, HttpMethod.Delete, $"api/logins?loginproviderId={provider}&providerkey={key}");
	}

	public Task<LoginProviderGetModel> GetLoginProvider(string accessToken, Guid id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp<LoginProviderGetModel>(accessToken, HttpMethod.Get, $"api/loginproviders/{id}");
	}

	public Task<LoginProviderGetModel[]> GetLoginProviders(string accessToken)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp<LoginProviderGetModel[]>(accessToken, HttpMethod.Get, $"api/loginproviders");
	}

	public Task<LoginProviderGetModel> CreateLoginProvider(string accessToken, LoginProviderCreateModel createModel)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (createModel == null)
		{
			throw new ArgumentNullException("createModel");
		}
		return RequestIdp<LoginProviderGetModel>(accessToken, HttpMethod.Post, $"api/loginproviders", createModel);
	}

	public Task<LoginProviderGetModel> UpdateLoginProvider(string accessToken, Guid id, LoginProviderUpdateModel updateModel)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (updateModel == null)
		{
			throw new ArgumentNullException("updateModel");
		}
		return RequestIdp<LoginProviderGetModel>(accessToken, new HttpMethod("PATCH"), $"api/loginproviders/{id.ToString()}", updateModel);
	}

	public Task DeleteLoginProvider(string accessToken, Guid id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp(accessToken, HttpMethod.Delete, $"api/loginproviders/{id}");
	}

	public Task<TrustedIssuerResponseModel> GetTrustedIssuer(string accessToken, int id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp<TrustedIssuerResponseModel>(accessToken, HttpMethod.Get, $"api/TrustedIssuers/{id}");
	}

	public Task<TrustedIssuerResponseModel[]> GetTrustedIssuers(string accessToken)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp<TrustedIssuerResponseModel[]>(accessToken, HttpMethod.Get, $"api/TrustedIssuers");
	}

	public Task<TrustedIssuerResponseModel> CreateTrustedIssuer(string accessToken, TrustedIssuerCreateModel createModel)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (createModel == null)
		{
			throw new ArgumentNullException("createModel");
		}
		return RequestIdp<TrustedIssuerResponseModel>(accessToken, HttpMethod.Post, $"api/TrustedIssuers", createModel);
	}

	public Task<TrustedIssuerResponseModel> UpdateTrustedIssuer(string accessToken, int id, TrustedIssuerUpdateModel updateModel)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (updateModel == null)
		{
			throw new ArgumentNullException("updateModel");
		}
		return RequestIdp<TrustedIssuerResponseModel>(accessToken, HttpMethod.Put, $"api/TrustedIssuers/{id}", updateModel);
	}

	public Task DeleteTrustedIssuer(string accessToken, int id)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return RequestIdp(accessToken, HttpMethod.Delete, $"api/TrustedIssuers/{id}");
	}

	public Task<IEnumerable<EventLogModel>> GetEventLogs(string accessToken, long idGreaterThan, int numberToReturn)
	{
		if (accessToken == null)
		{
			throw new ArgumentNullException("accessToken");
		}
		if (numberToReturn <= 0)
		{
			throw new ArgumentOutOfRangeException("numberToReturn");
		}
		return RequestIdp<IEnumerable<EventLogModel>>(accessToken, HttpMethod.Get, $"api/EventLogs?filter=id gt {idGreaterThan}&skip=0&top={numberToReturn}");
	}

	private async Task RequestIdp(string accessToken, HttpMethod httpMethod, FormattableString path, object model = null)
	{
		HttpRequestMessage val = new HttpRequestMessage(httpMethod, BuildFullUri(path));
		val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		if (model != null)
		{
			val.Content = (HttpContent)new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
		}
		await Send(val).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<TResponse> RequestIdp<TResponse>(string accessToken, HttpMethod httpMethod, FormattableString path, object model = null)
	{
		HttpRequestMessage val = new HttpRequestMessage(httpMethod, BuildFullUri(path));
		val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		if (model != null)
		{
			string text = ((!(model is PatchDocumentBase patchDocumentBase)) ? JsonConvert.SerializeObject(model) : patchDocumentBase.GetPatchDocument(JObject.FromObject(model)));
			val.Content = (HttpContent)new StringContent(text, Encoding.UTF8, "application/json");
		}
		return await SendAndReadContent<TResponse>(val).ConfigureAwait(continueOnCapturedContext: false);
	}
}

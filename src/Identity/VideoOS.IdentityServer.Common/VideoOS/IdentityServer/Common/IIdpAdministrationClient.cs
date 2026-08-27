using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common.Model.Requests;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public interface IIdpAdministrationClient : IDisposable
{
	Task<GetUserModel> GetUser(string accessToken, string id);

	Task<GetUserModel[]> GetUsers(string accessToken);

	Task<GetUserModel> GetUserByName(string accessToken, string username);

	Task<GetUserModel> CreateUser(string accessToken, CreateUserModel createUserModel);

	Task<GetUserModel> UpdateUser(string accessToken, string id, UpdateUserModel updateUserModel);

	Task DeleteUser(string accessToken, string id);

	Task CreateAdmin(string accessToken, CreateAdminModel createAdminModel);

	Task DeleteAdmin(string accessToken, string id);

	Task<GetUserModel[]> GetUserCollection(string accessToken, ICollection<string> ids);

	Task<GetUserModel[]> CreateUserCollection(string accessToken, ICollection<CreateUserModel> createUserModels);

	Task<GetLoginModel> GetLogin(string accessToken, string provider, string key);

	Task<GetLoginModel> CreateLogin(string accessToken, CreateLoginModel createLoginModel);

	Task DeleteLogin(string accessToken, string provider, string key);

	Task<IEnumerable<EventLogModel>> GetEventLogs(string accessToken, long idGreaterThan, int numberToReturn);

	Task<GetUserModel[]> GetUsersAfterModifiedDate(string accessToken, long unixTime);

	Task<GetUserModel[]> GetUsersAfterModifiedDate(string accessToken, DateTime modifiedDate);

	Task<LoginProviderGetModel> GetLoginProvider(string accessToken, Guid id);

	Task<LoginProviderGetModel[]> GetLoginProviders(string accessToken);

	Task<LoginProviderGetModel> CreateLoginProvider(string accessToken, LoginProviderCreateModel createModel);

	Task<LoginProviderGetModel> UpdateLoginProvider(string accessToken, Guid id, LoginProviderUpdateModel updateModel);

	Task DeleteLoginProvider(string accessToken, Guid id);

	Task<TrustedIssuerResponseModel> GetTrustedIssuer(string accessToken, int id);

	Task<TrustedIssuerResponseModel[]> GetTrustedIssuers(string accessToken);

	Task<TrustedIssuerResponseModel> CreateTrustedIssuer(string accessToken, TrustedIssuerCreateModel createModel);

	Task<TrustedIssuerResponseModel> UpdateTrustedIssuer(string accessToken, int id, TrustedIssuerUpdateModel updateModel);

	Task DeleteTrustedIssuer(string accessToken, int id);
}

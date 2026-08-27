using System.Threading.Tasks;
using IdentityModel.Client;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface IUserInfoProvider
{
	Task<UserInfoResponse> GetUserInformation(string subjectToken, DiscoveryDocumentResponse discoResponse);
}

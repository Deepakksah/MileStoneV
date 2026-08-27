using System.Net.Http;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.WebApi;

public interface IAccessTokenHandler
{
	Task PopulateAuthorizationHeaderAsync(HttpRequestMessage httpRequestMessage);

	Task RestartAsync();
}

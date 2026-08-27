namespace VideoOS.Mobile.ReverseProxy.Authorization;

public interface ITokenProvider
{
	string GetOAuthToken(string connectionId);
}

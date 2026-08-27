namespace VideoOS.Management.VmoClient;

public interface ISiteConnectionInfoTokenExchange : ISiteConnectionInfoBase
{
	string TrustedJwtAccessToken { get; }
}

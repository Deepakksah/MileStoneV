namespace VideoOS.Server;

public interface IServerEnterpriseServerConnector
{
	void ValidateToken(string token, string userHostAddress, out string userName, out string password);
}

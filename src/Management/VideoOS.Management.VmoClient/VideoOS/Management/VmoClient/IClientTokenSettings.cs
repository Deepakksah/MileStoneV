namespace VideoOS.Management.VmoClient;

public interface IClientTokenSettings
{
	string ClientId { get; }

	string GetCalculatedClientId(ServerVersion serverVersion);
}

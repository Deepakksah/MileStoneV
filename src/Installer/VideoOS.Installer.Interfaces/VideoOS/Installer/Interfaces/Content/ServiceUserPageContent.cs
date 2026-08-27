using System.Security;

namespace VideoOS.Installer.Interfaces.Content;

public class ServiceUserPageContent
{
	public string ServiceName { get; }

	public string UserName { get; set; }

	public SecureString Password { get; set; }

	public int Rank { get; }

	public ServiceUserPageContent(string serviceName, string userName, SecureString password, int rank)
	{
		ServiceName = serviceName;
		UserName = userName;
		Password = password;
		Rank = rank;
	}

	public virtual bool Validate()
	{
		return !string.IsNullOrEmpty(UserName);
	}
}

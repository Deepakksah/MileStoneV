using System.Net;

namespace VideoOS.Management.VmoClient;

public class CredentialRetriever
{
	public bool ConnectAsUser { get; }

	public string UserName { get; }

	public string Password { get; }

	public CredentialRetriever(ICredentials networkCredential)
	{
		if (networkCredential != null)
		{
			NetworkCredential credential = networkCredential.GetCredential(null, null);
			UserName = GetUserDomainNameOrEmpty(credential);
			Password = credential.Password;
			if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
			{
				ConnectAsUser = true;
			}
		}
	}

	public static string GetUserDomainNameOrEmpty(NetworkCredential networkCredential)
	{
		if (networkCredential == null)
		{
			return string.Empty;
		}
		if (!string.IsNullOrEmpty(networkCredential.Domain) && !string.IsNullOrEmpty(networkCredential.UserName))
		{
			return networkCredential.Domain + "\\" + networkCredential.UserName;
		}
		return networkCredential.UserName;
	}
}

using System;
using System.Net;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

[Serializable]
public sealed class CredentialContainer
{
	public string UserName { get; private set; }

	public string Domain { get; private set; }

	private byte[] EncryptedPassword { get; set; }

	private CredentialContainer(NetworkCredential networkCredential)
	{
		UserName = networkCredential.UserName;
		EncryptedPassword = networkCredential.SecurePassword.EncryptCredentialPassword();
		Domain = networkCredential.Domain;
	}

	private NetworkCredential GetNetworkCredential()
	{
		return new NetworkCredential
		{
			UserName = UserName,
			SecurePassword = EncryptedPassword.DecryptCredentialPassword(),
			Domain = Domain
		};
	}

	public static implicit operator NetworkCredential(CredentialContainer credential)
	{
		return credential?.GetNetworkCredential();
	}

	public static implicit operator CredentialContainer(NetworkCredential networkCredential)
	{
		if (networkCredential == null)
		{
			return null;
		}
		return new CredentialContainer(networkCredential);
	}

	public static CredentialContainer ToCredential(NetworkCredential networkCredential)
	{
		return networkCredential;
	}

	public static NetworkCredential ToNetworkCredential(CredentialContainer credential)
	{
		return credential;
	}
}

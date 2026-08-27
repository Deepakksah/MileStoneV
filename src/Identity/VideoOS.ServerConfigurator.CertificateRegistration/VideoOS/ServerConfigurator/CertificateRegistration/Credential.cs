using System;
using System.Net;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

[Serializable]
public sealed class Credential
{
	public string UserName { get; private set; }

	public string Domain { get; private set; }

	private byte[] EncryptedPassword { get; set; }

	private Credential(NetworkCredential networkCredential)
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

	public static implicit operator NetworkCredential(Credential credential)
	{
		return credential?.GetNetworkCredential();
	}

	public static implicit operator Credential(NetworkCredential networkCredential)
	{
		if (networkCredential == null)
		{
			return null;
		}
		return new Credential(networkCredential);
	}

	public static Credential ToCredential(NetworkCredential networkCredential)
	{
		return networkCredential;
	}

	public static NetworkCredential ToNetworkCredential(Credential credential)
	{
		return credential;
	}
}

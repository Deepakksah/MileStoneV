using System;

namespace VideoOS.Common.Crypto;

[CLSCompliant(true)]
public interface ISecretHandler
{
	char[] ReadSecret();

	void EnsureUserEncryptedSecret();

	void WriteSecret(byte[] secret);

	bool SecretExists();
}

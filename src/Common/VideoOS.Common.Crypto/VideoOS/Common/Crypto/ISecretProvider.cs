using System;
using System.Security.AccessControl;
using System.Security.Cryptography;

namespace VideoOS.Common.Crypto;

[CLSCompliant(true)]
public interface ISecretProvider
{
	byte[] ReadSecret();

	byte[] ReadTempSecret();

	void WriteSecret(byte[] secret);

	void WriteSecret(byte[] secret, FileSystemAccessRule rule);

	void DeletePermSecret();

	void WriteTempSecret(byte[] secret);

	void WriteTempSecret(byte[] secret, FileSystemAccessRule rule);

	byte[] DecryptSecret(byte[] secret, DataProtectionScope scope);

	byte[] EncryptSecret(byte[] secret, DataProtectionScope scope);

	void DeleteTempSecret();
}

using System;

namespace VideoOS.Common.Crypto;

public interface IAsymmetricEncryption : IDisposable
{
	string KeyInformationAsBase64 { get; }

	string PublicKeyAsBase64 { get; }

	byte[] Encrypt(byte[] data);

	byte[] Decrypt(byte[] data);
}

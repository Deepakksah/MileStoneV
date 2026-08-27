using System;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Crypto;

public class SHA256HashAlgorithm : IHashAlgorithm, IDisposable
{
	private readonly HashAlgorithm _hashAlgorithm = new SHA256Cng();

	private bool _disposed;

	public byte[] ComputeHash(byte[] value)
	{
		return _hashAlgorithm.ComputeHash(value);
	}

	public byte[] ComputeHash(string value)
	{
		return ComputeHash(Encoding.Unicode.GetBytes(value));
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_hashAlgorithm.Dispose();
			_disposed = true;
		}
	}
}

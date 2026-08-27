using System;

namespace VideoOS.Common.Crypto;

public interface IHashAlgorithm : IDisposable
{
	byte[] ComputeHash(byte[] value);

	byte[] ComputeHash(string value);
}

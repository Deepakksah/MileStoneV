namespace VideoOS.Common.Crypto;

public static class HashAlgorithmFactory
{
	public static IHashAlgorithm Create()
	{
		return new SHA256HashAlgorithm();
	}
}

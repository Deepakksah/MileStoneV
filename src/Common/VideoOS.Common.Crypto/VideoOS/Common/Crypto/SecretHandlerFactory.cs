namespace VideoOS.Common.Crypto;

public class SecretHandlerFactory : ISecretHandlerFactory
{
	private readonly ISecretProvider _secretProvider;

	public SecretHandlerFactory(ISecretProvider secretProvider)
	{
		_secretProvider = secretProvider;
	}

	public ISecretHandlerDisposable CreateSecretHandler()
	{
		return new SecretHandler(_secretProvider);
	}
}

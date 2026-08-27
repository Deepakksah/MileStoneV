namespace VideoOS.Common.Crypto;

public interface ISecretHandlerFactory
{
	ISecretHandlerDisposable CreateSecretHandler();
}

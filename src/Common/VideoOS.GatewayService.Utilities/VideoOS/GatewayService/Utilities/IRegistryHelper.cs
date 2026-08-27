namespace VideoOS.GatewayService.Utilities;

public interface IRegistryHelper
{
	void CleanUpRegistry();

	string ReadKey(RegistryKeys key);

	void WriteKey(RegistryKeys key, string value);
}

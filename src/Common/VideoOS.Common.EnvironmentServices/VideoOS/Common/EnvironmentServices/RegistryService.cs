using Microsoft.Win32;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
public class RegistryService : IRegistryService
{
	private IRegistryKeyService _registryKeyService;

	public IRegistryKey CurrentUser => RegistryKeyWrapper.WrapppedKey(Registry.CurrentUser);

	public IRegistryKey LocalMachine => RegistryKeyWrapper.WrapppedKey(Registry.LocalMachine);

	public IRegistryKey ClassesRoot => RegistryKeyWrapper.WrapppedKey(Registry.ClassesRoot);

	public IRegistryKey Users => RegistryKeyWrapper.WrapppedKey(Registry.Users);

	public IRegistryKey PerformanceData => RegistryKeyWrapper.WrapppedKey(Registry.PerformanceData);

	public IRegistryKey CurrentConfig => RegistryKeyWrapper.WrapppedKey(Registry.CurrentConfig);

	public IRegistryKeyService RegistryKeyService
	{
		get
		{
			IRegistryKeyService obj = _registryKeyService ?? new RegistreyKeyService();
			IRegistryKeyService result = obj;
			_registryKeyService = obj;
			return result;
		}
	}

	public object GetValue(string keyName, string valueName, object defaultValue)
	{
		return Registry.GetValue(keyName, valueName, defaultValue);
	}

	public void SetValue(string keyName, string valueName, object value)
	{
		Registry.SetValue(keyName, valueName, value);
	}

	public void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
	{
		Registry.SetValue(keyName, valueName, value, valueKind);
	}
}

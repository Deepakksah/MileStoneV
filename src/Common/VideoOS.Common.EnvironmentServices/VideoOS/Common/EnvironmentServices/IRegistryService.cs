using Microsoft.Win32;

namespace VideoOS.Common.EnvironmentServices;

public interface IRegistryService
{
	IRegistryKey CurrentUser { get; }

	IRegistryKey LocalMachine { get; }

	IRegistryKey ClassesRoot { get; }

	IRegistryKey Users { get; }

	IRegistryKey PerformanceData { get; }

	IRegistryKey CurrentConfig { get; }

	IRegistryKeyService RegistryKeyService { get; }

	object GetValue(string keyName, string valueName, object defaultValue);

	void SetValue(string keyName, string valueName, object value);

	void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind);
}

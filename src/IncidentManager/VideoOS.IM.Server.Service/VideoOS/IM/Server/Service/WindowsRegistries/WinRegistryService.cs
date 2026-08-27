using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.OEM;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.WindowsRegistries;

public sealed class WinRegistryService : IWinRegistryService
{
	private readonly IRegistryService _registryService;

	private string RegistryPath { get; }

	public WinRegistryService()
		: this("SOFTWARE\\" + OemResourceService.Instance.CompanyNameShort + "\\" + OemResourceService.Instance.IncidentManagerApplicationName, new RegistryService())
	{
	}

	public WinRegistryService([NotNull] string keyName, [NotNull] IRegistryService registryService)
	{
		keyName.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\WindowsRegistries\\WinRegistryService.cs", 30);
		registryService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\WindowsRegistries\\WinRegistryService.cs", 31);
		_registryService = registryService;
		RegistryPath = _registryService.LocalMachine.Name + "\\" + keyName;
	}

	[NotNull]
	public string ReadRegistryValue([NotNull] string keyValueName, [NotNull] string defaultValue)
	{
		keyValueName.NullCheck("", "ReadRegistryValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\WindowsRegistries\\WinRegistryService.cs", 41);
		defaultValue.NullCheck("", "ReadRegistryValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\WindowsRegistries\\WinRegistryService.cs", 42);
		return _registryService.GetValue(RegistryPath, keyValueName, defaultValue).ToString();
	}

	[NotNull]
	public string WriteRegistryValue([NotNull] string keyValueName, [NotNull] string value)
	{
		keyValueName.NullCheck("", "WriteRegistryValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\WindowsRegistries\\WinRegistryService.cs", 50);
		value.NullCheck("", "WriteRegistryValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\WindowsRegistries\\WinRegistryService.cs", 51);
		_registryService.SetValue(RegistryPath, keyValueName, value, RegistryValueKind.String);
		return value;
	}

	public void DeleteRegSubKey()
	{
		_registryService.LocalMachine.DeleteSubKey(RegistryPath, throwOnMissingSubKey: false);
	}
}

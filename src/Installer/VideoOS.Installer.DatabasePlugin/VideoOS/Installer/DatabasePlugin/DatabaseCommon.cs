using Microsoft.Win32;

namespace VideoOS.Installer.DatabasePlugin;

public static class DatabaseCommon
{
	private const string SQLServerInstanceSubKey = "SOFTWARE\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL";

	private const string SQLServerInstanceSubKey64 = "SOFTWARE\\Wow6432Node\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL";

	public static bool IsDefaultInstancePresent()
	{
		bool flag = false;
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL", writable: true);
		if (registryKey != null && registryKey.GetValue("MSSQLSERVER") != null)
		{
			flag = true;
		}
		if (!flag)
		{
			using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL", writable: true);
			if (registryKey2 != null && registryKey2.GetValue("MSSQLSERVER") != null)
			{
				flag = true;
			}
		}
		return flag;
	}
}

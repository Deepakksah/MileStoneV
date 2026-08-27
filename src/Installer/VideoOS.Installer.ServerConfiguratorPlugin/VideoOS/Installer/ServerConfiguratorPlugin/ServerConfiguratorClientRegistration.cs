using System.Diagnostics;
using System.IO;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class ServerConfiguratorClientRegistration
{
	public static void RemoveIDPSecrets()
	{
		RemoveIDPSecret(OemResources.Instance.Server.CommonApplicationDataFolder);
		RemoveIDPSecret(Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, "IIS", "ManagementServer"));
	}

	public static void RemoveIDPSecret(string providerPath)
	{
		LogManager.Write("Remove Authorization Server SecretFile: Started...");
		try
		{
			FileSecretProvider fileSecretProvider = new FileSecretProvider(providerPath, "idp.tmp", "idp.sec");
			fileSecretProvider.DeleteTempSecret();
			fileSecretProvider.DeletePermSecret();
		}
		catch (RemoveFileSecretException ex)
		{
			LogManager.Write("Failed to delete Authorization Server secret during uninstallation", ex.ToString(), TraceEventType.Error);
		}
	}
}

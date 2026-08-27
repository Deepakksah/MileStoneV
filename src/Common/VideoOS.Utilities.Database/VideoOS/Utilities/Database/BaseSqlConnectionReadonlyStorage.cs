using System;
using System.Security;
using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public abstract class BaseSqlConnectionReadonlyStorage : ISqlConnectionReadonlyStorage
{
	private const string FailToReadRegistryMessage = "Reading connection string failed";

	protected string RegistryPath;

	protected string RegistryKeyName;

	public IRegistryService RegistryService;

	public BaseSqlConnectionReadonlyStorage(string registryPath, string registryKeyName)
		: this(registryPath, registryKeyName, new RegistryService())
	{
	}

	public BaseSqlConnectionReadonlyStorage(string registryPath, string registryKeyName, IRegistryService registryService)
	{
		RegistryPath = registryPath ?? throw new ArgumentNullException("registryPath");
		RegistryKeyName = registryKeyName ?? throw new ArgumentNullException("registryKeyName");
		RegistryService = registryService ?? throw new ArgumentNullException("registryService");
	}

	public virtual string RetrieveConnectionString()
	{
		try
		{
			using (IRegistryKey registryKey = RegistryService.RegistryKeyService.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				IRegistryKey registryKey2 = registryKey.OpenSubKey(RegistryPath);
				if (registryKey2 != null)
				{
					return (string)registryKey2.GetValue(RegistryKeyName);
				}
			}
			using (IRegistryKey registryKey3 = RegistryService.RegistryKeyService.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
			{
				IRegistryKey registryKey4 = registryKey3.OpenSubKey(RegistryPath);
				if (registryKey4 != null)
				{
					return (string)registryKey4.GetValue(RegistryKeyName);
				}
			}
			return null;
		}
		catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
		{
			throw new ConnectionStorageReadException("Reading connection string failed", ex);
		}
	}

	public void CleanConnectionString()
	{
		try
		{
			using (IRegistryKey registryKey = RegistryService.RegistryKeyService.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				registryKey.OpenSubKey(RegistryPath, writable: true)?.DeleteValue(RegistryKeyName, throwOnMissingValue: false);
			}
			using IRegistryKey registryKey2 = RegistryService.RegistryKeyService.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
			registryKey2.OpenSubKey(RegistryPath, writable: true)?.DeleteValue(RegistryKeyName, throwOnMissingValue: false);
		}
		catch
		{
		}
	}
}

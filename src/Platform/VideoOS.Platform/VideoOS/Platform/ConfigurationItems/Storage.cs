using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Storage : IConfigurationItem
{
	public ArchiveStorageFolder ArchiveStorageFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ArchiveStorageFolder")
				{
					return new ArchiveStorageFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string DiskPath => base.Wrapper.GetPropertyValue("DiskPath");

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public int RetainMinutes
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RetainMinutes");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RetainMinutes", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int MaxSize
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MaxSize");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MaxSize", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool IsDefault
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IsDefault");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public bool Signing
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Signing");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Signing", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EncryptionMethodValues => base.Wrapper.GetEnumValues("EncryptionMethod");

	public string EncryptionMethod => base.Wrapper.GetPropertyValue("EncryptionMethod");

	public Storage(ServerId serverId, ConfigurationItem storage)
		: base(serverId, storage)
	{
	}

	public Storage(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Storage(FQID fqid)
		: base(fqid)
	{
	}

	public ClassCollectionServerTask GetDevicesUsingStorage()
	{
		try
		{
			return new ClassCollectionServerTask(base.Wrapper.ExecuteMethod("GetDevicesUsingStorage").ExecuteMethod("GetDevicesUsingStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'GetDevicesUsingStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask ReadStorageInformation()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("ReadStorageInformation").ExecuteMethod("ReadStorageInformation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ReadStorageInformation':" + ex.Message);
		}
		return null;
	}

	public ServerTask SetStorageAsDefault()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("SetStorageAsDefault").ExecuteMethod("SetStorageAsDefault"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'SetStorageAsDefault':" + ex.Message);
		}
		return null;
	}

	public ServerTask EnableEncryption(SecureString password, string encryptionMethod)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("EnableEncryption");
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("EncryptionMethod", encryptionMethod);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("EnableEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'EnableEncryption':" + ex.Message);
		}
		return null;
	}

	public EnableEncryptionServerTask EnableEncryption()
	{
		try
		{
			return new EnableEncryptionServerTask(base.Wrapper.ExecuteMethod("EnableEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'EnableEncryption':" + ex.Message);
		}
		return null;
	}

	public ServerTask MoveRecordingStorage(string diskPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MoveRecordingStorage");
			configurationApiItemWrapper.SetProperty("DiskPath", diskPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MoveRecordingStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'MoveRecordingStorage':" + ex.Message);
		}
		return null;
	}

	public MoveRecordingStorageServerTask MoveRecordingStorage()
	{
		try
		{
			return new MoveRecordingStorageServerTask(base.Wrapper.ExecuteMethod("MoveRecordingStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'MoveRecordingStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangeEncryptionMethod(string encryptionMethod, SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeEncryptionMethod");
			configurationApiItemWrapper.SetProperty("EncryptionMethod", encryptionMethod);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeEncryptionMethod"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ChangeEncryptionMethod':" + ex.Message);
		}
		return null;
	}

	public ChangeEncryptionMethodServerTask ChangeEncryptionMethod()
	{
		try
		{
			return new ChangeEncryptionMethodServerTask(base.Wrapper.ExecuteMethod("ChangeEncryptionMethod"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ChangeEncryptionMethod':" + ex.Message);
		}
		return null;
	}

	public ServerTask ValidateStorageEncryptionPassword(SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ValidateStorageEncryptionPassword");
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ValidateStorageEncryptionPassword"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ValidateStorageEncryptionPassword':" + ex.Message);
		}
		return null;
	}

	public ValidateStorageEncryptionPasswordServerTask ValidateStorageEncryptionPassword()
	{
		try
		{
			return new ValidateStorageEncryptionPasswordServerTask(base.Wrapper.ExecuteMethod("ValidateStorageEncryptionPassword"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ValidateStorageEncryptionPassword':" + ex.Message);
		}
		return null;
	}

	public ServerTask DisableEncryption(SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("DisableEncryption");
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("DisableEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'DisableEncryption':" + ex.Message);
		}
		return null;
	}

	public DisableEncryptionServerTask DisableEncryption()
	{
		try
		{
			return new DisableEncryptionServerTask(base.Wrapper.ExecuteMethod("DisableEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'DisableEncryption':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangePasswordStorageEncryption(string oldPassword, string newPassword)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangePasswordStorageEncryption");
			configurationApiItemWrapper.SetProperty("oldPassword", oldPassword);
			configurationApiItemWrapper.SetProperty("newPassword", newPassword);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangePasswordStorageEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ChangePasswordStorageEncryption':" + ex.Message);
		}
		return null;
	}

	public ChangePasswordStorageEncryptionServerTask ChangePasswordStorageEncryption()
	{
		try
		{
			return new ChangePasswordStorageEncryptionServerTask(base.Wrapper.ExecuteMethod("ChangePasswordStorageEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'ChangePasswordStorageEncryption':" + ex.Message);
		}
		return null;
	}

	public ServerTask EnableEncryption(string password, string encryptionMethod)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("EnableEncryption");
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("EncryptionMethod", encryptionMethod);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("EnableEncryption"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Storage", "Unable to call method 'EnableEncryption':" + ex.Message);
		}
		return null;
	}
}

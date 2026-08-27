using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingStoragePropertyDefinition
{
	public static Property DiskPathProperty(string diskPath)
	{
		return new Property("DiskPath", diskPath)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingStorageDiskPath,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingStorageDiskPath",
			ToolTipTranslationId = "PropertyRecordingStorageDiskPathToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property IsDefaultProperty(bool isDefault)
	{
		return new Property("IsDefault", PropertyTypesHelper.GetValue(isDefault))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingStorageIsDefault,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingStorageIsDefault",
			ToolTipTranslationId = "PropertyRecordingStorageIsDefaultToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property RetainMinutesProperty(int retainMinutes)
	{
		return new Property("RetainMinutes", PropertyTypesHelper.GetValue(retainMinutes))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingStorageRetainMinutes,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingStorageRetainMinutes",
			ToolTipTranslationId = "PropertyRecordingStorageRetainMinutesToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 525600000)
		};
	}

	public static Property MaxSizeProperty(long maxSize)
	{
		return new Property("MaxSize", PropertyTypesHelper.GetValue(maxSize))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingStorageMaxSize,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingStorageMaxSize",
			ToolTipTranslationId = "PropertyRecordingStorageMaxSizeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 1099511627776L)
		};
	}

	public static Property SigningProperty(bool signing)
	{
		return new Property("Signing", PropertyTypesHelper.GetValue(signing))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingStorageSigning,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingStorageSigning",
			ToolTipTranslationId = "PropertyRecordingStorageSigningToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property EncryptionMethodProperty(EncryptionMethod encryptionMethod)
	{
		return new Property("EncryptionMethod", encryptionMethod.ToString())
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingStorageEncryption,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingStorageEncryption",
			ToolTipTranslationId = "PropertyRecordingStorageEncryptionToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(EncryptionMethod))
		};
	}

	public static Property PasswordProperty(string password)
	{
		return new Property("Password", password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStorageEncryptionPassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageEncryptionPassword",
			ToolTipTranslationId = "PropertyStorageEncryptionPasswordToolTip",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}

	public static Property CurrentPasswordProperty(string password)
	{
		return new Property("oldPassword", password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStorageEncryptionPasswordOld,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageEncryptionPasswordOld",
			ToolTipTranslationId = "PropertyStorageEncryptionPasswordOldToolTip",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}

	public static Property NewPasswordProperty(string password)
	{
		return new Property("newPassword", password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStorageEncryptionPasswordNew,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageEncryptionPasswordNew",
			ToolTipTranslationId = "PropertyStorageEncryptionPasswordNewToolTip",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}
}

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class LicensePropertyDefinition
{
	public const string SkuPropertyKey = "Sku";

	public const string SlcPropertyKey = "Slc";

	public const string CareLevelPropertyKey = "CareLevel";

	public const string CareIdPropertyKey = "CareId";

	public const string ProductDisplayNamePropertyKey = "ProductDisplayName";

	public const string ExpirationDatePropertyKey = "ExpirationDate";

	public const string PurchaseActivationDatePropertyKey = "PuAc";

	public const string CarePlusPropertyKey = "CarePlus";

	public const string CarePremiumPropertyKey = "CarePremium";

	public const string LicenseTypePropertyKey = "LicenseType";

	public const string ActivatedPropertyKey = "Activated";

	public const string ChangesWithoutActivationKey = "ChangesWithoutActivation";

	public const string InGracePropertyKey = "InGrace";

	public const string GraceExpiredPropertyKey = "GraceExpired";

	public const string NotLicensedPropertyKey = "NotLicensed";

	public const string NotePropertyKey = "Note";

	public const string FeaturePropertyKey = "Features";

	public const string FeatureExpirePropertyKey = "FeatureExpire";

	public const string ContactAddressPropertyKey = "ContactAddress";

	public const string ContactEmailPropertyKey = "ContactEmail";

	public const string ContactPhonePropertyKey = "ContactPhone";

	public const string ContactNamePropertyKey = "ContactName";

	public const string UserNamePropertyKey = "UserName";

	public const string PasswordPropertyKey = "Password";

	public const string ActivationAutomaticPropertyKey = "ActivationAutomatic";

	public const string LicensePropertyKey = "License";

	public const string PluginIdPropertyKey = "PluginId";

	public const string FirstGraceExpirationDatePropertyKey = "FirstGraceExpirationDate";

	public const string ProductionEnvironmentKey = "ProductionEnvironment";

	public const string LicenseFeatureChildParametersKey = "Parameters";

	public static Property ProductDisplayNameProperty(string productName)
	{
		return new Property("ProductDisplayName", productName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.ProductDisplayNameProperty,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "ProductDisplayNameProperty",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ExpireDateProperty(string expire)
	{
		return new Property("ExpirationDate", expire)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseExpirationDate,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseExpirationDate",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PurchaseActivationDateProperty(string purchaseActivationDate)
	{
		return new Property("PuAc", purchaseActivationDate)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyPurchaseActivationDate,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPurchaseActivationDate",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property CarePlusProperty(string carePlus)
	{
		return new Property("CarePlus", carePlus)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseCarePlusProperty,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseCarePlusProperty",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property CarePremiumProperty(string carePremium)
	{
		return new Property("CarePremium", carePremium)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseCarePremiumProperty,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseCarePremiumProperty",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property SkuProperty(string sku)
	{
		return new Property("Sku", sku)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseInfoSku,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseInfoSku",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property SlcProperty(string slc)
	{
		return new Property("Slc", slc)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseInfoSlc,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseInfoSlc",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property CareLevelProperty(string careLevel)
	{
		return new Property("CareLevel", careLevel)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseInfoCareLevel,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseInfoCareLevel",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property CareIdProperty(string careId)
	{
		return new Property("CareId", careId)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseInfoCareId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseInfoCareId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PluginIdProperty(Guid pluginId)
	{
		return new Property("PluginId", pluginId.ToString())
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseInfoPluginId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseInfoPluginId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property LicenseTypeProperty(string licenseType)
	{
		return new Property("LicenseType", licenseType)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseType",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ActivatedProperty(string activated)
	{
		return new Property("Activated", activated)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseActivated,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseActivated",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ChangesWithoutActivationProperty(string changesWithoutActivation)
	{
		return new Property("ChangesWithoutActivation", changesWithoutActivation)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseChangesWithoutActivation,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseChangesWithoutActivation",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property InGraceProperty(string inGrace)
	{
		return new Property("InGrace", inGrace)
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseInGrace,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseInGrace",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GraceExpiredProperty(string graceExpired)
	{
		return new Property("GraceExpired", graceExpired)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseGraceExpired,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseGraceExpired",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property NotLicensedProperty(string notLicensed)
	{
		return new Property("NotLicensed", notLicensed)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyNotLicensed,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyNotLicensed",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property NoteProperty(string note)
	{
		return new Property("Note", note)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLicenseNote,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseNote",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ActivationUserNameProperty()
	{
		return new Property("UserName", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLicenseActivateUsername,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseActivateUsername",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ActivationPasswordProperty()
	{
		return new Property("Password", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLicenseActivatePassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseActivatePassword",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}

	public static Property ActivationAutomaticProperty(bool current, bool canEdit)
	{
		return new Property("ActivationAutomatic", current.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = canEdit,
			DisplayName = ConfigurationApiResources.PropertyLicenseActivationAutomatic,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseActivationAutomatic",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FeatureProperty(ReadOnlyCollection<string> allFeatureNames)
	{
		Property property = new Property("Features", "");
		property.ValueType = "Array";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyLicenseFeatures;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyLicenseFeatures";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		property.ValueArray = allFeatureNames.ToArray();
		return property;
	}

	public static Property LicenseProperty(string base64)
	{
		return new Property("License", base64)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLicenseContent,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLicenseContent",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FirstGraceExpirationDateProperty(string expire)
	{
		return new Property("FirstGraceExpirationDate", expire)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyFirstGraceExpirationDate,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFirstGraceExpirationDate",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property SlcEnvironmentProperty(string softwareLicenseCodeEnvironment)
	{
		return new Property("ProductionEnvironment", softwareLicenseCodeEnvironment)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyProductionEnvironment,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyProductionEnvironment",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}

using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class BasicUserPropertyDefinition
{
	public static Property SidProperty(string sid)
	{
		return new Property("Sid", sid)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyBasicUserSid,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyBasicUserSid",
			UIImportance = UIImportance.Hidden,
			SortKey = 100
		};
	}

	public static Property CanChangePasswordProperty(bool canChangePassword)
	{
		return new Property("CanChangePassword", canChangePassword.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyBasicUserCanChangePassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyBasicUserCanChangePassword",
			ToolTipTranslationId = "PropertyBasicUserCanChangePasswordTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ForcePasswordChangeProperty(bool forceChangePassword, bool isSettable = true)
	{
		return new Property("ForcePasswordChange", forceChangePassword.ToString())
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyBasicUserForcePasswordChange,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyBasicUserForcePasswordChange",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property Status(BasicUserStatus basicUserStatus)
	{
		Property property = new Property("Status", PropertyTypesHelper.GetValue(basicUserStatus));
		property.ValueType = "Enum";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyBasicUserStatus;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyBasicUserStatus";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[3]
		{
			new ValueTypeInfo
			{
				Name = BasicUserStatus.Enabled.ToString(),
				Value = BasicUserStatus.Enabled.ToString()
			},
			new ValueTypeInfo
			{
				Name = BasicUserStatus.LockedOutByAdmin.ToString(),
				Value = BasicUserStatus.LockedOutByAdmin.ToString()
			},
			new ValueTypeInfo
			{
				Name = BasicUserStatus.LockedOutBySystem.ToString(),
				Value = BasicUserStatus.LockedOutBySystem.ToString()
			}
		};
		return property;
	}

	public static Property LockoutEnd(DateTime? basicUserLockoutEnd)
	{
		return new Property("LockoutEnd", PropertyTypesHelper.GetValue(basicUserLockoutEnd))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyBasicUserLockoutEnd,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyBasicUserLockoutEnd",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PasswordProperty(string password)
	{
		return new Property("Password", password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyBasicUserPassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyBasicUserPassword",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}

	public static Property IsExternal(bool isExternal)
	{
		return new Property("IsExternal", PropertyTypesHelper.GetValue(isExternal))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyIsExternal,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyIsExternal",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}

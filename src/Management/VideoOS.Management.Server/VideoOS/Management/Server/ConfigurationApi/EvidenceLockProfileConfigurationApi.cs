using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIEvidenceLockProfile")]
[EntityName("ConfigAPIEvidenceLockProfileFolder")]
public class EvidenceLockProfileConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public static Guid DefaultEvidenceLockProfileId = new Guid("b665dde9-1000-41fe-af15-ef5946314da7");

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			throw new NotSupportedException();
		}
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE");
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string name;
		List<RetentionTime> options;
		bool isDefault;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path.Id, out name, out options, out isDefault);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			Guid id = new Guid(path.Id);
			RetentionOptionsXmlFormatter retentionOptionsXmlFormatter = RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions"));
			RetentionTimeOption retentionTimeOption = retentionOptionsXmlFormatter.RetentionOptions.FirstOrDefault((RetentionTimeOption x) => x.Id == id);
			if (retentionTimeOption != null)
			{
				retentionTimeOption.RetentionTimes = options;
				retentionTimeOption.Default = isDefault;
				string value = RetentionOptionsXmlManager.Serialize(retentionOptionsXmlFormatter);
				SystemSetting.SetSystemSetting(requestContext, "RetentionOptions", value);
				return validateResult;
			}
			throw new PathNotFoundException(path.ToString());
		}
		catch (UnauthorizedAccessException)
		{
			throw new UnauthorizedAccessException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			throw new NotSupportedException();
		}
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE");
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string name;
		List<RetentionTime> options;
		bool isDefault;
		return ValidateItemInternal(requestContext, item, path.Id, out name, out options, out isDefault);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			throw new NotSupportedException();
		}
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_READ");
		if (path.IsFolder)
		{
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (RetentionTimeOption retentionOption in RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions")).RetentionOptions)
			{
				ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("EvidenceLockProfile", retentionOption.Id.ToString());
				list.Add(ConvertToConfigurationItem(retentionOption, path2));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			throw new NotSupportedException();
		}
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_READ");
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("EvidenceLockProfileFolder", ConfigurationItemPath.CreateSystemPath());
		}
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		try
		{
			RetentionOptionsXmlFormatter retentionOptionsXmlFormatter = RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions"));
			Guid id = new Guid(path.Id);
			RetentionTimeOption retentionTimeOption = retentionOptionsXmlFormatter.RetentionOptions.FirstOrDefault((RetentionTimeOption profile) => profile.Id == id);
			if (retentionTimeOption != null)
			{
				return ConvertToConfigurationItem(retentionTimeOption, path);
			}
			throw new PathNotFoundException(path.ToString());
		}
		catch (UnauthorizedAccessException)
		{
			throw new UnauthorizedAccessException(path.Text);
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			throw new NotSupportedException();
		}
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE");
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(methodId == "AddEvidenceLockProfile"))
		{
			if (methodId == "RemoveEvidenceLockProfile")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveEvidenceLockProfile(item, GetChildItems(requestContext, path), methodId);
				}
				return RemoveEvidenceLockProfile(requestContext, item, path);
			}
			throw new NotSupportedException();
		}
		if (!flag)
		{
			return CreateInvokeInfoAddEvidenceLockProfile(item, methodId);
		}
		return AddEvidenceLockProfile(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfoAddEvidenceLockProfile(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddEvidenceLockProfile;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.NewGuid().ToString()),
			GeneralPropertyDefinitions.GetNameProperty(ConfigurationApiResources.EvidenceLockNameNewProfile),
			EvidenceLockProfilePropertyDefinitions.GetRetentionTimeOptions(new List<RetentionTime>())
		};
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem AddEvidenceLockProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath folderPath)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, folderPath);
		if (!string.Equals(folderPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + folderPath.Text);
		}
		string text = Guid.NewGuid().ToString();
		string name;
		List<RetentionTime> options;
		bool isDefault;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, text, out name, out options, out isDefault);
		if (!validateResult.ValidatedOk)
		{
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Any())
			{
				throw new PropertyInvalidException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
			}
			throw new PropertyInvalidException();
		}
		string text2 = item.Properties.FirstOrDefault((Property p) => p.Key == "Id")?.Value;
		RetentionOptionsXmlFormatter retentionOptionsXmlFormatter = RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions"));
		Guid id = new Guid(text2 ?? text);
		if (retentionOptionsXmlFormatter.RetentionOptions.Any((RetentionTimeOption profile) => profile.Id == id))
		{
			throw new PropertyInvalidException("Duplicate Id");
		}
		RetentionTimeOption retentionTimeOption = new RetentionTimeOption
		{
			Default = isDefault,
			Id = id,
			Name = name,
			RetentionTimes = options
		};
		retentionOptionsXmlFormatter.RetentionOptions.Add(retentionTimeOption);
		string value = RetentionOptionsXmlManager.Serialize(retentionOptionsXmlFormatter);
		SystemSetting.SetSystemSetting(requestContext, "RetentionOptions", value);
		ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("EvidenceLockProfile", id.ToString());
		ConfigurationItem configurationItem = ConvertToConfigurationItem(retentionTimeOption, path);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem CreateInvokeInfoRemoveEvidenceLockProfile(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem[] source = members.Where((ConfigurationItem m) => !IsDefaultProfile(m.Path)).ToArray();
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveEvidenceLockProfile, "WizardRemoveEvidenceLockProfile", ConfigurationApiResources.PropertyRemoveEvidenceLockProfile, item, source.ToArray());
	}

	private static bool IsDefaultProfile(string path)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(path);
		return new Guid(configurationItemPath.Id) == DefaultEvidenceLockProfileId;
	}

	private static ConfigurationItem RemoveEvidenceLockProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		Guid id = new Guid(invokeMemberItemSelectionPath.Id);
		if (id == DefaultEvidenceLockProfileId)
		{
			throw new PathInvalidException("Cannot remove default profile");
		}
		RetentionOptionsXmlFormatter retentionOptionsXmlFormatter = RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions"));
		RetentionTimeOption retentionTimeOption = retentionOptionsXmlFormatter.RetentionOptions.FirstOrDefault((RetentionTimeOption r) => r.Id == id);
		if (retentionTimeOption != null)
		{
			retentionOptionsXmlFormatter.RetentionOptions.Remove(retentionTimeOption);
			string value = RetentionOptionsXmlManager.Serialize(retentionOptionsXmlFormatter);
			SystemSetting.SetSystemSetting(requestContext, "RetentionOptions", value);
			return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
		}
		throw new PathNotFoundException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, string id, out string name, out List<RetentionTime> options, out bool isDefault)
	{
		name = null;
		try
		{
			if (item.Properties == null)
			{
				throw new PropertyInvalidException("No properties provided");
			}
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
			Property property = item.Properties.FirstOrDefault((Property p) => p.Key == "RetentionTimeOptions");
			if (property == null || property.ValueArray == null || property.ValueArray.Length == 0)
			{
				throw new PropertyInvalidException("Missing property", "RetentionTimeOptions");
			}
			Guid guid = new Guid(id);
			ThrowIfDuplicateName(requestContext, name, guid);
			options = new List<RetentionTime>();
			string[] valueArray = property.ValueArray;
			for (int num = 0; num < valueArray.Length; num++)
			{
				string[] array = valueArray[num].Split(';');
				if (array.Length != 2)
				{
					throw new PropertyInvalidException("Incorrect property format", "RetentionTimeOptions");
				}
				if (!Enum.TryParse<OptionType>(array[0], out var result))
				{
					throw new PropertyInvalidException("Unknown time unit, expected one of: Days, Weeks, Months, Years, Unlimited, UserDefined", "RetentionTimeOptions");
				}
				if (!int.TryParse(array[1], out var result2) || result2 < -1 || result2 == 0)
				{
					throw new PropertyInvalidException("Invalid number", "RetentionTimeOptions");
				}
				options.Add(new RetentionTime
				{
					Option = result,
					Unit = result2
				});
			}
			for (int num2 = 0; num2 < options.Count; num2++)
			{
				for (int num3 = num2 + 1; num3 < options.Count; num3++)
				{
					if ((options[num2].Option == options[num3].Option && options[num2].Unit == options[num3].Unit) || (options[num2].Option == OptionType.UserDefined && options[num3].Option == OptionType.UserDefined) || (options[num2].Option == OptionType.Indefinite && options[num3].Option == OptionType.Indefinite))
					{
						throw new PropertyInvalidException("Duplicate options", "RetentionTimeOptions");
					}
				}
			}
			isDefault = guid == DefaultEvidenceLockProfileId;
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		catch (PropertyInvalidException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			options = null;
			isDefault = false;
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			options = null;
			isDefault = false;
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	protected static ConfigurationItem ConvertToConfigurationItem(RetentionTimeOption retentionTimeOption, ConfigurationItemPath path)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateSystemFolderPath("EvidenceLockProfileFolder");
		return new ConfigurationItem
		{
			UIHint = "DirectionHorizontal",
			ItemType = path.ItemType,
			ItemCategory = ItemCategories.Item,
			DisplayName = retentionTimeOption.Name,
			ParentPath = configurationItemPath.Text,
			Path = path.Text,
			ChildrenFilled = true,
			Properties = new List<Property>
			{
				GeneralPropertyDefinitions.GetIdPropertyGuid(retentionTimeOption.Id),
				GeneralPropertyDefinitions.GetNameProperty(retentionTimeOption.Name),
				EvidenceLockProfilePropertyDefinitions.GetRetentionTimeOptions(retentionTimeOption.RetentionTimes)
			}.ToArray(),
			MethodIds = new string[0]
		};
	}

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, Guid objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		if (RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions")).RetentionOptions.Any((RetentionTimeOption r) => r.Id != objectId && r.Name == name))
		{
			throw new ArgumentException("Duplicate name");
		}
	}
}

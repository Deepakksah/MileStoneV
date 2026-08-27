using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRegisteredClaim")]
public class RegisteredClaimConfigurationApi : ConfigurationApiEntityBase
{
	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		string name = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name) ? name : null);
		RegisteredClaimData registeredClaim = RegisteredClaim.Update(requestContext, path.Id, name, item.DisplayName);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = ConvertToConfigurationItem(registeredClaim)
		};
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return ValidateItemInternal(requestContext, item, path, parentPath);
	}

	private ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
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
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfParentPathInvalid(parentPath);
		try
		{
			string name = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name) ? name : null);
			RegisteredClaim.UpdateValidation(requestContext, path.Id, name, item.DisplayName);
			return new ValidateResult
			{
				ValidatedOk = true,
				ResultItem = item
			};
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		return new ConfigurationItem[0];
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		return ConvertToConfigurationItem(RegisteredClaim.QueryById(requestContext, path.Id));
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		throw new NotSupportedException();
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		throw new NotSupportedException();
	}

	public static ConfigurationItemPath Path(int claimId)
	{
		return Path(claimId.ToString(CultureInfo.InvariantCulture));
	}

	public static ConfigurationItemPath Path(string claimId)
	{
		return ConfigurationItemPath.CreateItemPath("RegisteredClaim", claimId);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfItemPathInvalid(path);
	}

	public static void ThrowIfItemPathInvalid(ConfigurationItemPath path)
	{
		if (string.Equals(path?.ItemType, "RegisteredClaim", StringComparison.Ordinal) && int.TryParse(path.Id, NumberStyles.Integer, CultureInfo.InvariantCulture, out var _) && !path.IsFolder)
		{
			return;
		}
		throw new PathInvalidException("Invalid path. Path: " + path.Text);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		RegisteredClaimFolderConfigurationApi.ThrowIfFolderPathInvalid(path);
	}

	public static ConfigurationItem ConvertToConfigurationItem(RegisteredClaimData registeredClaim)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "RegisteredClaim";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = registeredClaim.DisplayName;
		configurationItem.ParentPath = RegisteredClaimFolderConfigurationApi.Path(registeredClaim.LoginProviderId.ToString()).Text;
		configurationItem.Path = Path(registeredClaim.Id).Text;
		configurationItem.Children = Array.Empty<ConfigurationItem>();
		configurationItem.ChildrenFilled = true;
		configurationItem.MethodIds = Array.Empty<string>();
		configurationItem.Properties = new Property[3]
		{
			GeneralPropertyDefinitions.GetIdPropertyInt(registeredClaim.Id),
			GeneralPropertyDefinitions.GetNameProperty(registeredClaim.Name),
			RegisteredClaimPropertyDefinition.CaseSensitive(registeredClaim.CaseSensitive, isSettable: false)
		};
		return configurationItem;
	}
}

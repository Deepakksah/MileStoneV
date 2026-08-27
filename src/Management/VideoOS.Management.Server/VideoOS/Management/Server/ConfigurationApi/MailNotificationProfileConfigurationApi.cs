using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIMailNotificationProfile")]
[EntityName("ConfigAPIMailNotificationProfileFolder")]
public class MailNotificationProfileConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
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
		if (path.IsFolder)
		{
			MailNotificationProfile[] array = MailNotificationProfile.QueryProfiles(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			MailNotificationProfile[] array2 = array;
			foreach (MailNotificationProfile profile in array2)
			{
				list.Add(ConvertToConfigurationItem(profile));
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
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("MailNotificationProfileFolder", ConfigurationItemPath.CreateSystemPath());
		}
		Guid id = new Guid(path.Id);
		return ConvertToConfigurationItem(MailNotificationProfile.QueryProfile(requestContext, id) ?? throw new PathNotFoundException(path.Text));
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
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(methodId == "AddMailNotificationProfile"))
		{
			if (methodId == "RemoveMailNotificationProfile")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveMailNotificationProfile(item, GetChildItems(requestContext, path), methodId);
				}
				return RemoveMailNotificationProfile(requestContext, item, path);
			}
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddMailNotificationProfile(item, methodId);
		}
		return AddMailNotificationProfile(requestContext, item, path);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.ItemType != "MailNotificationProfile" || !Guid.TryParse(path.Id, out var result))
		{
			throw new ArgumentException("Invalid path", "path");
		}
		ValidateInternal(item, out var name, out var description, out var _, out var settings);
		if (MailNotificationProfile.QueryProfile(requestContext, result) == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return new ValidateResult(ConvertToConfigurationItem(MailNotificationProfile.UpdateProfile(requestContext, result, name, description, settings.SaveAsXml())));
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ValidateInternal(item, out var _, out var _, out var _, out var _);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = null
		};
	}

	private static ConfigurationItem CreateInvokeInfoRemoveMailNotificationProfile(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveNotificationProfile, "PropertyRemoveNotificationProfile", ConfigurationApiResources.PropertyRemoveNotificationProfile, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoAddMailNotificationProfile(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddNotificationProfile;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[15]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty, uiVisible: true),
			GeneralPropertyDefinitions.GetNameProperty(string.Empty, isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty(string.Empty),
			MailNotificationProfilePropertyDefinitions.Subject(string.Empty),
			MailNotificationProfilePropertyDefinitions.MessageText(string.Empty),
			MailNotificationProfilePropertyDefinitions.Recipients(string.Empty),
			MailNotificationProfilePropertyDefinitions.TimeBetweenMails("10"),
			MailNotificationProfilePropertyDefinitions.IncludeImages("false"),
			MailNotificationProfilePropertyDefinitions.NumberOfImages("5"),
			MailNotificationProfilePropertyDefinitions.EmbedImages("true"),
			MailNotificationProfilePropertyDefinitions.TimeBetweenImages("500"),
			MailNotificationProfilePropertyDefinitions.IncludeAVI("false"),
			MailNotificationProfilePropertyDefinitions.TimeBeforeEvent("2"),
			MailNotificationProfilePropertyDefinitions.TimeAfterEvent("4"),
			MailNotificationProfilePropertyDefinitions.FrameRate("5")
		};
		return configurationItem;
	}

	private static ConfigurationItem RemoveMailNotificationProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			MailNotificationProfile.DeleteProfile(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		}
		catch (Exception ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem AddMailNotificationProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Unsupported parent item type: " + path.Text);
		}
		ValidateInternal(item, out var name, out var description, out var id, out var settings);
		ConfigurationItem configurationItem = ConvertToConfigurationItem(MailNotificationProfile.CreateProfile(requestContext, id, name, description, settings.SaveAsXml()));
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static void ValidateInternal(ConfigurationItem item, out string name, out string description, out Guid id, out Settings settings)
	{
		string badEmail = string.Empty;
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Recipients", out string[] values) || !ValidateRecipients(values, out var recipientString, out badEmail))
		{
			throw new PropertyInvalidException("Missing or invalid formatted email " + badEmail, "Recipients");
		}
		description = GetStringValue(item, "Description");
		name = GetStringValue(item, "Name");
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value))
		{
			if (!Guid.TryParse(value, out var result))
			{
				throw new PropertyInvalidException("Id needs to be a valid Guid", "Id");
			}
			if (result == Guid.Empty)
			{
				id = Guid.NewGuid();
			}
			else
			{
				id = result;
			}
		}
		else
		{
			id = Guid.NewGuid();
		}
		string stringValue = GetStringValue(item, "Subject");
		string stringValue2 = GetStringValue(item, "MessageText");
		int intValue = GetIntValue(item, "TimeBetweenMails");
		bool boolValue = GetBoolValue(item, "IncludeImages");
		int intValue2 = GetIntValue(item, "NumberOfImages");
		bool boolValue2 = GetBoolValue(item, "EmbedImages");
		int intValue3 = GetIntValue(item, "TimeBetweenImages");
		bool boolValue3 = GetBoolValue(item, "IncludeAVI");
		int intValue4 = GetIntValue(item, "TimeBeforeEvent");
		int intValue5 = GetIntValue(item, "TimeAfterEvent");
		int intValue6 = GetIntValue(item, "FrameRate");
		settings = new Settings();
		settings.Set("Subject", stringValue);
		settings.Set("Text", stringValue2);
		settings.Set("Recipients", recipientString);
		settings.Set("TimeBetweenMails", intValue.ToString(CultureInfo.InvariantCulture));
		settings.Set("IncludeImages", boolValue.ToString(CultureInfo.InvariantCulture));
		settings.Set("NumberOfImages", intValue2.ToString(CultureInfo.InvariantCulture));
		settings.Set("EmbedImages", boolValue2.ToString(CultureInfo.InvariantCulture));
		settings.Set("TimeBetweenImages", intValue3.ToString(CultureInfo.InvariantCulture));
		settings.Set("IncludeAVI", boolValue3.ToString(CultureInfo.InvariantCulture));
		settings.Set("TimeBeforeEvent", intValue4.ToString(CultureInfo.InvariantCulture));
		settings.Set("TimeAfterEvent", intValue5.ToString(CultureInfo.InvariantCulture));
		settings.Set("FrameRate", intValue6.ToString(CultureInfo.InvariantCulture));
	}

	private static bool ValidateRecipients(string[] recipientFromProperty, out string recipientString, out string badEmail)
	{
		recipientString = null;
		List<string> list = new List<string>();
		foreach (string text in recipientFromProperty)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (!ValidateEmailAddress(text))
				{
					badEmail = text;
					return false;
				}
				list.Add(text);
			}
		}
		recipientString = string.Join(";", list);
		badEmail = string.Empty;
		return true;
	}

	private static string GetStringValue(ConfigurationItem item, string key)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			throw new PropertyInvalidException("", key);
		}
		return value;
	}

	private static int GetIntValue(ConfigurationItem item, string key)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value) || !int.TryParse(value, out var result))
		{
			throw new PropertyInvalidException("", key);
		}
		return result;
	}

	private static bool GetBoolValue(ConfigurationItem item, string key)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value) || !bool.TryParse(value, out var result))
		{
			throw new PropertyInvalidException("", key);
		}
		return result;
	}

	private static bool ValidateEmailAddress(string email)
	{
		try
		{
			if (!string.IsNullOrEmpty(email))
			{
				new MailAddress(email);
				return true;
			}
			return false;
		}
		catch (FormatException)
		{
			return false;
		}
	}

	private static ConfigurationItem ConvertToConfigurationItem(MailNotificationProfile profile)
	{
		Settings settings = new Settings();
		if (profile.SettingsXml != null)
		{
			settings.LoadXml(profile.SettingsXml, SettingsLoadMethod.Reload);
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "MailNotificationProfileFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("MailNotificationProfile", profile.ProfileId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "MailNotificationProfile",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = profile.Name,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(profile.LastModified));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(profile.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(profile.Description));
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(profile.ProfileId));
		list.Add(MailNotificationProfilePropertyDefinitions.Subject(settings.Get("Subject")));
		list.Add(MailNotificationProfilePropertyDefinitions.MessageText(settings.Get("Text")));
		list.Add(MailNotificationProfilePropertyDefinitions.Recipients(settings.Get("Recipients")));
		list.Add(MailNotificationProfilePropertyDefinitions.TimeBetweenMails(settings.Get("TimeBetweenMails")));
		list.Add(MailNotificationProfilePropertyDefinitions.IncludeImages(settings.Get("IncludeImages")));
		list.Add(MailNotificationProfilePropertyDefinitions.NumberOfImages(settings.Get("NumberOfImages")));
		list.Add(MailNotificationProfilePropertyDefinitions.EmbedImages(settings.Get("EmbedImages")));
		list.Add(MailNotificationProfilePropertyDefinitions.TimeBetweenImages(settings.Get("TimeBetweenImages")));
		list.Add(MailNotificationProfilePropertyDefinitions.IncludeAVI(settings.Get("IncludeAVI")));
		list.Add(MailNotificationProfilePropertyDefinitions.TimeBeforeEvent(settings.Get("TimeBeforeEvent")));
		list.Add(MailNotificationProfilePropertyDefinitions.TimeAfterEvent(settings.Get("TimeAfterEvent")));
		list.Add(MailNotificationProfilePropertyDefinitions.FrameRate(settings.Get("FrameRate")));
		obj.Properties = list.ToArray();
		return obj;
	}
}

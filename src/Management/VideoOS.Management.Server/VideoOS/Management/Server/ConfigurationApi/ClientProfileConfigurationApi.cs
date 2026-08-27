#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using VideoOS.Common.Integration;
using VideoOS.Common.SharedComponent;
using VideoOS.Common.SharedComponent.ClientProfile;
using VideoOS.Common.SharedComponent.Options;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIClientProfile")]
[EntityName("ConfigAPIClientProfileFolder")]
public class ClientProfileConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description, out var changedSettings, out var disabledLayoutGuids);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			ClientProfileSettings fromXml = ClientProfileSettings.GetFromXml(ClientProfile.QueryClientProfileById(requestContext, path.Id).SettingsXml);
			foreach (ClientProfileSectionSpecification section in new ClientProfileSpecification().Sections)
			{
				if ((section.App != "MC" && section.App != "All") || (!string.IsNullOrEmpty(section.Feature) && !GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, section.Feature)))
				{
					continue;
				}
				foreach (ClientProfileSettingSpecification setting in section.Settings)
				{
					if ((string.IsNullOrEmpty(setting.Feature) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, setting.Feature)) && !setting.IsSeperator)
					{
						if (changedSettings.ContainsKey(setting.ID))
						{
							fromXml[setting.ID].Value = changedSettings[setting.ID];
						}
						if (changedSettings.ContainsKey(setting.ID + "Locked"))
						{
							bool locked = bool.Parse(changedSettings[setting.ID + "Locked"]);
							fromXml[setting.ID].Locked = locked;
						}
					}
				}
			}
			string name2 = Enum.GetName(typeof(OptionId), OptionId.ViewsLayoutsDisabled);
			fromXml[name2].Value = disabledLayoutGuids;
			ClientProfile clientProfile = ClientProfile.UpdateClientProfile(requestContext, path.Id, name, description, ClientProfileSettings.GetAsXml(fromXml), ClientProfileType.SmartClientProfile);
			ConfigurationItem resultItem = ConvertToClientProfileConfigurationItem(requestContext, parentPath, clientProfile);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		try
		{
			ClientProfile.QueryClientProfileById(requestContext, path.Id);
			string name;
			string description;
			Dictionary<string, string> changedSettings;
			string disabledLayoutGuids;
			return ValidateItemInternal(requestContext, item, path, out name, out description, out changedSettings, out disabledLayoutGuids);
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out Dictionary<string, string> changedSettings, out string disabledLayoutGuids)
	{
		name = null;
		description = null;
		changedSettings = new Dictionary<string, string>();
		disabledLayoutGuids = "";
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		try
		{
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
			}
			ThrowIfDuplicateName(requestContext, name, path.Id);
			List<ErrorResult> list = new List<ErrorResult>();
			if (item.Children != null)
			{
				ClientProfileSpecification clientProfileSpecification = new ClientProfileSpecification();
				ConfigurationItem[] children = item.Children;
				foreach (ConfigurationItem configurationItem in children)
				{
					if (configurationItem.Properties == null)
					{
						continue;
					}
					string groupdId = configurationItem.ItemType.Substring("ClientProfile".Length);
					Property[] properties = configurationItem.Properties;
					foreach (Property property in properties)
					{
						string guidList;
						ErrorResult errorResult = ValidateProperty(requestContext, clientProfileSpecification, property, groupdId, out guidList);
						if (errorResult == null)
						{
							if (guidList == null)
							{
								changedSettings[property.Key] = property.Value;
							}
							else
							{
								disabledLayoutGuids = guidList;
							}
						}
						else
						{
							list.Add(errorResult);
						}
					}
				}
			}
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = !list.Any();
			validateResult.ErrorResults = list.ToArray();
			validateResult.ResultItem = item;
			return validateResult;
		}
		catch (PropertyInvalidException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			string[] clientProfileIds = ClientProfile.QueryClientProfileIds(requestContext, ClientProfileType.SmartClientProfile);
			ClientProfile[] source = ClientProfile.QueryClientProfilesByIds(requestContext, clientProfileIds);
			int ix;
			for (ix = clientProfileIds.Length - 1; ix >= 0; ix--)
			{
				ClientProfile clientProfile = source.FirstOrDefault((ClientProfile p) => p.Id == clientProfileIds[ix]);
				if (clientProfile != null)
				{
					list.Add(ConvertToClientProfileConfigurationItem(requestContext, path, clientProfile));
				}
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			throw new LicenseException();
		}
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("ClientProfileFolder", ConfigurationItemPath.CreateSystemPath());
		}
		try
		{
			ClientProfile clientProfile = ClientProfile.QueryClientProfileById(requestContext, path.Id);
			ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "ClientProfileFolder").CreatePath();
			return ConvertToClientProfileConfigurationItem(requestContext, parentPath, clientProfile);
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddClientProfile":
			if (!flag)
			{
				return CreateInvokeInfoAddClientProfile(item, method);
			}
			return AddClientProfile(requestContext, item, path);
		case "RemoveClientProfile":
			if (!flag)
			{
				return CreateInvokeInfoRemoveClientProfile(item, GetChildItems(requestContext, path), method);
			}
			return RemoveClientProfile(requestContext, item, path);
		case "ClientProfileUpPriority":
			if (!flag)
			{
				return CreateInvokeInfoMovePriority(requestContext, item, path, method);
			}
			return ClientProfileMovePriority(requestContext, item, path, up: true);
		case "ClientProfileDownPriority":
			if (!flag)
			{
				return CreateInvokeInfoMovePriority(requestContext, item, path, method);
			}
			return ClientProfileMovePriority(requestContext, item, path, up: false);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	private static ConfigurationItem CreateInvokeInfoMovePriority(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		if (path.IsFolder || path.ItemType != "ClientProfile")
		{
			throw new PathInvalidException();
		}
		try
		{
			if (ClientProfile.QueryClientProfileById(requestContext, path.Id).IsDefaultProfile)
			{
				throw new ArgumentException("Cannot prioritize default profile");
			}
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException("Profile not found");
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ((methodId == "ClientProfileUpPriority") ? ConfigurationApiResources.MethodClientProfileUpPriority : ConfigurationApiResources.MethodClientProfileDownPriority);
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[0];
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoAddClientProfile(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddClientProfile;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty("")
		};
		return configurationItem;
	}

	private static ConfigurationItem AddClientProfile(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string id = Guid.NewGuid().ToString();
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Name", out string value);
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Description", out string value2);
		ThrowIfDuplicateName(requestContext, value, id);
		try
		{
			ClientProfile.UpdateClientProfileValidation(requestContext, ref id, value, ref value2);
			ClientProfile clientProfile = LookupDefaultProfile(requestContext);
			ClientProfile clientProfile2 = ClientProfile.CreateClientProfile(requestContext, id, value, value2, clientProfile.SettingsXml, ClientProfileType.SmartClientProfile);
			ConfigurationItem configurationItem2 = ConvertToClientProfileConfigurationItem(requestContext, parentPath, clientProfile2);
			return MethodTypesHelper.CreateInvokeResult(configurationItem2, configurationItem2.Path, "Result", "Success", true);
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
	}

	private static ConfigurationItem CreateInvokeInfoRemoveClientProfile(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveClientProfile, "PropertyRemoveClientProfile", ConfigurationApiResources.PropertyRemoveClientProfile, item, members);
	}

	private static ConfigurationItem RemoveClientProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			if (ClientProfile.QueryClientProfileById(requestContext, invokeMemberItemSelectionPath.Id).IsDefaultProfile)
			{
				throw new ArgumentException("Cannot remove default profile");
			}
			ClientProfile.DeleteClientProfile(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException("Profile not found");
		}
		catch (ClientProfileDeleteException)
		{
			throw new PathNotFoundException("Profile not found");
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem ClientProfileMovePriority(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, bool up)
	{
		if (path.IsFolder || path.ItemType != "ClientProfile")
		{
			throw new PathInvalidException();
		}
		try
		{
			if (ClientProfile.QueryClientProfileById(requestContext, path.Id).IsDefaultProfile)
			{
				throw new ArgumentException("Cannot prioritize default profile");
			}
			if (up)
			{
				ClientProfile.UpdateClientProfileMoveUp(requestContext, path.Id);
			}
			else
			{
				ClientProfile.UpdateClientProfileMoveDown(requestContext, path.Id);
			}
		}
		catch (ClientProfileNotExistException)
		{
			throw new PathNotFoundException("Profile not found");
		}
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
	}

	private static ClientProfile LookupDefaultProfile(IGeminiRequestContext requestContext)
	{
		string[] ids = ClientProfile.QueryClientProfileIds(requestContext, ClientProfileType.SmartClientProfile);
		return ClientProfile.QueryClientProfilesByIds(requestContext, ids).First((ClientProfile p) => p.IsDefaultProfile);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, string objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		string[] ids = ClientProfile.QueryClientProfileIds(requestContext, ClientProfileType.SmartClientProfile);
		ClientProfile[] array = ClientProfile.QueryClientProfilesByIds(requestContext, ids);
		foreach (ClientProfile clientProfile in array)
		{
			if (!string.Equals(clientProfile.Id, objectId, StringComparison.OrdinalIgnoreCase) && clientProfile.Name == name)
			{
				throw new PropertyInvalidException("Duplicate name", "Name");
			}
		}
	}

	private static ErrorResult ValidateProperty(IGeminiRequestContext requestContext, ClientProfileSpecification clientProfileSpecification, Property property, string groupdId, out string guidList)
	{
		guidList = null;
		if (property.ValueType == "Int" && !int.TryParse(property.Value, out var _))
		{
			return new ErrorResult
			{
				ErrorProperty = property.Key,
				ErrorText = "Not an integer"
			};
		}
		if (property.ValueType == "Tick" && !bool.TryParse(property.Value, out var _))
		{
			return new ErrorResult
			{
				ErrorProperty = property.Key,
				ErrorText = "Not true or false"
			};
		}
		if (property.ValueType == "Enum")
		{
			ClientProfileSectionSpecification clientProfileSectionSpecification = clientProfileSpecification.Sections.FirstOrDefault((ClientProfileSectionSpecification s) => s.ID == groupdId);
			if (clientProfileSectionSpecification == null)
			{
				return new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = "Unknown parent"
				};
			}
			ClientProfileSettingSpecification clientProfileSettingSpecification = clientProfileSectionSpecification.Settings.FirstOrDefault((ClientProfileSettingSpecification s) => s.ID == property.Key);
			if (clientProfileSettingSpecification == null)
			{
				return new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = "Unknown property"
				};
			}
			if (clientProfileSettingSpecification.Control.ComboBoxPluginClassName != null && clientProfileSettingSpecification.Control.ComboBoxPluginClassName.EndsWith("ComboBoxPluginTimeZone", StringComparison.OrdinalIgnoreCase))
			{
				if (string.IsNullOrEmpty(property.Value))
				{
					return null;
				}
				if (TimeZoneInfo.GetSystemTimeZones().FirstOrDefault((TimeZoneInfo z) => z.Id == property.Value) == null)
				{
					return new ErrorResult
					{
						ErrorProperty = property.Key,
						ErrorText = "Enum timezone value invalid"
					};
				}
				return null;
			}
			if (clientProfileSettingSpecification.Control.Elements == null)
			{
				return new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = "No defined enum values"
				};
			}
			if (clientProfileSettingSpecification.Control.Elements.FirstOrDefault((ClientProfileControlValueSpecification e) => e.ID == property.Value) == null)
			{
				return new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = "Enum value invalid"
				};
			}
		}
		if (property.ValueType == "PathList")
		{
			try
			{
				guidList = ConvertPathListToGuids(requestContext, property.Value);
			}
			catch (ViewLayoutNotFoundException)
			{
				return new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = "Path invalid"
				};
			}
			catch (ArgumentException)
			{
				return new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = "Path invalid"
				};
			}
		}
		_ = property.ValueType == "String";
		return null;
	}

	private static string ConvertPathListToGuids(IGeminiRequestContext requestContext, string pathList)
	{
		string[] array = pathList.Split(';');
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(";");
				}
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(text);
				if (configurationItemPath.ItemType != "Layout")
				{
					throw new ArgumentException("Invalid itemtype");
				}
				ViewLayout.QueryViewLayoutById(requestContext, configurationItemPath.Id);
				stringBuilder.Append(configurationItemPath.Id);
			}
		}
		return stringBuilder.ToString();
	}

	private static ConfigurationItem ConvertToClientProfileConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, ClientProfile clientProfile)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "ClientProfile";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		if (clientProfile.IsDefaultProfile)
		{
			configurationItem.DisplayName = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.DefaultNodeInTreeClientProfile, OemResources.Instance.SmartClient.ClientName);
		}
		else
		{
			configurationItem.DisplayName = clientProfile.Name;
		}
		configurationItem.ParentPath = parentPath.Text;
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, clientProfile.Id);
		configurationItem.Path = configurationItemPath.Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(clientProfile.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(clientProfile.Name, !clientProfile.IsDefaultProfile));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(clientProfile.Description, !clientProfile.IsDefaultProfile));
		list.Add(ClientProfilePropertyDefinitions.IsDefaultProfileProperty(clientProfile.IsDefaultProfile));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(clientProfile.LastModified));
		configurationItem.Children = GenerateChildItems(requestContext, clientProfile, configurationItemPath);
		configurationItem.ChildrenFilled = true;
		configurationItem.Properties = list.ToArray();
		List<string> list2 = new List<string>();
		if (!clientProfile.IsDefaultProfile)
		{
			list2.Add("ClientProfileUpPriority");
			list2.Add("ClientProfileDownPriority");
		}
		configurationItem.MethodIds = list2.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem[] GenerateChildItems(IGeminiRequestContext requestContext, ClientProfile clientProfile, ConfigurationItemPath parentPath)
	{
		Dictionary<string, ConfigurationItem> dictionary = new Dictionary<string, ConfigurationItem>();
		ClientProfileSettings fromXml = ClientProfileSettings.GetFromXml(clientProfile.SettingsXml);
		foreach (ClientProfileSectionSpecification section in new ClientProfileSpecification().Sections)
		{
			if ((!(section.App != "MC") || !(section.App != "All")) && (string.IsNullOrEmpty(section.Feature) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, section.Feature)))
			{
				List<Property> properties = GenerateSectionProperties(requestContext, section, fromXml);
				dictionary.Add(section.ID, GenerateClientProfileChild(section, parentPath, properties));
			}
		}
		string name = Enum.GetName(typeof(OptionId), OptionId.ViewsLayoutsDisabled);
		fromXml.TryGetValue(name, out var clientProfileSetting);
		dictionary.Add("ClientProfileViewLayout", CreateViewLayoutItem(parentPath, clientProfileSetting));
		return dictionary.Values.ToArray();
	}

	private static ConfigurationItem GenerateClientProfileChild(ClientProfileSectionSpecification section, ConfigurationItemPath parentPath, List<Property> properties)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ClientProfile" + section.ID,
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = section.LocalizedName,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, parentPath.Id).Text;
		new List<Property>().Add(GeneralPropertyDefinitions.GetNameProperty(section.LocalizedName, isSettable: false));
		obj.Children = null;
		obj.ChildrenFilled = true;
		obj.Properties = properties.ToArray();
		obj.MethodIds = new string[0];
		return obj;
	}

	private static List<Property> GenerateSectionProperties(IGeminiRequestContext requestContext, ClientProfileSectionSpecification section, ClientProfileSettings settings)
	{
		List<Property> list = new List<Property>();
		foreach (ClientProfileSettingSpecification setting in section.Settings)
		{
			if ((!string.IsNullOrEmpty(setting.Feature) && !GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, setting.Feature)) || setting.IsSeperator)
			{
				continue;
			}
			bool flag = false;
			Property property = null;
			switch (setting.Control.ControlType)
			{
			case "ComboBox":
				property = ClientProfilePropertyDefinitions.GenericEnumProperty(setting, setting.DefaultValue);
				break;
			case "CustomComboBox":
			{
				string valueType = "Enum";
				ValueTypeInfo[] valueTypeInfos = null;
				if (!string.IsNullOrEmpty(setting.Control.ComboBoxPluginClassName))
				{
					string[] array2 = setting.Control.ComboBoxPluginClassName.Split('.');
					string text2 = array2[array2.Length - 1];
					if (text2 == "ComboBoxPluginTimeZone")
					{
						List<ValueTypeInfo> list2 = new List<ValueTypeInfo>();
						foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
						{
							list2.Add(new ValueTypeInfo
							{
								Name = systemTimeZone.DisplayName,
								Value = systemTimeZone.Id
							});
						}
						valueTypeInfos = list2.ToArray();
					}
					else
					{
						Trace.WriteLine("ConfigurationAPI.ClientProfile missing handling of ComboBoxPluginClass type: " + text2);
					}
					property = ClientProfilePropertyDefinitions.GenericProperty(setting, valueType, valueTypeInfos, setting.DefaultValue);
				}
				else
				{
					property = ClientProfilePropertyDefinitions.GenericEnumProperty(setting, "");
				}
				break;
			}
			case "Button":
				property = ((!setting.Control.ReportDisplayValueAsPicture) ? ClientProfilePropertyDefinitions.GenericStringProperty(setting, "") : ClientProfilePropertyDefinitions.GenericImageProperty(setting, ""));
				break;
			case "TextBox":
			{
				string valueType = "String";
				ValueTypeInfo[] valueTypeInfos = null;
				if (!string.IsNullOrEmpty(setting.Control.ValidationPluginClassName))
				{
					string[] array = setting.Control.ValidationPluginClassName.Split('.');
					string text = array[array.Length - 1];
					if (!(text == "ValidationPluginInactiveTimeout"))
					{
						if (!(text == "ValidationPluginExportDefaultFilePath"))
						{
							Trace.WriteLine("ConfigurationAPI.ClientProfile missing handling of validation type: " + text);
						}
					}
					else
					{
						valueType = "Int";
						valueTypeInfos = new ValueTypeInfo[2]
						{
							new ValueTypeInfo
							{
								Name = "MinValue",
								Value = "0"
							},
							new ValueTypeInfo
							{
								Name = "MaxValue",
								Value = "10080"
							}
						};
					}
				}
				string resourceString = ResourcesUtility.GetResourceString(setting.DefaultValue);
				if (resourceString == null)
				{
					resourceString = setting.DefaultValue;
				}
				property = ClientProfilePropertyDefinitions.GenericProperty(setting, valueType, valueTypeInfos, resourceString);
				break;
			}
			case "TextBoxButton":
			{
				string resourceString = ResourcesUtility.GetResourceString(setting.DefaultValue);
				property = ClientProfilePropertyDefinitions.GenericStringProperty(setting, resourceString);
				break;
			}
			default:
				flag = true;
				break;
			}
			if (flag)
			{
				continue;
			}
			if (settings.Contains(setting.ID))
			{
				property.Value = settings[setting.ID].Value;
			}
			list.Add(property);
			if (setting.LockedAvailable)
			{
				bool defaultLocked = setting.DefaultLocked;
				if (settings.Contains(setting.ID))
				{
					defaultLocked = settings[setting.ID].Locked;
				}
				list.Add(ClientProfilePropertyDefinitions.LockedProperty(setting, defaultLocked));
			}
		}
		return list;
	}

	private static ConfigurationItem CreateViewLayoutItem(ConfigurationItemPath parentPath, ClientProfileSetting setting)
	{
		string itemType = "ClientProfileViewLayouts";
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = itemType,
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = ConfigurationApiResources.ClientProfileViewLayouts,
			ParentPath = parentPath.Text,
			Path = ConfigurationItemPath.CreateItemPath(itemType, parentPath.Id).Text,
			ChildrenFilled = true,
			Children = new ConfigurationItem[0]
		};
		List<Property> list = new List<Property>();
		string[] obj = ((setting == null) ? new string[0] : setting.Value.Split(';'));
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		string[] array = obj;
		foreach (string text in array)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append(ConfigurationItemPath.CreateItemPath("Layout", text.Trim()).Text);
			}
		}
		list.Add(ClientProfilePropertyDefinitions.ViewLayoutProperty(stringBuilder.ToString()));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}
}

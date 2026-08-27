using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedComponent;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIViewGroup")]
[EntityName("ConfigAPIViewGroupFolder")]
public class ViewGroupConfigurationApi : SecurityConfigurationApi, IConfigurationApiSupport
{
	private const string GroupNodeName = "group";

	private const string DisplayNameAttributeName = "displayname";

	public const string ViewGroupDataXmlKey = "ViewGroupDataXml";

	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>> { 
	{
		"ViewGroup",
		new List<string> { "CREATE_VIEWGROUP", "ADMIN_SECURITY" }
	} };

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 41);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 42);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 43);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 44);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description, out var customProperty);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			try
			{
				ServerGroup serverGroup = ServerGroup.QueryServerGroupById(requestContext, path.Id);
				serverGroup.CustomProperties = CustomProperty.GetCustomProperties(requestContext, serverGroup.Id);
				AlignNameAndCustomProperty(serverGroup, name, customProperty);
				serverGroup.Description = description;
				ServerGroup serverGroup2 = ServerGroup.UpdateServerGroup(requestContext, serverGroup);
				serverGroup2.CustomProperties = CustomProperty.GetCustomProperties(requestContext, serverGroup2.Id);
				ConfigurationItem resultItem = ConvertToConfigurationItem(parentPath, serverGroup2);
				validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = resultItem;
				return validateResult;
			}
			catch (ServerGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		throw new NotSupportedException();
	}

	private static void AlignNameAndCustomProperty(ServerGroup currentServerGroup, string name, CustomProperty customProperty)
	{
		CustomProperty customProperty2 = null;
		XmlDocument xmlDocument = null;
		string text = null;
		if (currentServerGroup.CustomProperties != null)
		{
			customProperty2 = currentServerGroup.CustomProperties.FirstOrDefault();
			if (customProperty2 != null && !string.IsNullOrEmpty(customProperty2.Value))
			{
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(customProperty2.Value);
				text = xmlDocument["group"].Attributes["displayname"].Value;
			}
		}
		if (customProperty == null || string.IsNullOrEmpty(customProperty.Value))
		{
			currentServerGroup.Name = name;
			if (xmlDocument != null)
			{
				xmlDocument["group"].Attributes["displayname"].Value = name;
				EnsureSmartClientIdIsSet(xmlDocument, enforce: true);
				customProperty2.Value = xmlDocument.OuterXml;
			}
			return;
		}
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.LoadXml(customProperty.Value);
		string value = xmlDocument2["group"].Attributes["displayname"].Value;
		if (text != value)
		{
			currentServerGroup.Name = value;
		}
		else if (currentServerGroup.Name != name)
		{
			currentServerGroup.Name = name;
			xmlDocument2["group"].Attributes["displayname"].Value = name;
		}
		EnsureSmartClientIdIsSet(xmlDocument2, enforce: false);
		customProperty.Value = xmlDocument2.OuterXml;
		currentServerGroup.CustomProperties = new CustomProperty[1] { customProperty };
	}

	private static void EnsureSmartClientIdIsSet(XmlDocument customPropertyXml, bool enforce)
	{
		XmlAttribute xmlAttribute = customPropertyXml["group"].Attributes["smartClientId"];
		if (xmlAttribute == null)
		{
			customPropertyXml["group"].Attributes.Append(customPropertyXml.CreateAttribute("smartClientId")).Value = Guid.NewGuid().ToString();
		}
		else if (enforce || string.IsNullOrEmpty(xmlAttribute.Value))
		{
			xmlAttribute.Value = Guid.NewGuid().ToString();
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 155);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 156);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 157);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 158);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		string name;
		string description;
		CustomProperty customProperty;
		if (!path.IsFolder)
		{
			return ValidateItemInternal(requestContext, item, path, out name, out description, out customProperty);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out CustomProperty customProperty)
	{
		name = null;
		description = null;
		customProperty = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
					if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ViewGroupDataXml", out string value) && !string.IsNullOrEmpty(value))
					{
						XmlDocument xmlDocument = new XmlDocument();
						try
						{
							xmlDocument.LoadXml(value);
							customProperty = new CustomProperty
							{
								ObjectId = path.Id,
								Name = VMOConstants.CustomPropertyXmlDefinition,
								Value = value
							};
						}
						catch (XmlException ex)
						{
							throw new ArgumentException(ex.Message, "ViewGroupDataXml", ex);
						}
					}
				}
				try
				{
					ServerGroup serverGroup = ServerGroup.QueryServerGroupById(requestContext, path.Id);
					if (serverGroup.StructureType != ServerGroup.EnumStructureType.ViewGroup)
					{
						throw new PathNotFoundException(path.Text);
					}
					string nameToCheck = name;
					ServerGroup serverGroup2 = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.ViewGroup, serverGroup.ParentId).FirstOrDefault((ServerGroup s) => s.Name == nameToCheck);
					if (serverGroup2 != null && new Guid(serverGroup2.Id) != new Guid(path.Id))
					{
						throw new PropertyInvalidException("Group name already exists", "Name");
					}
				}
				catch (ServerGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
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

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 251);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 252);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			ServerGroup[] array = null;
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.ViewGroup, "430C2FE2-B233-42DA-B508-1A22EF03383D");
			}
			else
			{
				try
				{
					array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.ViewGroup, path.Id);
				}
				catch (ServerGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			ServerGroup[] array2 = array;
			foreach (ServerGroup serverGroup in array2)
			{
				if (serverGroup.Id != "4BF8A8B0-54AB-4E82-994F-98D8E9584176")
				{
					list.Add(ConvertToConfigurationItem(path, serverGroup));
					continue;
				}
				ServerGroup serverGroup2 = ServerGroup.QueryPrivateViewGroup(requestContext, requestContext.AuthenticatedUserSid);
				if (serverGroup2 != null)
				{
					list.Add(ConvertToConfigurationItem(path, serverGroup2));
				}
			}
			return list.ToArray();
		}
		ServerGroup serverGroup3 = null;
		try
		{
			serverGroup3 = ServerGroup.QueryServerGroupById(requestContext, path.Id);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("ViewGroupFolder", path);
		ConfigurationItem configurationItem2 = GroupConfigurationItemFactory.CreateFolderItem("ViewFolder", path);
		if (!ServerGroup.IsPrivateViewGroupRoot(serverGroup3.ParentId) && !ServerGroup.IsSpecialType(serverGroup3.ParentId, "430C2FE2-B233-42DA-B508-1A22EF03383D"))
		{
			return new ConfigurationItem[2] { configurationItem, configurationItem2 };
		}
		return new ConfigurationItem[1] { configurationItem };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 323);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 324);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, ConfigurationItemPath.CreateSystemPath());
			}
			string id = path.Id;
			try
			{
				if (ServerGroup.QueryServerGroupById(requestContext, id).StructureType != ServerGroup.EnumStructureType.ViewGroup)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			catch (ServerGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, id);
			return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
		}
		string id2 = path.Id;
		ServerGroup serverGroup = null;
		try
		{
			serverGroup = ServerGroup.QueryServerGroupById(requestContext, id2);
			CustomProperty[] customProperties = CustomProperty.GetCustomProperties(requestContext, id2);
			serverGroup.CustomProperties = customProperties;
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		string parentId = serverGroup.ParentId;
		ConfigurationItemPath configurationItemPath = null;
		string folderType = "ViewGroupFolder";
		configurationItemPath = ((!string.IsNullOrEmpty(parentId) && !ServerGroup.IsSpecialType(parentId, "430C2FE2-B233-42DA-B508-1A22EF03383D") && !ServerGroup.IsPrivateViewGroupRoot(serverGroup.ParentId)) ? ConfigurationItemPath.CreateFolderPath("ViewGroup", parentId, folderType) : ConfigurationItemPath.CreateSystemFolderPath(folderType));
		return ConvertToConfigurationItem(configurationItemPath, serverGroup);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		item.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 386);
		methodId.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ViewGroupConfigurationApi.cs", 387);
		switch (methodId)
		{
		case "AddViewGroup":
			if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return CreateAddViewGroup(item, methodId);
			}
			return AddViewGroup(requestContext, item, path);
		case "RemoveViewGroup":
			if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return CreateRemoveViewGroup(item, GetChildItems(requestContext, path), methodId);
			}
			return RemoveViewGroup(requestContext, item, path);
		case "ChangeSecurityPermissions":
			if (ServerGroup.IsPrivateViewGroupRoot(parentPath.Id) || !string.IsNullOrEmpty(parentPath.Id))
			{
				throw new NotSupportedException();
			}
			return InvokeSecurityMethod(requestContext, item, path, methodId);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				if (!path.Id.IsGuid())
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "ViewGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "ViewGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem AddViewGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal) && !string.Equals(parentPath.ParentItemType, "ViewGroup", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string text = null;
		text = (string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal) ? "430C2FE2-B233-42DA-B508-1A22EF03383D" : parentPath.Id.ToUpperInvariant());
		CustomProperty[] array = null;
		if (!((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Id", out string value))
		{
			value = Guid.NewGuid().ToString();
		}
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Name", out string name);
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Description", out string value2);
		if (((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ViewGroupDataXml", out string value3) && !string.IsNullOrEmpty(value3))
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(value3);
				array = new CustomProperty[1]
				{
					new CustomProperty
					{
						ObjectId = text,
						Name = VMOConstants.CustomPropertyXmlDefinition,
						Value = value3
					}
				};
			}
			catch (XmlException ex)
			{
				throw new ArgumentException(ex.Message, "ViewGroupDataXml", ex);
			}
		}
		if (ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.ViewGroup, text).FirstOrDefault((ServerGroup s) => s.Name == name) != null)
		{
			throw new PropertyInvalidException("Group name already exists", "Name");
		}
		ServerGroup serverGroup = new ServerGroup
		{
			Id = value,
			ParentId = text,
			Name = name,
			Description = value2,
			StructureType = ServerGroup.EnumStructureType.ViewGroup
		};
		if (array != null)
		{
			serverGroup.CustomProperties = array;
		}
		else
		{
			serverGroup.CustomProperties = new CustomProperty[1]
			{
				new CustomProperty
				{
					Name = "CustomPropertyXmlDefinition",
					Value = BuildCustomProperty(serverGroup)
				}
			};
		}
		try
		{
			serverGroup = ServerGroup.CreateServerGroup(requestContext, serverGroup);
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (ServerGroupMaxDepthExceededException)
		{
			throw new ArgumentException("Too many levels created");
		}
		catch (ServerGroupCreationException)
		{
			throw new PropertyInvalidException("Group ID already exists", "Id");
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		ConfigurationItem configurationItem2 = ConvertToConfigurationItem(parentPath, serverGroup);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem2.Path, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveViewGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
	{
		if (string.Equals(parentPath.ParentItemType, "ViewGroup", StringComparison.Ordinal))
		{
			if (!parentPath.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
			}
		}
		else if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string value = null;
		if (!((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		if (value == null)
		{
			throw ExceptionFactory.CreateArgumentNullException("ItemSelection");
		}
		bool result = false;
		if (((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("RemoveMembers", out string value2) && !bool.TryParse(value2, out result))
		{
			throw new PropertyInvalidException("Invalid value, expected True/False", "RemoveMembers");
		}
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid path. " + value, "ItemSelection", inner);
		}
		if (configurationItemPath.IsFolder || !string.Equals("ViewGroup", configurationItemPath.ItemType, StringComparison.Ordinal) || !configurationItemPath.Id.IsGuid())
		{
			throw new PropertyInvalidException("Invalid path. " + value, "ItemSelection");
		}
		try
		{
			if (ServerGroup.IsPrivateViewGroupRoot(ServerGroup.QueryServerGroupById(requestContext, configurationItemPath.Id).ParentId))
			{
				throw new PropertyInvalidException("Cannot remove private view group");
			}
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(configurationItemPath.Text);
		}
		string id = configurationItemPath.Id;
		try
		{
			if (result)
			{
				DeleteMembersAndGroups(requestContext, id);
			}
			else
			{
				ServerGroup.DeleteServerGroup(requestContext, id);
			}
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(configurationItemPath.Text);
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(configurationItem);
	}

	private static void DeleteMembersAndGroups(IGeminiRequestContext requestContext, string groupId)
	{
		ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.ViewGroup, groupId);
		foreach (ServerGroup serverGroup in array)
		{
			DeleteMembersAndGroups(requestContext, serverGroup.Id);
		}
		View[] array2 = View.QueryViewHeriarchyByServerGroupIdNoSecurity(requestContext, groupId);
		foreach (View view in array2)
		{
			View.DeleteView(requestContext, view.Id);
		}
		ServerGroup.DeleteServerGroup(requestContext, groupId);
	}

	public static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, ServerGroup group)
	{
		string displayName = (ServerGroup.IsPrivateViewGroupRoot(group.ParentId) ? ConfigurationApiResources.ViewGroupPrivate : group.Name);
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("ViewGroup", group.Id);
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "ViewGroup",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = displayName,
			ParentPath = parentPath.Text,
			Path = configurationItemPath.Text
		};
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(group.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(group.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(group.Description));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(group.LastModified));
		list.Add(GetViewGroupDataXmml(group.CustomProperties?.FirstOrDefault()));
		configurationItem.MethodIds = new string[0];
		if (!ServerGroup.IsPrivateViewGroupRoot(group.ParentId) && ServerGroup.IsSpecialType(group.ParentId, "430C2FE2-B233-42DA-B508-1A22EF03383D"))
		{
			configurationItem.MethodIds = SecurityConfigurationApiBase.AddSecurityMethods(configurationItem.MethodIds);
		}
		ConfigurationItem configurationItem2 = GroupConfigurationItemFactory.CreateFolderItem("ViewGroupFolder", configurationItemPath);
		ConfigurationItem configurationItem3 = GroupConfigurationItemFactory.CreateFolderItem("ViewFolder", configurationItemPath);
		if (ServerGroup.IsPrivateViewGroupRoot(group.ParentId) || ServerGroup.IsSpecialType(group.ParentId, "430C2FE2-B233-42DA-B508-1A22EF03383D"))
		{
			configurationItem.Children = new ConfigurationItem[1] { configurationItem2 };
		}
		else
		{
			configurationItem.Children = new ConfigurationItem[2] { configurationItem2, configurationItem3 };
		}
		configurationItem.ChildrenFilled = true;
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateRemoveViewGroup(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "InvokeInfo",
			UIHint = "DirectionVertical",
			DisplayName = ConfigurationApiResources.WizardRemoveViewGroup,
			ParentPath = item.ParentPath,
			Path = item.Path
		};
		List<Property> list = new List<Property>();
		list.Add(DeviceGroupPropertyDefinition.RemoveMembersProperty(removeMembers: false));
		configurationItem.MethodIds = new string[1] { methodId };
		IEnumerable<ConfigurationItem> source = members.Where((ConfigurationItem m) => m.DisplayName != "Private");
		list.Add(GetMembersAsEnumsProperty(source.ToArray()));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateAddViewGroup(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "InvokeInfo",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.WizardAddViewGroup,
			ParentPath = item.ParentPath,
			Path = item.Path
		};
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.NewGuid(), uiVisible: true, isSettable: true));
		list.Add(GeneralPropertyDefinitions.GetNameProperty("New View Group"));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(""));
		list.Add(GetViewGroupDataXmml(null));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static Property GetViewGroupDataXmml(CustomProperty customProperty)
	{
		string value = ((customProperty == null) ? "" : customProperty.Value);
		return new Property("ViewGroupDataXml", value)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewGroupDataXml,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property GetMembersAsEnumsProperty(ConfigurationItem[] members)
	{
		Property property = new Property("ItemSelection", "")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySelectMemberToRemove,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (ConfigurationItem configurationItem in members)
		{
			list.Add(new ValueTypeInfo
			{
				Name = configurationItem.DisplayName,
				TranslationId = "",
				Value = configurationItem.Path
			});
		}
		property.ValueTypeInfos = list.ToArray();
		return property;
	}

	private static string BuildCustomProperty(ServerGroup viewGroup)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			OmitXmlDeclaration = true
		});
		xmlWriter.WriteStartElement("group");
		xmlWriter.WriteAttributeString("id", viewGroup.Id.ToLower());
		xmlWriter.WriteAttributeString("displayname", viewGroup.Name);
		xmlWriter.WriteAttributeString("shortcut", "");
		xmlWriter.WriteAttributeString("smartClientId", Guid.NewGuid().ToString());
		xmlWriter.WriteStartElement("properties");
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		return stringBuilder.ToString();
	}
}

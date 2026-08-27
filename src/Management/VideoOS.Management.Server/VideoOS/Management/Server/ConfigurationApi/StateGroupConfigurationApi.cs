using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIStateGroupFolder")]
[EntityName("ConfigAPIStateGroup")]
public class StateGroupConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string StatesPropertyKey = "States";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var states);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		StateGroupInfo stateGroupInfo = StateGroup.GetStateGroupInfo(requestContext, new Guid(path.Id));
		if (name != null)
		{
			stateGroupInfo.Name = name;
		}
		if (states != null)
		{
			stateGroupInfo.States = states;
		}
		StateGroup.UpdateStateGroupInfo(requestContext, stateGroupInfo);
		ConfigurationItem resultItem = ConstructItem(stateGroupInfo, parentPath);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = resultItem;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		string name;
		Collection<string> states;
		if (!path.IsFolder)
		{
			return ValidateItemInternal(requestContext, item, path, out name, out states);
		}
		throw new NotSupportedException();
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
			return GroupConfigurationItemFactory.CreateFolderItem("StateGroupFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (Guid.TryParse(path.Id, out var result))
		{
			StateGroupInfo stateGroupInfo = StateGroup.GetStateGroupInfo(requestContext, result);
			if (stateGroupInfo != null)
			{
				return ConstructItem(stateGroupInfo, ConfigurationItemPath.CreateSystemFolderPath("StateGroupFolder"));
			}
			throw new PathNotFoundException();
		}
		throw new PathInvalidException();
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
		if (!path.IsFolder)
		{
			return new ConfigurationItem[0];
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (StateGroupInfo allStateGroup in StateGroup.GetAllStateGroups(requestContext))
		{
			list.Add(ConstructItem(allStateGroup, path));
		}
		return list.ToArray();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(methodId == "AddStateGroup"))
		{
			if (methodId == "RemoveStateGroup")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveStateGroup(item, GetChildItems(requestContext, path), methodId);
				}
				return RemoveStateGroup(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", methodId), methodId);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddStateGroup(item, methodId);
		}
		return AddStateGroup(requestContext, item, path);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out Collection<string> states)
	{
		name = null;
		states = null;
		try
		{
			if (!path.IsFolder)
			{
				Guid guid = new Guid(path.Id);
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("States", out string[] values);
					states = new Collection<string>();
					string[] array = values;
					foreach (string text in array)
					{
						if (text.Contains(","))
						{
							throw new ArgumentException("Comma not allowed in state name", "states");
						}
						states.Add(text.Trim(' '));
					}
				}
				ThrowIfDuplicateName(requestContext, name, guid);
				if (string.IsNullOrEmpty(name))
				{
					throw new ArgumentException("Name cannot be blank");
				}
				Dictionary<Guid, HashSet<string>> dictionary = BuildEventTypeUsage(requestContext);
				if (dictionary.ContainsKey(guid))
				{
					foreach (string item2 in dictionary[guid])
					{
						if (!states.Contains(item2))
						{
							throw new ArgumentException("Cannot remove/change a state that is referenced in an EventType definition");
						}
					}
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

	private static ConfigurationItem CreateInvokeInfoAddStateGroup(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddStateGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[3]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.NewGuid(), uiVisible: true, isSettable: true),
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			GetStatesProperty(Enumerable.Empty<string>())
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveStateGroup(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveStateGroup, "WizardRemoveStateGroup", ConfigurationApiResources.PropertyRemoveStateGroup, item, members);
	}

	private static ConfigurationItem AddStateGroup(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = Guid.NewGuid().ToString();
		if (item.Properties == null)
		{
			throw new ArgumentException("Item.Properties is null");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("States", out string[] values);
		Collection<string> collection = new Collection<string>();
		string[] array = values;
		foreach (string text in array)
		{
			if (text.Contains(","))
			{
				throw new ArgumentException("Comma not allowed in state name", "States");
			}
			collection.Add(text.Trim(' '));
		}
		try
		{
			ThrowIfDuplicateName(requestContext, value2, Guid.Empty);
		}
		catch (ArgumentException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		StateGroupInfo stateGroupInfo = new StateGroupInfo
		{
			Id = new Guid(value),
			Name = value2,
			States = collection
		};
		StateGroup.CreateStateGroupInfo(requestContext, stateGroupInfo);
		ConfigurationItem configurationItem = ConstructItem(stateGroupInfo, path);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveStateGroup(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		if (StateGroup.GetStateGroupInfo(requestContext, new Guid(invokeMemberItemSelectionPath.Id)) == null)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		if (BuildEventTypeUsage(requestContext).ContainsKey(new Guid(invokeMemberItemSelectionPath.Id)))
		{
			throw new PropertyInvalidException("Cannot delete state group in use");
		}
		StateGroup.DeleteStateGroup(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, Guid objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		foreach (StateGroupInfo allStateGroup in StateGroup.GetAllStateGroups(requestContext))
		{
			if (allStateGroup.Id != objectId && allStateGroup.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	private static Dictionary<Guid, HashSet<string>> BuildEventTypeUsage(IGeminiRequestContext requestContext)
	{
		Dictionary<Guid, HashSet<string>> dictionary = new Dictionary<Guid, HashSet<string>>();
		List<string> list = new List<string>();
		foreach (object value in Enum.GetValues(typeof(GeneratorType)))
		{
			list.Add(value.ToString());
		}
		foreach (EventTypeInfo eventType in EventType.GetEventTypes(requestContext, list.ToArray(), null))
		{
			EventDefinition eventDefinition = new EventDefinition(eventType.Definition);
			if (eventDefinition.StateGroupId.HasValue)
			{
				if (!dictionary.ContainsKey(eventDefinition.StateGroupId.Value))
				{
					dictionary.Add(eventDefinition.StateGroupId.Value, new HashSet<string>());
				}
				dictionary[eventDefinition.StateGroupId.Value].Add(eventDefinition.State);
			}
		}
		return dictionary;
	}

	private static ConfigurationItem ConstructItem(StateGroupInfo stateGroupInfo, ConfigurationItemPath parentPath)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("StateGroup", stateGroupInfo.Id.ToString());
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemCategory = ItemCategories.Item,
			ItemType = "StateGroup",
			Path = configurationItemPath.Text,
			DisplayName = stateGroupInfo.Name,
			ParentPath = parentPath.Text,
			UIHint = "DirectionHorizontal",
			Children = new ConfigurationItem[0],
			ChildrenFilled = true
		};
		configurationItem.Properties = new Property[4]
		{
			GeneralPropertyDefinitions.GetNameProperty(stateGroupInfo.Name, isSettable: true),
			GeneralPropertyDefinitions.GetIdPropertyGuid(stateGroupInfo.Id),
			GeneralPropertyDefinitions.GetLastModifiedProperty(stateGroupInfo.LastModified),
			GetStatesProperty(stateGroupInfo.States)
		};
		return configurationItem;
	}

	private static Property GetStatesProperty(IEnumerable<string> states)
	{
		Property property = new Property("States", states.ToArray());
		property.ValueType = "Array";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyStateGroupStates;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyStateGroupStates";
		property.ToolTipTranslationId = "PropertyStateGroupStatesTooltip";
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
		return property;
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
}

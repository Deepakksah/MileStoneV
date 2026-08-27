using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Proxy.RestApi.ConfigApiRelated;

namespace VideoOS.Platform.Proxy.RestApi;

public class RestApiConvert
{
	private static INameConvert _nameConvert = new NameConvert();

	private static readonly Collection<string> isInvokeField = new Collection<string> { "Path", "path", "State", "state", "ErrorText", "errorText" };

	private static List<string> _excludeFields = new List<string>
	{
		"enabled", "displayName", "relations", "stream", "hardwareDeviceEvent", "generalSettings", "hardwarePtzDeviceSetting", "timeProfileAppointmentRoot", "timeProfileAppointmentRecur", "timeProfileAppointmentVariance",
		"patrollingEntry", "viewItem", "licenseFeature"
	};

	private static Collection<string> _alwaysReadOnlyFields = new Collection<string> { "DisplayName", "CreateDate" };

	private static Collection<string> _methodsToBeHandledWithPost = new Collection<string>
	{
		"AddBasicOwnerInfo", "RemoveBasicOwnerInfo", "AddDeviceGroupMember", "RemoveDeviceGroupMember", "AddPtzPreset", "RemovePtzPreset", "AddRoleMember", "RemoveRoleMember", "AddPatrollingEntry", "RemovePatrollingEntry",
		"AddAppointment", "RemoveAppointment", "AddRecurringAppointment", "RemoveRecurringAppointment", "AddStream", "RemoveStream"
	};

	private static Collection<string> _itemTypesThatHasNoChildren = new Collection<string> { "BasicOwnerInformation", "ClientSettings", "EventType", "CustomProperties", "HardwareDeviceEvent", "EvidenceLockProfile", "StateGroup", "TimeProfile", "User" };

	public static string ConvertConfigPathToRestPath(string configPathString)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(configPathString);
		_ = configurationItemPath.IsFolder;
		return ConvertItemTypeToResource(configurationItemPath.ItemType) + "/" + configurationItemPath.Id;
	}

	public static ConfigurationItem ConvertToConfigurationItem(JsonObject resource, string callerItemType, JsonObject tasks, JsonObject definitions, bool fromInvoke)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath("");
		string parentPath = ((callerItemType == "System") ? "" : ("/" + callerItemType + "Folder"));
		string text = "";
		string text2 = "";
		bool flag = false;
		if (!fromInvoke)
		{
			JsonObject child = resource.GetChild("relations");
			if (child == null)
			{
				flag = true;
				text = TrimFolder(callerItemType);
			}
			else
			{
				JsonObject child2 = child.GetChild("self");
				JsonObject child3 = child.GetChild("parent");
				text = ConvertResourceToItemType(child2.GetString("type"));
				text2 = child2.GetString("id");
				configurationItemPath = ((!Guid.TryParse(text2, out var result)) ? new ConfigurationItemPath(text + "[" + text2 + "]") : new ConfigurationItemPath(text, result));
				if (child3 != null)
				{
					string text3 = ConvertResourceToItemType(child3.GetString("type"));
					string g = child3.GetString("id");
					ConfigurationItemPath configurationItemPath2 = ((!(text3 == "Monitor") || !(text == "View")) ? new ConfigurationItemPath(new ConfigurationItemPath(text3, new Guid(g)), text) : new ConfigurationItemPath(text3, new Guid(g)));
					parentPath = configurationItemPath2.Path;
				}
				else if (text == "User")
				{
					parentPath = configurationItemPath.Path;
				}
			}
		}
		else
		{
			text = callerItemType;
		}
		string[] methodIds = TasksToMethodIds(tasks);
		bool? flag2 = resource.GetBool("enabled");
		if (!flag2.HasValue)
		{
			flag2 = resource.GetBool("Enabled");
		}
		EnablePropertyInfo enableProperty = ((!flag2.HasValue) ? null : new EnablePropertyInfo
		{
			DisplayName = "Enabled",
			Enabled = flag2.Value,
			UIToFollowEnabled = true
		});
		ConfigurationItem[] array = ConstructChildItems(resource, definitions, parentPath);
		return new ConfigurationItem
		{
			ItemType = (string.IsNullOrEmpty(text) ? "System" : text),
			Path = configurationItemPath.Path,
			DisplayName = resource.GetString("displayName"),
			ItemCategory = (flag ? "ChildItem" : "Item"),
			ChildrenFilled = (HasNoChildren(text) || array.Length != 0),
			Children = array,
			EnableProperty = enableProperty,
			MethodIds = methodIds,
			ParentPath = parentPath,
			Properties = ConstructProperties(text, text2, resource, definitions),
			SortKey = 100,
			UIHint = string.Empty,
			ServerCookie = null
		};
	}

	private static Property[] ConstructProperties(string itemType, string id, JsonObject resource, JsonObject definitions)
	{
		List<Property> list = new List<Property>();
		bool flag = false;
		if (itemType == "User" && string.IsNullOrEmpty(resource.GetString("name")))
		{
			list.Add(ConstructNameFromDisplayName(resource));
		}
		foreach (string propertyName in resource.GetPropertyNames())
		{
			if (propertyName == "id" || propertyName == "Id")
			{
				flag = true;
			}
			string type = resource.GetType(propertyName);
			bool flag2 = IsPathOrPathList(resource.GetChild(propertyName), definitions?.GetChild(propertyName));
			if (ShouldBeInProperties(itemType, propertyName) && (type != "Object" || flag2))
			{
				Property property = new Property
				{
					Key = FieldToKey(itemType, propertyName, itemType == "MIPItem"),
					DisplayName = propertyName,
					ValueTypeInfos = GetValueTypeInfos(propertyName, definitions)
				};
				SetDefinitionFields(property, propertyName, definitions);
				SetValuefrom(itemType, property, propertyName, resource, definitions, flag2);
				list.Add(property);
			}
			else
			{
				if (!(propertyName == "FQID"))
				{
					continue;
				}
				JsonObject child = resource.GetChild(propertyName);
				foreach (string propertyName2 in child.GetPropertyNames())
				{
					string text = UpperCaseFirst(propertyName2);
					Property item = new Property
					{
						Key = "FQID." + text,
						DisplayName = text,
						IsSettable = false,
						SortKey = 100,
						ToolTipTranslationId = null,
						UIImportance = 2,
						Value = child.GetString(propertyName2)
					};
					list.Add(item);
				}
			}
		}
		if (!flag && !string.IsNullOrEmpty(id))
		{
			list.Add(ConstructIdProperty(id));
		}
		return list.ToArray();
	}

	private static Property ConstructIdProperty(string id)
	{
		return new Property
		{
			Key = "Id",
			DisplayName = "Id",
			IsSettable = false,
			UIImportance = 2,
			Value = id
		};
	}

	private static Property ConstructNameFromDisplayName(JsonObject resource)
	{
		return new Property
		{
			Key = "Name",
			DisplayName = "Name",
			IsSettable = false,
			UIImportance = 2,
			Value = resource.GetString("displayName")
		};
	}

	private static ConfigurationItem[] ConstructChildItems(JsonObject resource, JsonObject definitions, string parentPath)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (string propertyName in resource.GetPropertyNames())
		{
			if (resource.GetType(propertyName) == "Object" && propertyName != "relations" && propertyName != "FQID")
			{
				JsonObject definitions2 = definitions?.GetChild(propertyName);
				ConfigurationItem configurationItem = ConstructChildItems(propertyName, parentPath, resource.GetChild(propertyName), definitions2);
				if (configurationItem != null)
				{
					list.Add(configurationItem);
				}
			}
			if (!resource.GetChild(propertyName).IsArrayOfObjects)
			{
				continue;
			}
			JsonObject child = resource.GetChild(propertyName);
			JsonObject jsonObject = definitions?.GetChild(propertyName);
			int num = 0;
			bool flag = jsonObject?.IsArray ?? false;
			foreach (JsonObject child2 in child.GetChildren())
			{
				JsonObject definitions3 = ((!flag) ? jsonObject : jsonObject.GetChildAt(num++));
				ConfigurationItem configurationItem2 = ConstructChildItems(propertyName, parentPath, child2, definitions3);
				if (configurationItem2 != null)
				{
					list.Add(configurationItem2);
				}
			}
		}
		return list.ToArray();
	}

	private static bool IsPathOrPathList(JsonObject property, JsonObject definition)
	{
		if (property.GetPropertyNames().Count == 2 && !string.IsNullOrEmpty(property.GetString("id")) && !string.IsNullOrEmpty(property.GetString("type")))
		{
			return true;
		}
		if (definition != null)
		{
			bool? flag = definition.GetBool("link");
			if (flag.HasValue && flag.Value)
			{
				return true;
			}
		}
		return false;
	}

	private static ConfigurationItem ConstructChildItems(string fieldName, string parentPath, JsonObject child, JsonObject definitions)
	{
		if (IsPathOrPathList(child, definitions))
		{
			return null;
		}
		bool? flag = child.GetBool("enabled");
		if (!flag.HasValue)
		{
			flag = child.GetBool("Enabled");
		}
		EnablePropertyInfo enableProperty = ((!flag.HasValue) ? null : new EnablePropertyInfo
		{
			DisplayName = "Enabled",
			Enabled = flag.Value,
			UIToFollowEnabled = true
		});
		string text = child.GetString("id");
		string text2 = FieldToKey("", _nameConvert.ChildFieldNameToItemType(fieldName), noCasingOfMIPItems: false);
		string itemCategory = "ChildItem";
		if (string.IsNullOrEmpty(text))
		{
			JsonObject child2 = child.GetChild("relations");
			if (child2 != null)
			{
				JsonObject child3 = child2.GetChild("self");
				if (child3 != null)
				{
					text = child3.GetString("id");
					text2 = FieldToKey("", _nameConvert.ChildFieldNameToItemType(child3.GetString("type")), noCasingOfMIPItems: false);
					itemCategory = "Item";
				}
			}
		}
		string text3 = (string.IsNullOrEmpty(text) ? parentPath : (text2 + "[" + text + "]"));
		return new ConfigurationItem
		{
			ItemType = text2,
			Path = text3,
			DisplayName = child.GetString("displayName"),
			ItemCategory = itemCategory,
			ChildrenFilled = true,
			Children = ConstructChildItems(child, definitions, text3),
			EnableProperty = enableProperty,
			MethodIds = new string[0],
			ParentPath = parentPath,
			Properties = ConstructProperties(text2, null, child, definitions),
			SortKey = 100,
			UIHint = string.Empty,
			ServerCookie = null
		};
	}

	private static string FieldToKey(string itemType, string field, bool noCasingOfMIPItems)
	{
		if (CaseProperty(itemType, field, noCasingOfMIPItems))
		{
			return field.Substring(0, 1).ToUpperInvariant() + field.Substring(1);
		}
		return field;
	}

	public static bool CaseProperty(string itemType, string field, bool noCasingOfMIPItems)
	{
		if (itemType == "CustomProperties" || ((itemType == "MIPItem" || noCasingOfMIPItems) && !IsInvokeField(field)))
		{
			return false;
		}
		if (Guid.TryParse(field, out var _))
		{
			return false;
		}
		return true;
	}

	private static bool IsInvokeField(string field)
	{
		return isInvokeField.Contains(field);
	}

	private static void SetDefinitionFields(Property property, string field, JsonObject definitions)
	{
		property.UIImportance = 2;
		property.SortKey = 100;
		property.ItemReference = false;
		property.IsSettable = false;
		property.TranslationId = string.Empty;
		property.ToolTipTranslationId = string.Empty;
		if (definitions == null)
		{
			return;
		}
		JsonObject child = definitions.GetChild(field);
		if (child != null)
		{
			string text = child.GetString("valueType");
			if (text != null && text.StartsWith("Path", StringComparison.OrdinalIgnoreCase))
			{
				property.ItemReference = true;
			}
			property.IsSettable = child.GetBool("canSet") == true;
			property.TranslationId = child.GetString("translationId");
			property.ToolTipTranslationId = child.GetString("toolTipTranslationId");
		}
	}

	private static ValueTypeInfo[] GetValueTypeInfos(string field, JsonObject definitions)
	{
		if (definitions == null)
		{
			return null;
		}
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		JsonObject child = definitions.GetChild(field);
		string text = "";
		if (child == null)
		{
			if (field == "name")
			{
				child = definitions.GetChild("displayName");
				text = "string";
			}
			if (child == null)
			{
				return list.ToArray();
			}
		}
		else
		{
			text = child.GetString("valueType");
		}
		switch (text)
		{
		case "enum":
		{
			JsonObject child3 = child.GetChild("options");
			if (child3 == null || !child3.IsArray)
			{
				break;
			}
			foreach (JsonObject child4 in child3.GetChildren())
			{
				string name2 = child4.GetString("displayName");
				string value4 = child4.GetString("value");
				string translationId = child4.GetString("translationId");
				list.Add(new ValueTypeInfo
				{
					Name = name2,
					Value = value4,
					TranslationId = translationId
				});
			}
			break;
		}
		case "slider":
		case "double":
		case "string":
		case "int":
		{
			string value = child.GetString("MinValue");
			if (!string.IsNullOrEmpty(value))
			{
				list.Add(new ValueTypeInfo
				{
					Name = "MinValue",
					Value = value
				});
			}
			string value2 = child.GetString("MaxValue");
			if (!string.IsNullOrEmpty(value2))
			{
				list.Add(new ValueTypeInfo
				{
					Name = "MaxValue",
					Value = value2
				});
			}
			string value3 = child.GetString("StepValue");
			if (!string.IsNullOrEmpty(value3))
			{
				list.Add(new ValueTypeInfo
				{
					Name = "StepValue",
					Value = value3
				});
			}
			break;
		}
		case "path":
		{
			string text2 = child.GetString("type");
			if (!string.IsNullOrEmpty(text2))
			{
				list.Add(new ValueTypeInfo
				{
					Name = "PathItemType",
					Value = _nameConvert.ResourcePluralToItemType(text2)
				});
			}
			break;
		}
		case "pathList":
		{
			JsonObject child2 = child.GetChild("type");
			if (child2 != null)
			{
				if (child2.IsArray)
				{
					List<string> list2 = new List<string>();
					foreach (JsonObject child5 in child2.GetChildren())
					{
						string item = _nameConvert.ResourcePluralToItemType(child5.Value);
						list2.Add(item);
					}
					list.Add(new ValueTypeInfo
					{
						Name = "PathItemType",
						Value = string.Join(",", list2)
					});
				}
				else
				{
					list.Add(new ValueTypeInfo
					{
						Name = "PathItemType",
						Value = _nameConvert.ResourcePluralToItemType(child2.Value)
					});
				}
			}
			if (child.GetChild("allowAllItems") != null)
			{
				list.Add(new ValueTypeInfo
				{
					Name = "PathAllowAllItemType",
					Value = string.Empty
				});
			}
			break;
		}
		case "array":
		{
			string name = child.GetString("type");
			list.Add(new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = UpperCaseFirst(name)
			});
			break;
		}
		case "dictionary":
			list.Add(new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			});
			break;
		default:
			_ = field != "displayName";
			break;
		case "time":
		case "bool":
		case "dateTime":
		case "progress":
		case "separator":
			break;
		}
		return list.ToArray();
	}

	private static void SetValuefrom(string callerItemType, Property property, string field, JsonObject resource, JsonObject definitions, bool isPath)
	{
		string text = resource.GetType(field);
		if (callerItemType == "LicenseFeature" && field == "parameters")
		{
			text = "dictionary";
		}
		if (definitions != null)
		{
			if (callerItemType == "InvokeInfo" && field == "path")
			{
				text = "path";
			}
			else
			{
				JsonObject child = definitions.GetChild(field);
				if (child != null)
				{
					string text2 = child.GetString("valueType");
					if (!string.IsNullOrEmpty(text2))
					{
						text = text2;
					}
				}
			}
		}
		if (text == "string" && isPath)
		{
			text = "path";
		}
		if (text == null)
		{
			return;
		}
		switch (text.Length)
		{
		case 6:
		{
			char c = text[0];
			if ((uint)c <= 79u)
			{
				switch (c)
				{
				default:
					return;
				case 'D':
					break;
				case 'O':
					if (text == "Object")
					{
						JsonObject child4 = resource.GetChild(field);
						string id = child4.GetString("id");
						string itemType = ConvertResourceToItemType(child4.GetString("type"));
						property.Value = new ConfigurationItemPath(itemType, id).Path;
						property.ValueType = "Path";
					}
					return;
				}
				if (!(text == "Double"))
				{
					break;
				}
				goto IL_03e0;
			}
			if (c != 'S')
			{
				if (c == 'd')
				{
					if (!(text == "double"))
					{
						break;
					}
					goto IL_03e0;
				}
				if (c != 's')
				{
					break;
				}
				if (!(text == "string"))
				{
					if (text == "slider")
					{
						int? integer = resource.GetInteger(field);
						if (integer.HasValue)
						{
							property.Value = integer.Value.ToString(CultureInfo.InvariantCulture);
						}
						else
						{
							property.Value = "0";
						}
						property.ValueType = "Slider";
					}
					break;
				}
			}
			else if (!(text == "String"))
			{
				break;
			}
			property.Value = resource.GetString(field);
			property.ValueType = "String";
			break;
		}
		case 4:
		{
			char c = text[0];
			if ((uint)c <= 98u)
			{
				if (c != 'B')
				{
					if (c == 'D')
					{
						if (!(text == "Date"))
						{
							break;
						}
						goto IL_0521;
					}
					if (c != 'b' || !(text == "bool"))
					{
						break;
					}
				}
				else if (!(text == "Bool"))
				{
					break;
				}
				goto IL_0337;
			}
			switch (c)
			{
			case 't':
				if (text == "time")
				{
					property.Value = resource.GetTime(field);
					property.ValueType = "Time";
				}
				break;
			case 'p':
				if (text == "path")
				{
					JsonObject child3 = resource.GetChild(field);
					string text3 = child3.GetString("type");
					string text4 = child3.GetString("id");
					if (string.IsNullOrEmpty(text3))
					{
						property.Value = resource.GetString(field);
						property.ValueType = "Path";
						break;
					}
					ResourceEntry resourcePlural = _nameConvert.GetResourcePlural(text3);
					string value = (string.IsNullOrEmpty(text4) ? ("/" + resourcePlural.ItemTypeFolder) : (resourcePlural.ItemType + "[" + text4 + "]"));
					property.Value = value;
					property.ValueType = "Path";
				}
				break;
			case 'e':
				if (text == "enum")
				{
					property.Value = CheckForPathConvert(resource.GetString(field));
					property.ValueType = "Enum";
					property.ValueTypeInfos = BuildValueTypeEnum(field, definitions);
				}
				break;
			}
			break;
		}
		case 7:
		{
			char c = text[0];
			if (c != 'B')
			{
				if (c != 'I' || !(text == "Integer"))
				{
					break;
				}
				goto IL_035f;
			}
			if (!(text == "Boolean"))
			{
				break;
			}
			goto IL_0337;
		}
		case 5:
		{
			switch (text[0])
			{
			default:
				return;
			case 'a':
				if (!(text == "array"))
				{
					return;
				}
				break;
			case 'A':
				if (!(text == "Array"))
				{
					return;
				}
				break;
			}
			Collection<JsonObject> children = resource.GetChild(field).GetChildren();
			List<string> list2 = new List<string>();
			foreach (JsonObject item2 in children)
			{
				if (item2.IsObject)
				{
					string text7 = item2.GetString("type");
					string text8 = item2.GetString("id");
					if (!string.IsNullOrEmpty(text7) && !string.IsNullOrEmpty(text8))
					{
						list2.Add(_nameConvert.ResourcePluralToItemType(text7) + "[" + text8 + "]");
					}
					else if (!string.IsNullOrEmpty(text7))
					{
						list2.Add(_nameConvert.ResourcePluralToItemType(text7) ?? "");
					}
				}
				else
				{
					list2.Add(item2.Value);
				}
			}
			property.Value = string.Join(ArraySeperator(callerItemType, field), list2);
			property.ValueArray = list2.ToArray();
			property.ValueType = "Array";
			break;
		}
		case 8:
			switch (text[3])
			{
			default:
				return;
			case 'e':
				break;
			case 'h':
			{
				if (!(text == "pathList"))
				{
					return;
				}
				JsonObject child5 = resource.GetChild(field);
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				if (child5.IsArray)
				{
					foreach (JsonObject child6 in child5.GetChildren())
					{
						string resourceName = child6.GetString("type");
						string text5 = child6.GetString("id");
						ResourceEntry resourcePlural2 = _nameConvert.GetResourcePlural(resourceName);
						string text6 = (string.IsNullOrEmpty(text5) ? ("/" + resourcePlural2.ItemTypeFolder) : (resourcePlural2.ItemType + "[" + text5 + "]"));
						stringBuilder.Append((flag ? "" : ArraySeperator(callerItemType, field)) + text6);
						flag = false;
					}
				}
				property.Value = stringBuilder.ToString();
				property.ValueType = "PathList";
				return;
			}
			case 'g':
				if (text == "progress")
				{
					int? integer2 = resource.GetInteger(field);
					if (integer2.HasValue)
					{
						property.Value = integer2.Value.ToString(CultureInfo.InvariantCulture);
					}
					else
					{
						property.Value = "0";
					}
					property.ValueType = "Progress";
				}
				return;
			case 'f':
				return;
			}
			if (!(text == "dateTime"))
			{
				break;
			}
			goto IL_0521;
		case 3:
			if (!(text == "int"))
			{
				break;
			}
			goto IL_035f;
		case 9:
			if (text == "separator")
			{
				property.Value = resource.GetString(field);
				property.ValueType = "Separator";
				property.ValueTypeInfos = BuildValueTypeEnum(field, definitions);
			}
			break;
		case 10:
		{
			if (!(text == "dictionary"))
			{
				break;
			}
			JsonObject child2 = resource.GetChild(field);
			List<string> list = new List<string>();
			foreach (JsonObject child7 in child2.GetChildren())
			{
				string arg = child7.GetString("key");
				string arg2 = child7.GetString("value");
				string item = $"{arg};{arg2}";
				list.Add(item);
			}
			property.ValueArray = list.ToArray();
			break;
		}
		case 0:
		case 1:
		case 2:
			break;
			IL_035f:
			property.Value = resource.GetInteger(field)?.ToString(CultureInfo.InvariantCulture);
			property.ValueType = "Int";
			break;
			IL_0521:
			property.Value = resource.GetDateTime(field);
			property.ValueType = "DateTime";
			break;
			IL_03e0:
			property.Value = resource.GetDouble(field).ToString(CultureInfo.InvariantCulture);
			property.ValueType = "Double";
			break;
			IL_0337:
			property.Value = resource.GetBool(field).ToString();
			property.ValueType = "Tick";
			break;
		}
	}

	private static string UpperCaseFirst(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		return name.Substring(0, 1).ToUpperInvariant() + name.Substring(1);
	}

	private static bool ShouldBeInProperties(string itemType, string field)
	{
		if (itemType == "User" && field == "displayName")
		{
			return true;
		}
		return !_excludeFields.Contains(field);
	}

	public static string[] TasksToMethodIds(JsonObject tasks)
	{
		if (tasks == null)
		{
			return new string[0];
		}
		List<string> list = new List<string>();
		foreach (JsonObject child in tasks.GetChildren())
		{
			list.Add(child.GetString("id"));
		}
		return list.ToArray();
	}

	public static string ConvertResourceToItemType(string resourceName)
	{
		string text = _nameConvert.ChildFieldNameToItemType(resourceName);
		if (text != resourceName)
		{
			return text;
		}
		return _nameConvert.ResourcePluralToItemType(text);
	}

	public static string ConvertItemTypeToResource(string itemType)
	{
		if (itemType.EndsWith("Folder", StringComparison.Ordinal))
		{
			return _nameConvert.FolderItemTypeToResourcePlural(itemType);
		}
		return _nameConvert.ItemTypeToResourcePlural(itemType);
	}

	public static string FolderConvertItemTypeToResource(string itemType)
	{
		return _nameConvert.FolderItemTypeToResourcePlural(itemType);
	}

	public static string ConvertItemTypeToResourceSkipFolder(string itemType)
	{
		if (!itemType.EndsWith("Folder", StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("Folder expected", "itemType");
		}
		return FolderConvertItemTypeToResource(itemType.Substring(0, itemType.Length - 6));
	}

	public static string ConvertConfigurationItemToJson(ConfigurationItem item, bool noData, bool createProperties = true, bool isMIPItem = false)
	{
		ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder = new ConfigApiRelatedJsonBuilder(array: false);
		if (createProperties)
		{
			if (item.EnableProperty != null)
			{
				configApiRelatedJsonBuilder.AppendElement("enabled", item.EnableProperty.Enabled.ToString().ToLowerInvariant());
			}
			if (item.Properties != null)
			{
				Property[] properties;
				if (item.Properties.FirstOrDefault((Property p) => p.Key == "FQID.Kind") != null)
				{
					ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder2 = new ConfigApiRelatedJsonBuilder(array: false);
					properties = item.Properties;
					foreach (Property property in properties)
					{
						if (property.Key.StartsWith("FQID", StringComparison.Ordinal))
						{
							configApiRelatedJsonBuilder2.AppendKeyString(_nameConvert.PropertyKeyToField("MIPItem", property.Key.Substring(5)), property.Value);
						}
					}
					configApiRelatedJsonBuilder.AppendElement("FQID", configApiRelatedJsonBuilder2.ToJson());
				}
				properties = item.Properties;
				foreach (Property property2 in properties)
				{
					if (!property2.Key.StartsWith("FQID", StringComparison.Ordinal) && (!(item.ItemType == "InvokeInfo") || (!(property2.Key == "State") && !(property2.Key == "ErrorText") && !(property2.Key == "ErrorCode"))))
					{
						SerializeProperty(isMIPItem ? "MIPItem" : item.ItemType, configApiRelatedJsonBuilder, property2);
					}
				}
			}
			if (item.ChildrenFilled && item.Children != null && item.Children.Length != 0 && !item.Children[0].ItemType.EndsWith("Folder", StringComparison.Ordinal))
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				ConfigurationItem[] children = item.Children;
				foreach (ConfigurationItem configurationItem in children)
				{
					if (configurationItem.ItemCategory == ItemCategories.ChildItem)
					{
						if (!dictionary.ContainsKey(configurationItem.ItemType))
						{
							dictionary.Add(configurationItem.ItemType, 0);
						}
						dictionary[configurationItem.ItemType]++;
					}
				}
				Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
				children = item.Children;
				foreach (ConfigurationItem child in children)
				{
					SerializeChildItem(configApiRelatedJsonBuilder, dictionary, dictionary2, child, noData);
				}
				foreach (KeyValuePair<string, List<string>> item2 in dictionary2)
				{
					ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder3 = new ConfigApiRelatedJsonBuilder(array: true);
					foreach (string item3 in item2.Value)
					{
						configApiRelatedJsonBuilder3.AppendElement(item3);
					}
					configApiRelatedJsonBuilder.AppendElement(_nameConvert.PropertyKeyToField("", item2.Key), configApiRelatedJsonBuilder3.ToJson());
				}
			}
		}
		return configApiRelatedJsonBuilder.ToJson();
	}

	public static bool IsRemoveMethod(string methodId)
	{
		if (MethodToBeHandledWithPost(methodId))
		{
			return false;
		}
		if (methodId.StartsWith("Remove", StringComparison.Ordinal))
		{
			return true;
		}
		if (methodId.StartsWith("Delete", StringComparison.Ordinal))
		{
			return true;
		}
		return false;
	}

	public static bool IsAddMethod(string itemType, string methodId)
	{
		if (MethodToBeHandledWithPost(methodId))
		{
			return false;
		}
		string text = TrimFolder(itemType);
		if (methodId.StartsWith("Add" + text, StringComparison.Ordinal))
		{
			return true;
		}
		if (methodId.StartsWith("MethodIdAdd" + text, StringComparison.Ordinal))
		{
			return true;
		}
		return false;
	}

	public static string TrimFolder(string itemType)
	{
		if (itemType.EndsWith("Folder", StringComparison.Ordinal))
		{
			return itemType.Substring(0, itemType.Length - 6);
		}
		return itemType;
	}

	private static void SerializeProperty(string itemType, ConfigApiRelatedJsonBuilder jb, Property property)
	{
		if (!_alwaysReadOnlyFields.Contains(property.Key))
		{
			if (property.ValueType == "Int" || property.ValueType == "Progress" || property.ValueType == "Slider")
			{
				jb.AppendPropertyInt(itemType, property.Key, property.Value);
			}
			else if (property.ValueType == "Tick")
			{
				jb.AppendPropertyTick(itemType, property.Key, property.Value);
			}
			else if (property.ValueType == "Path")
			{
				ConfigApiRelatedJsonBuilder.AppendPropertyPath(jb, property);
			}
			else if (property.ValueType == "PathList")
			{
				ConfigApiRelatedJsonBuilder.AppendPropertyPathList(jb, itemType, property);
			}
			else if (property.ValueType == "Array")
			{
				ConfigApiRelatedJsonBuilder.AppendPropertyArray(jb, itemType, property);
			}
			else if (property.ValueType == "Enum")
			{
				jb.AppendPropertyKeyStringValue(itemType, property.Key, JsonConvertUtilities.CheckConvertPath(property.Value));
			}
			else
			{
				jb.AppendPropertyKeyStringValue(itemType, property.Key, property.Value);
			}
		}
	}

	private static void SerializeChildItem(ConfigApiRelatedJsonBuilder jb, Dictionary<string, int> childTypeCount, Dictionary<string, List<string>> childItemArrays, ConfigurationItem child, bool noData)
	{
		if (!(child.ItemCategory == ItemCategories.ChildItem))
		{
			return;
		}
		string key = _nameConvert.ChildItemTypeToFieldName(child.ItemType);
		if (childTypeCount[child.ItemType] == 1)
		{
			jb.AppendElement(_nameConvert.PropertyKeyToField("", key), ConvertConfigurationItemToJson(child, noData));
			return;
		}
		if (!childItemArrays.ContainsKey(key))
		{
			childItemArrays.Add(key, new List<string>());
		}
		childItemArrays[key].Add(ConvertConfigurationItemToJson(child, noData));
	}

	private static ValueTypeInfo[] BuildValueTypeEnum(string field, JsonObject definitions)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		if (definitions != null)
		{
			JsonObject child = definitions.GetChild(field);
			if (child != null)
			{
				JsonObject child2 = child.GetChild("options");
				if (child2 != null && child2.IsArray)
				{
					foreach (JsonObject child3 in child2.GetChildren())
					{
						list.Add(new ValueTypeInfo
						{
							Name = child3.GetString("displayName"),
							Value = CheckForPathConvert(child3.GetString("value")),
							TranslationId = child3.GetString("translationId")
						});
					}
				}
			}
		}
		return list.ToArray();
	}

	private static string CheckForPathConvert(string resourcePath)
	{
		if (string.IsNullOrEmpty(resourcePath))
		{
			return resourcePath;
		}
		string[] array = resourcePath.Split('/');
		if (array.Length == 2)
		{
			string text = _nameConvert.ResourcePluralToItemType(array[0], throwForInvalid: false);
			if (text != null && Guid.TryParse(array[1], out var result))
			{
				return new ConfigurationItemPath(text, result).Path;
			}
		}
		return resourcePath;
	}

	private static string ArraySeperator(string itemType, string field)
	{
		if (!(field == "related") && !(field == "relatedCameraList"))
		{
			return ";";
		}
		return ",";
	}

	private static bool MethodToBeHandledWithPost(string methodId)
	{
		return _methodsToBeHandledWithPost.Contains(methodId);
	}

	private static bool HasNoChildren(string itemType)
	{
		return _itemTypesThatHasNoChildren.Contains(itemType);
	}
}

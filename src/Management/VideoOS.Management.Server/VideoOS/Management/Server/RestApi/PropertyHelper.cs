using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public static class PropertyHelper
{
	private const int MaxRestApiPathSize = 100;

	private static readonly HashSet<Tuple<string, string>> OptionalProperties = new HashSet<Tuple<string, string>>
	{
		new Tuple<string, string>("ClearView", "ClientId"),
		new Tuple<string, string>("ClearViewItem", "ClientId"),
		new Tuple<string, string>("ApplyViewToView", "ClientId"),
		new Tuple<string, string>("LoadTasks", "TaskType"),
		new Tuple<string, string>("LoadTasks", "IncludeRecordingServerTasks")
	};

	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public static int CountRequiredSettableProperties(ConfigurationItem item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		int num = 0;
		if (item.Properties != null)
		{
			Property[] properties = item.Properties;
			foreach (Property property in properties)
			{
				if (property.IsSettable && (item.MethodIds.Length == 0 || !OptionalProperties.Contains(new Tuple<string, string>(item.MethodIds.First(), property.Key))))
				{
					num++;
				}
			}
		}
		return num;
	}

	public static Collection<string> UpdateProperties(ConfigurationItem item, ElementList jsonElement, bool listMustMatch, bool optionalProperty, bool noCasing = false)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (jsonElement == null)
		{
			throw new ArgumentNullException("jsonElement");
		}
		Dictionary<string, List<ConfigurationItem>> dictionary = new Dictionary<string, List<ConfigurationItem>>();
		if (item.Children != null)
		{
			ConfigurationItem[] children = item.Children;
			foreach (ConfigurationItem configurationItem in children)
			{
				if (!dictionary.ContainsKey(configurationItem.ItemType))
				{
					dictionary[configurationItem.ItemType] = new List<ConfigurationItem>();
				}
				dictionary[configurationItem.ItemType].Add(configurationItem);
			}
		}
		Collection<string> collection = new Collection<string>();
		foreach (IElement element in jsonElement.Elements)
		{
			if (SpecialCases.IsPropertyRestApiPrivate(element.Key))
			{
				continue;
			}
			string text = NameConvert.FieldToPropertyKeyNameConvert(item.ItemType, item.MethodIds, element.Key);
			if (text == "FQID")
			{
				FillFqid(item, element);
			}
			else
			{
				if (item.ItemType == "InvokeInfo" && (text == "state" || text == "errorText"))
				{
					continue;
				}
				StringElement stringElement = element as StringElement;
				IntElement intElement = element as IntElement;
				BooleanElement booleanElement = element as BooleanElement;
				ElementList elementList = element as ElementList;
				NullElement nullElement = element as NullElement;
				DoubleElement doubleElement = element as DoubleElement;
				if (stringElement != null)
				{
					string value = stringElement.Value;
					string text2 = SetPropertyValue(item, text, value, optionalProperty, noCasing);
					if (text2 != null)
					{
						collection.Add(text2);
					}
				}
				else if (intElement != null)
				{
					string value2 = intElement.Value;
					string text3 = SetPropertyValue(item, text, value2, optionalProperty, noCasing);
					if (text3 != null)
					{
						collection.Add(text3);
					}
				}
				else if (doubleElement != null)
				{
					string value3 = doubleElement.Value;
					string text4 = SetPropertyValue(item, text, value3, optionalProperty, noCasing);
					if (text4 != null)
					{
						collection.Add(text4);
					}
				}
				else if (booleanElement != null)
				{
					if (text == "enabled" && item.EnableProperty != null)
					{
						if (item.EnableProperty.Enabled != booleanElement.Value)
						{
							collection.Add(text);
							item.EnableProperty.Enabled = booleanElement.Value;
						}
					}
					else
					{
						string text5 = SetPropertyValue(item, text, booleanElement.Value.ToString(), optionalProperty, noCasing);
						if (text5 != null)
						{
							collection.Add(text5);
						}
					}
				}
				else if (nullElement != null)
				{
					string text6 = SetPropertyValue(item, text, null, optionalProperty, noCasing);
					if (text6 != null)
					{
						collection.Add(text6);
					}
				}
				else
				{
					if (elementList == null || text == "relations")
					{
						continue;
					}
					if (SetPropertyValue(item, text, elementList, out var propertyKey))
					{
						collection.Add(propertyKey);
						continue;
					}
					if (!item.ChildrenFilled || item.Children == null)
					{
						if (!listMustMatch)
						{
							continue;
						}
						throw new PropertyException(element.Key, "Element group not found");
					}
					bool flag = false;
					int num = 0;
					string key = NameConvert.ResourcePluralToItemType(text);
					if (!dictionary.ContainsKey(key))
					{
						throw new PropertyException(element.Key, "Element group not found");
					}
					foreach (ConfigurationItem item2 in dictionary[key])
					{
						if (num >= elementList.Elements.Count)
						{
							break;
						}
						ElementList jsonElement2 = ((elementList.TokenType != TokenType.ArrayStart) ? elementList : (elementList.Elements.ElementAt(num) as ElementList));
						UpdateProperties(item2, jsonElement2, listMustMatch, optionalProperty);
						collection.Add(element.Key);
						flag = true;
						num++;
					}
					if (!flag && listMustMatch)
					{
						throw new PropertyException(element.Key, "Element group not found");
					}
				}
			}
		}
		return collection;
	}

	private static void FillFqid(ConfigurationItem item, IElement element)
	{
		if (!(element is ElementList elementList))
		{
			return;
		}
		foreach (IElement element2 in elementList.Elements)
		{
			SetPropertyValue(item, "FQID." + NameConvert.PropertyFieldToKey(item.ItemType, element2.Key, noCasing: true), elementList.GetStringElementValue(element2.Key), optional: false, noCasing: true);
		}
	}

	private static string SetPropertyValue(ConfigurationItem item, string key, string value, bool optional, bool noCasing)
	{
		if (item.ItemType != "InvokeInfo" && key == "enabled")
		{
			if (bool.TryParse(value, out var result))
			{
				if (item.EnableProperty != null)
				{
					item.EnableProperty.Enabled = result;
				}
				return key;
			}
			if (item.ItemType == "MIPItem" && item.EnableProperty != null)
			{
				if (string.Compare("Yes", value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					item.EnableProperty.Enabled = true;
				}
				if (string.Compare("No", value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					item.EnableProperty.Enabled = false;
				}
				return key;
			}
			throw new PropertyException(key, "property value not boolean");
		}
		if (item.ItemType != "InvokeInfo" && key == "displayName")
		{
			item.DisplayName = value;
			return key;
		}
		Property property = LookupProperty(item, key, out key);
		if (property == null)
		{
			if (optional)
			{
				return null;
			}
			List<Property> list = ((item.Properties == null) ? new List<Property>() : item.Properties.ToList());
			list.Add(new Property(NameConvert.PropertyFieldToKey(item.ItemType, key, noCasing), value));
			item.Properties = list.ToArray();
			return key;
		}
		if (property.IsSettable)
		{
			if (property.ValueType == "Enum" && property.ValueTypeInfos != null)
			{
				ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
				foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
				{
					if (string.Compare(valueTypeInfo.Value, value, StringComparison.OrdinalIgnoreCase) == 0)
					{
						value = valueTypeInfo.Value;
						break;
					}
				}
			}
			if (value == null && property.ValueType == "Int")
			{
				property.Value = string.Empty;
			}
			else
			{
				property.Value = CheckForResourceLinkToPath(value);
			}
		}
		else
		{
			StringComparison comparisonType = StringComparison.Ordinal;
			if (string.Compare("id", property.Key, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare("appointmentRootId", property.Key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return null;
			}
			if (property.ValueType == "Tick")
			{
				comparisonType = StringComparison.OrdinalIgnoreCase;
			}
			if (string.Compare(property.Value, CheckForResourceLinkToPath(value), comparisonType) != 0)
			{
				throw new PropertyInvalidException("Property cannot be changed: " + property.Key, key);
			}
		}
		return key;
	}

	private static void SetIfAllowed(Property property, string key, string newValue)
	{
		if (property.IsSettable)
		{
			property.Value = newValue;
		}
		else if (string.Compare(property.Value, newValue, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new PropertyInvalidException("Property cannot be changed: " + property.Key, key);
		}
	}

	private static bool SetPropertyValue(ConfigurationItem item, string key, ElementList element, out string propertyKey)
	{
		Property property = LookupProperty(item, key, out propertyKey);
		if (property != null)
		{
			string text = property.ValueType;
			if (text == "Separator" && property.ValueTypeInfos != null && property.ValueTypeInfos.Length != 0 && property.ValueTypeInfos[0].Name == "PathItemType")
			{
				text = "PathList";
			}
			switch (text)
			{
			case "Path":
				SetIfAllowed(property, key, ResourceLinkToPath(key, element));
				return true;
			case "PathList":
			{
				string text3 = "";
				foreach (IElement element2 in element.Elements)
				{
					if (!(element2 is ElementList elementList))
					{
						throw new ArgumentException("RestAPI: invalid JSON format");
					}
					if (elementList.Elements.Count != 0)
					{
						text3 = text3 + ResourceLinkToPath(property.Key, elementList) + ";";
					}
				}
				SetIfAllowed(property, key, text3.Trim(';'));
				return true;
			}
			case "Array":
			{
				if (!property.IsSettable)
				{
					throw new PropertyInvalidException("Property cannot be changed: " + property.Key, key);
				}
				string text2 = property.ValueTypeInfos?.FirstOrDefault((ValueTypeInfo vti) => vti.Name == "ArrayValueType")?.Value;
				if (text2 == "String")
				{
					property.ValueArray = (from e in element.Elements.OfType<StringElement>()
						select e.Value).ToArray();
					return true;
				}
				if (text2 == "Int")
				{
					property.ValueArray = (from e in element.Elements.OfType<IntElement>()
						select e.Value).ToArray();
					return true;
				}
				break;
			}
			}
			if (property.Key == "ItemSelection")
			{
				SetIfAllowed(property, key, ResourceLinkToPath(key, element));
				return true;
			}
			throw new InternalServerErrorException("RestAPI: multilevel property not handled: " + key + ", ValueType:" + property.ValueType);
		}
		return false;
	}

	private static string ResourceLinkToPath(string key, ElementList resourceLink)
	{
		if (resourceLink == null || resourceLink.Elements.Count > 2)
		{
			throw new ArgumentException(key + " field format error, " + ((resourceLink == null) ? "NULL" : ("ElementCount=" + resourceLink.Elements.Count.ToString(CultureInfo.InvariantCulture))));
		}
		if (!(resourceLink.Element("type") is StringElement stringElement))
		{
			throw new ArgumentException("type invalid in pathList");
		}
		string itemType = NameConvert.ResourcePluralToItemType(stringElement.Value);
		if (resourceLink.Elements.Count == 1)
		{
			return "/" + NameConvert.ResourcePluralToFolderItemType(stringElement.Value);
		}
		if (!(resourceLink.Element("id") is StringElement stringElement2) || string.IsNullOrEmpty(stringElement2.Value))
		{
			return "/" + NameConvert.ResourcePluralToFolderItemType(stringElement.Value);
		}
		return ConfigurationItemPath.CreateItemPath(itemType, stringElement2.Value).Text;
	}

	private static string CheckForResourceLinkToPath(string resourceLink)
	{
		if (string.IsNullOrEmpty(resourceLink))
		{
			return resourceLink;
		}
		if (resourceLink.Length > 100)
		{
			return resourceLink;
		}
		if (!char.IsLetter(resourceLink[0]))
		{
			return resourceLink;
		}
		string[] array = resourceLink.Split('/');
		if (array.Length == 2)
		{
			string text = NameConvert.ResourcePluralToItemType(array[0], throwForInvalid: false);
			if (text != null)
			{
				return ConfigurationItemPath.CreateItemPath(text, array[1]).Text;
			}
		}
		return resourceLink;
	}

	private static Property LookupProperty(ConfigurationItem item, string fieldName, out string propertyKey)
	{
		propertyKey = fieldName;
		if (item.Properties == null)
		{
			return null;
		}
		string text = NameConvert.PropertyFieldToKey(item.ItemType, fieldName);
		string value = "/" + text + "/";
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (string.Compare(property.Key, text, StringComparison.OrdinalIgnoreCase) == 0 || property.Key.Contains(value))
			{
				propertyKey = property.Key;
				return property;
			}
		}
		return null;
	}
}

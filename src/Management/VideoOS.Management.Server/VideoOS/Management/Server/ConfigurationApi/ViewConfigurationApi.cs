using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIView")]
[EntityName("ConfigAPIViewFolder")]
public class ViewConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private const string ViewlayoutType_Custom = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayoutCustom, VideoOS.RemoteClient.Application";

	private const string EmptyViewItem_Definition = "<viewitem id=\"{0}\" displayname=\"Empty ViewItem\" shortcut=\"\" type=\"VideoOS.RemoteClient.Application.Data.Configuration.EmptyViewItem, VideoOS.RemoteClient.Application\"><properties /></viewitem>";

	public const string LayoutViewItems_Default = "<ViewItems><ViewItem><Position><X>0</X><Y>0</Y></Position><Size><Width>600</Width><Height>300</Height></Size></ViewItem><ViewItem><Position><X>0</X><Y>300</Y></Position><Size><Width>600</Width><Height>300</Height></Size></ViewItem></ViewItems>";

	public const string Icon_default = "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";

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 30);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 31);
		if (path.IsFolder)
		{
			View[] source = View.QueryViewHeriarchyByServerGroupIdNoSecurity(requestContext, path.Id);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (View item in source.Where((View v) => string.Compare(v.ViewGroupId, path.Id, StringComparison.OrdinalIgnoreCase) == 0))
			{
				item.ViewItems = View.QueryViewItems(requestContext, item.Id);
				list.Add(ConvertToViewConfigurationItem(path, item));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 55);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 56);
		if (path.IsFolder)
		{
			if (path.ParentItemType != "ViewGroup")
			{
				throw new ArgumentException("Parent type is incorrect", "path");
			}
			if (path.ItemType != "ViewFolder")
			{
				throw new ArgumentException("Folder type is incorrect", "path");
			}
			return GroupConfigurationItemFactory.CreateFolderItem("ViewFolder", ConfigurationItemPath.CreateItemPath("ViewGroup", path.Id));
		}
		try
		{
			View view = View.QueryView(requestContext, path.Id);
			if (view == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			view.ViewItems = View.QueryViewItems(requestContext, view.Id);
			ConfigurationItemPath parentPath = (string.IsNullOrEmpty(view.ViewGroupId) ? ConfigurationItemPath.CreateItemPath("Monitor", view.MonitorId) : ConfigurationItemPath.CreateFolderPath("ViewGroup", view.ViewGroupId, "ViewFolder"));
			return ConvertToViewConfigurationItem(parentPath, view);
		}
		catch (ViewNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		item.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 102);
		methodId.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 103);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (methodId)
		{
		case "AddView":
			if (!flag)
			{
				return CreateAddViewFirst(item, methodId);
			}
			return CreateAddViewSecond(requestContext, item, path, methodId);
		case "RemoveView":
			if (!flag)
			{
				return CreateRemoveView(item, GetChildItems(requestContext, path), methodId);
			}
			return RemoveView(requestContext, item, path);
		case "ApplyViewToView":
			if (!flag)
			{
				return CreateApplyViewToView(item, GetChildItems(requestContext, path), methodId);
			}
			return ApplyViewToView(requestContext, item, path);
		case "ClearView":
			if (!flag)
			{
				return CreateClearView(item, GetChildItems(requestContext, path), methodId);
			}
			return ClearView(requestContext, item, path);
		case "ClearViewItem":
			if (!flag)
			{
				return CreateClearViewItem(item, GetChildItems(requestContext, path), methodId);
			}
			return ClearViewItem(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 131);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 132);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 133);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 134);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath.Id, out var definitionXml, out var name, out var _);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		Dictionary<int, Tuple<string, Guid>> dictionary = new Dictionary<int, Tuple<string, Guid>>();
		ConfigurationItem[] children = item.Children;
		for (int i = 0; i < children.Length; i++)
		{
			validateResult = ValidateViewItem(children[i], out var xml, out var position, out var viewItemId);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			dictionary[position] = new Tuple<string, Guid>(xml, viewItemId);
		}
		try
		{
			View view = View.QueryView(requestContext, path.Id);
			view.ViewItems = View.QueryViewItems(requestContext, view.Id);
			ViewItem[] viewItems = view.ViewItems;
			foreach (ViewItem viewItem in viewItems)
			{
				if (!dictionary.ContainsKey(viewItem.Position))
				{
					throw new PropertyInvalidException(FormattableString.Invariant($"Missing view item definition for position {viewItem.Position}"));
				}
				Tuple<string, Guid> tuple = dictionary[viewItem.Position];
				bool flag = new Guid(viewItem.Id) == tuple.Item2;
				if (!(viewItem.DefinitionXml.Equals(tuple.Item1, StringComparison.Ordinal) && flag))
				{
					string id = viewItem.Id;
					viewItem.DefinitionXml = tuple.Item1;
					viewItem.Id = ((tuple.Item2 == Guid.Empty) ? viewItem.Id : tuple.Item2.ToString());
					if (viewItem.Id != id)
					{
						ViewItem.ChangeViewItemId(requestContext, id, viewItem.Id, fireChangeEvent: false);
					}
					ViewItem.UpdateViewItem(requestContext, viewItem);
				}
			}
			if (!view.DefinitionXml.Equals(definitionXml, StringComparison.Ordinal) || !view.Name.Equals(name, StringComparison.Ordinal))
			{
				view.DefinitionXml = definitionXml;
				view.Name = name;
				view = View.UpdateView(requestContext, view);
			}
			view.ViewItems = View.QueryViewItems(requestContext, view.Id);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = ConvertToViewConfigurationItem(parentPath, view);
			return validateResult;
		}
		catch (ViewNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (ViewUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 213);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 214);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 215);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 216);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (View.QueryView(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string definitionXml;
			string name;
			int count;
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath.Id, out definitionXml, out name, out count);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			ConfigurationItem[] children = item.Children;
			for (int i = 0; i < children.Length; i++)
			{
				validateResult = ValidateViewItem(children[i], out var xml, out var position, out var _);
				if (!validateResult.ValidatedOk)
				{
					return validateResult;
				}
				dictionary[position] = xml;
			}
			return validateResult;
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string parentId, out string definitionXml, out string name, out int count)
	{
		definitionXml = null;
		name = "";
		count = 0;
		try
		{
			if (!path.IsFolder || item.ItemType == "InvokeInfo")
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				}
				string nameToCheck = name;
				if (View.QueryViewsNoSecurity(requestContext, parentId).FirstOrDefault((View s) => s.Name == nameToCheck && !s.Id.Equals(path.Id, StringComparison.OrdinalIgnoreCase)) != null)
				{
					throw new PropertyInvalidException("View name already exists", "Name");
				}
				definitionXml = EncodeViewDefinition(item, path, out count);
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
		catch (PropertyInvalidException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "ViewGroup", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	public static ConfigurationItem ConvertToViewConfigurationItem(ConfigurationItemPath parentPath, View view)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "View",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = view.Name,
			ParentPath = parentPath.Text,
			ChildrenFilled = true,
			Path = ConfigurationItemPath.CreateItemPath("View", view.Id).Text
		};
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(view.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(view.Name, isSettable: true));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(view.LastModified));
		if (!DecodeViewDefinition(view.DefinitionXml, out var _, out var _, out var shortcut, out var viewLayoutType, out var customId, out var layoutIcon, out var viewItemsXml, out var clientId))
		{
			throw new PropertyInvalidException();
		}
		if (!string.IsNullOrEmpty(shortcut) && !int.TryParse(shortcut, out var _))
		{
			throw new PropertyInvalidException("Shortcut is not a number", "Shortcut");
		}
		list.Add(ViewPropertyDefinition.GetShortcutProperty(shortcut));
		list.Add(ViewPropertyDefinition.GetViewLayoutTypeProperty(viewLayoutType));
		list.Add(ViewPropertyDefinition.GetViewLayoutCustomIdProperty(customId));
		list.Add(ViewPropertyDefinition.GetViewLayoutIconProperty(layoutIcon));
		list.Add(ViewPropertyDefinition.GetLayoutViewItemsProperty(viewItemsXml));
		list.Add(ViewPropertyDefinition.GetClientIdProperty(clientId));
		configurationItem.Properties = list.ToArray();
		List<string> list2 = new List<string> { "ApplyViewToView", "ClearView", "ClearViewItem" };
		configurationItem.MethodIds = list2.ToArray();
		List<ConfigurationItem> list3 = new List<ConfigurationItem>();
		if (view.ViewItems != null)
		{
			ViewItem[] viewItems = view.ViewItems;
			foreach (ViewItem viewItem in viewItems)
			{
				list3.Add(ConstructViewItemConfigurationItem(viewItem.Id, viewItem.Position, viewItem.DefinitionXml, configurationItem.Path));
			}
		}
		configurationItem.Children = list3.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem ConstructViewItemConfigurationItem(string id, int position, string definitionXml, string parentPath)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "ViewItem";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.ChildItem;
		configurationItem.DisplayName = FormattableString.Invariant($"Index {position}");
		configurationItem.ParentPath = parentPath;
		configurationItem.ChildrenFilled = true;
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.Path = ConfigurationItemPath.CreateItemPath("View", id).Text;
		configurationItem.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(id),
			ViewPropertyDefinition.GetViewItemPositionProperty(position.ToString(CultureInfo.InvariantCulture)),
			ViewPropertyDefinition.GetViewItemDefinitionXmlProperty(definitionXml)
		}.ToArray();
		return configurationItem;
	}

	public static bool DecodeViewDefinition(string xml, out string id, out string displayName, out string shortcut, out string viewLayoutType, out string customId, out string layoutIcon, out string viewItemsXml, out string clientId)
	{
		id = (shortcut = (displayName = (viewLayoutType = (customId = (layoutIcon = (viewItemsXml = (clientId = "")))))));
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("view");
		if (xmlNode == null)
		{
			return false;
		}
		id = xmlNode.Attributes["id"]?.Value;
		displayName = xmlNode.Attributes["displayname"]?.Value;
		shortcut = xmlNode.Attributes["shortcut"]?.Value;
		viewLayoutType = xmlNode.Attributes["viewlayouttype"]?.Value;
		clientId = xmlNode.Attributes["smartClientId"]?.Value;
		string text = xmlDocument.SelectSingleNode("view/properties/property[@name='LayoutProperty']").Attributes["value"]?.Value;
		if (text == null)
		{
			return false;
		}
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.LoadXml(text);
		XmlNode xmlNode2 = xmlDocument2.SelectSingleNode("ViewLayout");
		customId = xmlNode2.SelectSingleNode("CustomId")?.InnerText;
		layoutIcon = xmlNode2.SelectSingleNode("ViewLayoutIcon")?.InnerText;
		if (layoutIcon == null)
		{
			layoutIcon = xmlNode2.SelectSingleNode("Icon")?.InnerText;
		}
		viewItemsXml = xmlNode2.SelectSingleNode("ViewItems")?.OuterXml;
		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(layoutIcon) || string.IsNullOrEmpty(viewItemsXml))
		{
			return false;
		}
		if (xmlNode2.SelectSingleNode("ViewItems").SelectNodes("ViewItem").Count == 0)
		{
			return false;
		}
		return true;
	}

	private static string EncodeViewDefinition(ConfigurationItem item, ConfigurationItemPath path, out int count)
	{
		string value = "";
		string text = "";
		string value2 = "";
		string value3 = "";
		string value4 = "";
		string value5 = "";
		string value6 = Guid.Empty.ToString();
		count = 0;
		text = path.Id;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Shortcut", out string value7);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ViewLayoutType", out value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutCustomId", out value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutIcon", out value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutViewItems", out value5);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientId", out value6);
		if (!string.IsNullOrEmpty(value3) && !Guid.TryParse(value3, out var _))
		{
			throw new PropertyInvalidException("is not a guid", "LayoutCustomId");
		}
		if (!string.IsNullOrEmpty(value7) && !int.TryParse(value7, out var _))
		{
			throw new PropertyInvalidException("Shortcut is not a number", "Shortcut");
		}
		count = ValidateLayoutViewItems(value5);
		ValidateIcon(value4);
		StringBuilder stringBuilder = new StringBuilder();
		string value8;
		using (XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture)))
		{
			xmlTextWriter.WriteStartElement("ViewLayout");
			if (!string.IsNullOrEmpty(value3))
			{
				xmlTextWriter.WriteElementString("CustomId", value3);
			}
			xmlTextWriter.WriteElementString("Icon", value4);
			xmlTextWriter.WriteRaw(value5);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			value8 = stringBuilder.ToString();
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		using XmlTextWriter xmlTextWriter2 = new XmlTextWriter(new StringWriter(stringBuilder2, CultureInfo.InvariantCulture));
		xmlTextWriter2.WriteStartElement("view");
		xmlTextWriter2.WriteAttributeString("id", text);
		xmlTextWriter2.WriteAttributeString("displayname", value);
		xmlTextWriter2.WriteAttributeString("shortcut", value7);
		xmlTextWriter2.WriteAttributeString("viewlayouttype", value2);
		xmlTextWriter2.WriteAttributeString("smartClientId", value6);
		xmlTextWriter2.WriteStartElement("properties");
		xmlTextWriter2.WriteStartElement("property");
		xmlTextWriter2.WriteAttributeString("name", "LayoutProperty");
		xmlTextWriter2.WriteAttributeString("value", value8);
		xmlTextWriter2.WriteEndElement();
		xmlTextWriter2.WriteEndElement();
		xmlTextWriter2.WriteEndElement();
		xmlTextWriter2.Flush();
		return stringBuilder2.ToString();
	}

	private static int ValidateLayoutViewItems(string xml)
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNodeList xmlNodeList = (xmlDocument.SelectSingleNode("ViewItems") ?? throw new PropertyInvalidException("View Items layout not a valid XML", "LayoutViewItems")).SelectNodes("ViewItem");
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				foreach (XmlNode item in xmlNodeList)
				{
					XmlNode xmlNode = item.SelectSingleNode("Position");
					XmlNode xmlNode2 = item.SelectSingleNode("Size");
					if (xmlNode != null && xmlNode2 != null)
					{
						XmlNode xmlNode3 = xmlNode.SelectSingleNode("X");
						XmlNode xmlNode4 = xmlNode.SelectSingleNode("Y");
						XmlNode xmlNode5 = xmlNode2.SelectSingleNode("Width");
						XmlNode xmlNode6 = xmlNode2.SelectSingleNode("Height");
						if (xmlNode3 != null && xmlNode4 != null && xmlNode5 != null && xmlNode6 != null)
						{
							continue;
						}
					}
					throw new PropertyInvalidException("View Items layout not a valid XML", "LayoutViewItems");
				}
				return xmlNodeList.Count;
			}
			throw new PropertyInvalidException("View Items layout not a valid XML", "LayoutViewItems");
		}
		catch (XmlException)
		{
			throw new PropertyInvalidException("View Items layout not a valid XML", "LayoutViewItems");
		}
	}

	private static void ValidateIcon(string icon)
	{
		try
		{
			using MemoryStream stream = new MemoryStream(Convert.FromBase64String(icon));
			Image.FromStream(stream);
		}
		catch (FormatException)
		{
			throw new PropertyInvalidException("Icon is not a valid base64 image", "LayoutIcon");
		}
		catch (OutOfMemoryException)
		{
			throw new PropertyInvalidException("Icon is not a valid base64 image", "LayoutIcon");
		}
		catch (ArgumentException)
		{
			throw new PropertyInvalidException("Icon is not a valid base64 image", "LayoutIcon");
		}
	}

	private static ValidateResult ValidateViewItem(ConfigurationItem viewItem, out string xml, out int position, out Guid viewItemId)
	{
		xml = "";
		position = 0;
		viewItemId = Guid.Empty;
		ValidateResult validateResult = new ValidateResult
		{
			ValidatedOk = true
		};
		if (((IEnumerable<Property>)viewItem.Properties).TryGetPropertyValue("ViewItemId", out string value) && !Guid.TryParse(value, out viewItemId))
		{
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "ViewItemId",
					ErrorText = "Invalid guid"
				}
			};
			validateResult.ValidatedOk = false;
			return validateResult;
		}
		if (!((IEnumerable<Property>)viewItem.Properties).TryGetPropertyValue("ViewItemPosition", out string value2) || !int.TryParse(value2, out position))
		{
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "ViewItemPosition",
					ErrorText = "Invalid integer"
				}
			};
			validateResult.ValidatedOk = false;
			return validateResult;
		}
		if (!((IEnumerable<Property>)viewItem.Properties).TryGetPropertyValue("ViewItemDefinitionXml", out xml) || !ValidateViewItemXml(xml, out var viewItemId2))
		{
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "ViewItemDefinitionXml",
					ErrorText = "Invalid xml"
				}
			};
			validateResult.ValidatedOk = false;
			return validateResult;
		}
		if (viewItemId2 != Guid.Empty)
		{
			viewItemId = viewItemId2;
		}
		return validateResult;
	}

	private static bool ValidateViewItemXml(string xml, out Guid viewItemId)
	{
		viewItemId = Guid.Empty;
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("viewitem");
			if (xmlNode == null)
			{
				return false;
			}
			if (xmlNode.Attributes["id"] == null || !Guid.TryParse(xmlNode.Attributes["id"].Value, out viewItemId) || xmlNode.Attributes["displayname"] == null || xmlNode.Attributes["type"] == null)
			{
				return false;
			}
			return true;
		}
		catch (XmlException)
		{
			return false;
		}
	}

	private static ConfigurationItem CreateAddViewFirst(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "InvokeInfo",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.WizardAddView,
			ParentPath = item.ParentPath,
			Path = item.Path
		};
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(ViewPropertyDefinition.GetLayoutViewItemsProperty("<ViewItems><ViewItem><Position><X>0</X><Y>0</Y></Position><Size><Width>600</Width><Height>300</Height></Size></ViewItem><ViewItem><Position><X>0</X><Y>300</Y></Position><Size><Width>600</Width><Height>300</Height></Size></ViewItem></ViewItems>"));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateAddViewSecond(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		if (item.Properties.Length > 1)
		{
			return CreateAddViewThird(requestContext, item, path);
		}
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "InvokeInfo",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.WizardAddView,
			ParentPath = item.ParentPath,
			Path = item.Path
		};
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>();
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutViewItems", out string value))
		{
			int num = ValidateLayoutViewItems(value);
			Guid id = Guid.NewGuid();
			list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(id, uiVisible: true, isSettable: true));
			list.Add(GeneralPropertyDefinitions.GetNameProperty("New View", isSettable: true));
			list.Add(ViewPropertyDefinition.GetShortcutProperty(""));
			list.Add(ViewPropertyDefinition.GetViewLayoutTypeProperty("VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayoutCustom, VideoOS.RemoteClient.Application"));
			list.Add(ViewPropertyDefinition.GetViewLayoutCustomIdProperty(Guid.NewGuid().ToString()));
			list.Add(ViewPropertyDefinition.GetViewLayoutIconProperty("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"));
			list.Add(ViewPropertyDefinition.GetLayoutViewItemsProperty(value));
			list.Add(ViewPropertyDefinition.GetClientIdProperty(""));
			List<ConfigurationItem> list2 = new List<ConfigurationItem>();
			for (int i = 0; i < num; i++)
			{
				list2.Add(CreateEmptyViewItem(Guid.NewGuid().ToString(), i, item.Path));
			}
			configurationItem.Children = list2.ToArray();
			configurationItem.ChildrenFilled = true;
		}
		else
		{
			list.Add(ViewPropertyDefinition.GetLayoutViewItemsProperty("<ViewItems><ViewItem><Position><X>0</X><Y>0</Y></Position><Size><Width>600</Width><Height>300</Height></Size></ViewItem><ViewItem><Position><X>0</X><Y>300</Y></Position><Size><Width>600</Width><Height>300</Height></Size></ViewItem></ViewItems>"));
		}
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateAddViewThird(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath parentPath)
	{
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value);
		try
		{
			if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out value) && View.QueryView(requestContext, value) != null)
			{
				throw new PropertyInvalidException("View with provided ID already exists", "Id");
			}
		}
		catch (ViewNotFoundException)
		{
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("View", value);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, configurationItemPath, parentPath.Id, out var definitionXml, out var name, out var count);
		if (!validateResult.ValidatedOk)
		{
			throw new ArgumentException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
		}
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		ConfigurationItem[] children = item.Children;
		for (int i = 0; i < children.Length; i++)
		{
			validateResult = ValidateViewItem(children[i], out var xml, out var position, out var _);
			if (!validateResult.ValidatedOk)
			{
				throw new ArgumentException(validateResult.ErrorResults[0].ErrorProperty);
			}
			if (position < 0 || position > count || dictionary.ContainsKey(position))
			{
				throw new PropertyInvalidException("ViewItem position is invalid");
			}
			dictionary[position] = xml;
		}
		try
		{
			string id = parentPath.Id;
			View view = new View
			{
				Id = value,
				ViewGroupId = id,
				CustomProperties = null,
				DefinitionXml = definitionXml,
				Description = "",
				Name = name
			};
			List<ViewItem> list = new List<ViewItem>();
			children = item.Children;
			for (int i = 0; i < children.Length; i++)
			{
				ValidateViewItem(children[i], out var xml2, out var position2, out var viewItemId2);
				list.Add(new ViewItem
				{
					CustomProperties = null,
					DefinitionXml = xml2,
					Id = viewItemId2.ToString(),
					Position = position2,
					ViewId = value,
					ViewGroupId = id
				});
			}
			view.ViewItems = list.ToArray();
			View.CreateView(requestContext, id, view);
			return MethodTypesHelper.CreateInvokeResult(item, configurationItemPath.Text, "Result", "Success", true);
		}
		catch (ViewUpdateException)
		{
			throw new PathNotFoundException(configurationItemPath.Text);
		}
		catch (DatabaseConfigUnknownErrorException)
		{
			throw new InternalConfigurationApiException("Internal server error during view creation");
		}
	}

	public static ConfigurationItem CreateRemoveView(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "InvokeInfo",
			UIHint = "DirectionVertical",
			DisplayName = ConfigurationApiResources.WizardRemoveView,
			ParentPath = item.ParentPath,
			Path = item.Path
		};
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(ViewPropertyDefinition.GetMembersAsEnumsProperty(members));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static ConfigurationItem RemoveView(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
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
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid path. " + value, "ItemSelection", inner);
		}
		if (configurationItemPath.IsFolder || !string.Equals("View", configurationItemPath.ItemType, StringComparison.Ordinal) || !configurationItemPath.Id.IsGuid())
		{
			throw new PropertyInvalidException("Invalid path. " + configurationItemPath, "ItemSelection");
		}
		string id = configurationItemPath.Id;
		try
		{
			View.DeleteView(requestContext, id);
		}
		catch (RemoveNonExistingViewException)
		{
			throw new PathNotFoundException(configurationItemPath.Text);
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(configurationItem);
	}

	private static ConfigurationItem CreateClearViewItem(ConfigurationItem item, ConfigurationItem[] configurationItems, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.DisplayName = ConfigurationApiResources.WizardClearViewItem;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			ViewPropertyDefinition.GetViewItemIdProperty(string.Empty),
			ViewPropertyDefinition.GetClientIdProperty(Guid.NewGuid().ToString())
		};
		return configurationItem;
	}

	private static ConfigurationItem ClearViewItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "ClearViewItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 894);
		item.NullCheck("", "ClearViewItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 895);
		path.NullCheck("", "ClearViewItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 896);
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ViewItemId", out string viewItemId))
		{
			throw new PropertyInvalidException("Missing property", "ViewItemId");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientId", out string value) || !Guid.TryParse(value, out var _))
		{
			value = Guid.NewGuid().ToString();
		}
		try
		{
			if (!View.QueryViewItems(requestContext, path.Id).Any((ViewItem v) => string.Equals(v.Id, viewItemId, StringComparison.OrdinalIgnoreCase)))
			{
				throw new PathNotFoundException("View item with given ID not found: " + viewItemId);
			}
			ViewItem.ClearViewItem(requestContext, viewItemId, value);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (ViewNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem CreateClearView(ConfigurationItem item, ConfigurationItem[] configurationItems, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.DisplayName = ConfigurationApiResources.WizardClearView;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new Property[1] { ViewPropertyDefinition.GetClientIdProperty(Guid.NewGuid().ToString()) };
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem ClearView(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "ClearView", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 946);
		item.NullCheck("", "ClearView", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 947);
		path.NullCheck("", "ClearView", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 948);
		try
		{
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientId", out string value) || !Guid.TryParse(value, out var _))
			{
				value = Guid.NewGuid().ToString();
			}
			View view = View.ClearView(requestContext, path.Id, value);
			if (view == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = (string.IsNullOrEmpty(view.ViewGroupId) ? ConfigurationItemPath.CreateItemPath("Monitor", view.MonitorId) : ConfigurationItemPath.CreateFolderPath("ViewGroup", view.ViewGroupId, "ViewFolder"));
			return MethodTypesHelper.CreateInvokeResult(ConvertToViewConfigurationItem(parentPath, view), null, "Result", "Success", true);
		}
		catch (ViewNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem CreateApplyViewToView(ConfigurationItem item, ConfigurationItem[] configurationItems, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyViewToView;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			GeneralPropertyDefinitions.GetPathProperty("ViewToApply", ConfigurationApiResources.PropertyViewToApply, "", new string[1] { "View" }, "PropertyViewToApply", isSettable: true),
			ViewPropertyDefinition.GetClientIdProperty(Guid.NewGuid().ToString())
		};
		return configurationItem;
	}

	private static ConfigurationItem ApplyViewToView(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "ApplyViewToView", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 1004);
		item.NullCheck("", "ApplyViewToView", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 1005);
		path.NullCheck("", "ApplyViewToView", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\View\\ViewConfigurationApi.cs", 1006);
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ViewToApply", out string value))
		{
			throw new PropertyInvalidException("Missing property", "ViewToApply");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientId", out string value2) || !Guid.TryParse(value2, out var _))
		{
			value2 = Guid.NewGuid().ToString();
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
		if (configurationItemPath.ItemType != "View")
		{
			throw new PropertyInvalidException("Invalid path", "ViewToApply");
		}
		if (string.Equals(configurationItemPath.Id, path.Id, StringComparison.OrdinalIgnoreCase))
		{
			throw new PropertyInvalidException("Cannot apply view to itself", "ViewToApply");
		}
		try
		{
			View view = View.QueryView(requestContext, path.Id);
			if (view == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			view = View.ApplyViewToView(requestContext, configurationItemPath.Id, path.Id, value2);
			ConfigurationItemPath parentPath = (string.IsNullOrEmpty(view.ViewGroupId) ? ConfigurationItemPath.CreateItemPath("Monitor", view.MonitorId) : ConfigurationItemPath.CreateFolderPath("ViewGroup", view.ViewGroupId, "ViewFolder"));
			return MethodTypesHelper.CreateInvokeResult(ConvertToViewConfigurationItem(parentPath, view), null, "Result", "Success", true);
		}
		catch (ViewNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem CreateEmptyViewItem(string viewItemId, int position, string parentPath)
	{
		return ConstructViewItemConfigurationItem(viewItemId, position, string.Format(CultureInfo.InvariantCulture, "<viewitem id=\"{0}\" displayname=\"Empty ViewItem\" shortcut=\"\" type=\"VideoOS.RemoteClient.Application.Data.Configuration.EmptyViewItem, VideoOS.RemoteClient.Application\"><properties /></viewitem>", viewItemId), parentPath);
	}
}

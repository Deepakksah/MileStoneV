using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform.Admin;

namespace VideoOS.Platform.Util;

public static class ConfigurationUtil
{
	public static bool SaveItemConfiguration(XmlNode topNode, XmlNode currentItemNode, Item item)
	{
		if (topNode == null)
		{
			throw new ArgumentException("topNode cannot be null", "topNode");
		}
		if (item == null)
		{
			throw new ArgumentException("item cannot be null", "item");
		}
		string text = "";
		bool result = false;
		try
		{
			if (item != null && item.FQID != null && item.FQID.ServerId != null)
			{
				Guid id = ((item.FQID.ObjectId == Guid.Empty) ? item.FQID.ServerId.Id : item.FQID.ObjectId);
				XmlWriter xmlWriter;
				if (currentItemNode != null)
				{
					text = currentItemNode.InnerXml;
					currentItemNode.InnerXml = "";
					xmlWriter = currentItemNode.CreateNavigator().AppendChild();
					WriteItemToXmlWriter(xmlWriter, item, id);
				}
				else
				{
					xmlWriter = topNode.CreateNavigator().SelectSingleNode("Configurations").AppendChild();
					xmlWriter.WriteStartElement("GUID" + item.FQID.Kind.ToString());
					xmlWriter.WriteAttributeString("itemID", id.ToString());
					WriteItemToXmlWriter(xmlWriter, item, id);
				}
				xmlWriter.Flush();
				xmlWriter.Close();
				result = currentItemNode == null || currentItemNode.InnerXml != text;
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("PluginConfiguration: XmlInvalid - ", exception);
			if (currentItemNode != null && !string.IsNullOrEmpty(text))
			{
				currentItemNode.InnerXml = text;
			}
		}
		return result;
	}

	private static void WriteItemToXmlWriter(XmlWriter tw, Item item, Guid id)
	{
		tw.WriteElementString("Id", id.ToString());
		tw.WriteElementString("Name", item.Name);
		item.FQID.Serialize(tw);
		if (item.Properties == null || item.Properties.Count <= 0)
		{
			return;
		}
		tw.WriteStartElement("properties");
		foreach (string key in item.Properties.Keys)
		{
			tw.WriteStartElement("property");
			tw.WriteAttributeString("name", key);
			tw.WriteAttributeString("value", item.Properties[key]);
			tw.WriteEndElement();
		}
		tw.WriteEndElement();
	}

	public static List<Item> GetItemsForItemKind(XmlNode node, Item parentItem, Guid kind)
	{
		try
		{
			List<Item> list = new List<Item>();
			if (kind == Guid.Empty || node == null)
			{
				return list;
			}
			XmlNodeList xmlNodeList = node.SelectNodes("Configurations/GUID" + kind.ToString());
			if (xmlNodeList != null)
			{
				foreach (XmlNode item2 in xmlNodeList)
				{
					Item item = DeserializeItem(item2);
					if (parentItem == null)
					{
						list.Add(item);
					}
					else if (item.FQID.ParentId == parentItem.FQID.ObjectId || item.FQID.ParentId == parentItem.FQID.ServerId.Id)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("GetItemForItemType", exception);
			return null;
		}
	}

	public static void DeleteItemConfiguration(XmlNode topNode, XmlNode itemNode)
	{
		try
		{
			if (itemNode != null)
			{
				topNode.SelectSingleNode("Configurations").RemoveChild(itemNode);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("DeleteItemConfiguration - ", exception);
		}
	}

	public static Item DeserializeItem(XmlNode xmlNode)
	{
		if (xmlNode == null)
		{
			return null;
		}
		try
		{
			string text = "";
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("Name");
			XmlNode xmlNode3 = xmlNode.SelectSingleNode("FQID");
			if (xmlNode2 == null || xmlNode3 == null)
			{
				return null;
			}
			text = xmlNode2.InnerText;
			Item item = new Item(new FQID(xmlNode3), text);
			XmlNode xmlNode4 = xmlNode.SelectSingleNode("properties");
			if (xmlNode4 != null)
			{
				XmlNodeList xmlNodeList = xmlNode4.SelectNodes("property");
				if (xmlNodeList != null)
				{
					foreach (XmlNode item2 in xmlNodeList)
					{
						string value = item2.Attributes["name"].Value;
						string value2 = item2.Attributes["value"].Value;
						item.Properties[value] = value2;
					}
				}
			}
			return item;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("DeserializeItem", exception);
		}
		return null;
	}

	public static ItemNode GetParentForItemNode(List<ItemNode> itemNodes, Guid parentKind)
	{
		foreach (ItemNode itemNode in itemNodes)
		{
			if (itemNode.ItemKind == parentKind)
			{
				return itemNode;
			}
			if (itemNode.ItemNodes != null)
			{
				ItemNode parentForItemNode = GetParentForItemNode(itemNode.ItemNodes, parentKind);
				if (parentForItemNode != null)
				{
					return parentForItemNode;
				}
			}
		}
		return null;
	}
}

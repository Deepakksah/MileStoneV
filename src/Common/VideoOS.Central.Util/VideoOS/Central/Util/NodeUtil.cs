using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VideoOS.Central.Util;

public class NodeUtil
{
	private NodeUtil()
	{
	}

	public static string InnerText(XmlNode node, string id)
	{
		XmlElement xmlElement = node[id];
		if (xmlElement != null)
		{
			return xmlElement.InnerText;
		}
		return "";
	}

	public static int InnerInt(XmlNode node, string id, int defaultInt)
	{
		XmlElement xmlElement = node[id];
		if (xmlElement != null)
		{
			try
			{
				return Convert.ToInt32(xmlElement.InnerText, 10);
			}
			catch (Exception)
			{
				return defaultInt;
			}
		}
		return defaultInt;
	}

	public static bool InnerBool(XmlNode node, string id, bool defaultBool)
	{
		XmlElement xmlElement = node[id];
		if (xmlElement != null)
		{
			try
			{
				return Convert.ToBoolean(xmlElement.InnerText, CultureInfo.InvariantCulture);
			}
			catch (Exception ex) when (ex is FormatException || ex is NullReferenceException)
			{
				return defaultBool;
			}
		}
		return defaultBool;
	}

	public static Hashtable InnerList(XmlNode node)
	{
		Hashtable hashtable = new Hashtable();
		if (node != null)
		{
			try
			{
				foreach (XmlNode item in node)
				{
					string innerText = item.InnerText;
					if (!string.IsNullOrEmpty(innerText))
					{
						hashtable.Add(innerText, "");
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return hashtable;
	}

	public static Hashtable InnerListAttr(XmlNode node, string attr1, string attr2)
	{
		Hashtable hashtable = new Hashtable();
		if (node != null)
		{
			try
			{
				foreach (XmlNode item in node)
				{
					string innerText = item.Attributes[attr1].InnerText;
					string innerText2 = item.Attributes[attr2].InnerText;
					if (!string.IsNullOrEmpty(innerText) && !string.IsNullOrEmpty(innerText2))
					{
						hashtable.Add(innerText, innerText2);
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return hashtable;
	}

	public static List<string> InnerStringList(XmlNode node)
	{
		List<string> list = new List<string>();
		if (node != null)
		{
			try
			{
				foreach (XmlNode item in node)
				{
					string innerText = item.InnerText;
					if (!string.IsNullOrEmpty(innerText))
					{
						list.Add(innerText);
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}
}

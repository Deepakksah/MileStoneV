using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace VideoOS.Management.Server;

public sealed class ViewItemsInformationProvider
{
	private static readonly XmlReaderSettings Settings = new XmlReaderSettings
	{
		CheckCharacters = true,
		DtdProcessing = DtdProcessing.Prohibit,
		MaxCharactersFromEntities = 1024L
	};

	private static readonly Dictionary<string, Guid> ViewItemTypes = new Dictionary<string, Guid>
	{
		{
			"EmptyViewItem",
			ViewAndLayoutItem.EmptyBuiltinId
		},
		{
			"CameraContentType.CameraViewItem",
			ViewAndLayoutItem.CameraBuiltinId
		},
		{
			"HtmlViewItem",
			ViewAndLayoutItem.HTMLBuiltinId
		},
		{
			"HotspotViewItem",
			ViewAndLayoutItem.HotspotBuiltinId
		},
		{
			"MapViewItem",
			ViewAndLayoutItem.MapBuiltinId
		},
		{
			"MatrixViewItem",
			ViewAndLayoutItem.MatrixBuiltinId
		},
		{
			"CarrouselViewItem",
			ViewAndLayoutItem.CarrouselBuiltinId
		}
	};

	public static IEnumerable<ViewItemInformation> GetViewItemsInformation(IGeminiRequestContext requestContext, Guid viewId)
	{
		List<ViewItemInformation> list = new List<ViewItemInformation>();
		string id;
		string displayName;
		string shortcut;
		string viewLayoutType;
		string customId;
		string layoutIcon;
		string viewItemsXml;
		string clientId;
		bool flag = ViewConfigurationApi.DecodeViewDefinition(View.QueryView(requestContext, viewId.ToString()).DefinitionXml, out id, out displayName, out shortcut, out viewLayoutType, out customId, out layoutIcon, out viewItemsXml, out clientId);
		ViewItem[] array = View.QueryViewItems(requestContext, viewId.ToString());
		if (array != null)
		{
			Rectangle[] layoutRectangles = GetLayoutRectangles(viewItemsXml);
			ViewItem[] array2 = array;
			foreach (ViewItem viewItem in array2)
			{
				Rectangle viewItemRectangle = (flag ? layoutRectangles.ElementAtOrDefault(viewItem.Position) : default(Rectangle));
				list.Add(CreateViewItemInformation(viewItem, viewItemRectangle));
			}
		}
		return list;
	}

	private static Rectangle[] GetLayoutRectangles(string rectanglesDefinitionXml)
	{
		List<Rectangle> list = new List<Rectangle>();
		foreach (XmlElement item in GetDocumentFromXml(rectanglesDefinitionXml).FirstChild.ChildNodes.OfType<XmlElement>())
		{
			List<XmlElement> source = item.ChildNodes.OfType<XmlElement>().ToList();
			IEnumerable<XmlElement> elements = source.First((XmlElement x) => x.Name == "Position").ChildNodes.OfType<XmlElement>();
			int intValueForRectangle = GetIntValueForRectangle(elements, "X");
			int intValueForRectangle2 = GetIntValueForRectangle(elements, "Y");
			IEnumerable<XmlElement> elements2 = source.First((XmlElement x) => x.Name == "Size").ChildNodes.OfType<XmlElement>();
			int intValueForRectangle3 = GetIntValueForRectangle(elements2, "Width");
			int intValueForRectangle4 = GetIntValueForRectangle(elements2, "Height");
			list.Add(new Rectangle(intValueForRectangle, intValueForRectangle2, intValueForRectangle3, intValueForRectangle4));
		}
		return list.ToArray();
	}

	private static int GetIntValueForRectangle(IEnumerable<XmlElement> elements, string fieldName)
	{
		if (!int.TryParse(elements.First((XmlElement p) => p.Name == fieldName).InnerText, out var result))
		{
			return 0;
		}
		return result;
	}

	private static ViewItemInformation CreateViewItemInformation(ViewItem viewItem, Rectangle viewItemRectangle)
	{
		Guid viewItemType = Guid.Empty;
		XmlNode firstChild = GetDocumentFromXml(viewItem.DefinitionXml).FirstChild;
		XmlNode xmlNode = firstChild.SelectSingleNode("iteminfo");
		XmlNode propertyNode = firstChild.SelectSingleNode("properties");
		Dictionary<string, string> viewAttributes = GetViewAttributes(xmlNode);
		Dictionary<string, string> viewProperties = GetViewProperties(propertyNode);
		if (firstChild.Attributes?["type"] != null)
		{
			string value = firstChild.Attributes["type"].Value;
			foreach (string key in ViewItemTypes.Keys)
			{
				if (value.Contains(key))
				{
					viewItemType = ViewItemTypes[key];
					break;
				}
			}
			if (value.Contains("MIP.PlatformViewItem") && xmlNode != null)
			{
				if (xmlNode.Attributes != null)
				{
					XmlAttribute xmlAttribute = xmlNode.Attributes["id"];
					if (xmlAttribute != null && Guid.TryParse(xmlAttribute.Value, out var result))
					{
						viewItemType = result;
					}
				}
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("viewitem");
				if (xmlNode2 != null)
				{
					XmlNode xmlNode3 = xmlNode2.SelectSingleNode("properties");
					if (xmlNode3 != null)
					{
						XmlNodeList xmlNodeList = xmlNode3.SelectNodes("property");
						if (xmlNodeList != null)
						{
							foreach (XmlNode item in xmlNodeList)
							{
								if (item.Attributes != null)
								{
									XmlAttribute xmlAttribute2 = item.Attributes["name"];
									XmlAttribute xmlAttribute3 = item.Attributes["value"];
									if (xmlAttribute2 != null && xmlAttribute3 != null)
									{
										viewProperties.Add(xmlAttribute2.Value, xmlAttribute3.Value);
									}
								}
							}
						}
					}
				}
			}
		}
		return new ViewItemInformation(viewItemRectangle, viewItemType, viewProperties, viewAttributes);
	}

	private static XmlDocument GetDocumentFromXml(string xmlDocument)
	{
		MemoryStream memoryStream = null;
		XmlDocument xmlDocument2 = new XmlDocument();
		try
		{
			memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlDocument));
			using XmlReader reader = XmlReader.Create(memoryStream, Settings);
			memoryStream = null;
			xmlDocument2.Load(reader);
			return xmlDocument2;
		}
		finally
		{
			memoryStream?.Dispose();
		}
	}

	private static Dictionary<string, string> GetViewProperties(XmlNode propertyNode)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (propertyNode?.Attributes != null)
		{
			foreach (XmlElement item in propertyNode.ChildNodes.OfType<XmlElement>())
			{
				string value = item.Attributes["name"].Value;
				string value2 = item.Attributes["value"].Value;
				if (value != null && value2 != null)
				{
					dictionary.Add(value, value2);
				}
			}
		}
		return dictionary;
	}

	private static Dictionary<string, string> GetViewAttributes(XmlNode itemInfoNode)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (itemInfoNode != null && itemInfoNode.Attributes != null)
		{
			foreach (XmlAttribute attribute in itemInfoNode.Attributes)
			{
				dictionary.Add(attribute.Name, attribute.Value);
			}
		}
		return dictionary;
	}
}

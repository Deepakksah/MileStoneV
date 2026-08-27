using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Installer.Common.Utilities;

public static class XmlUpdater
{
	public static void UpdateXmlFiles(string pathToOldFile, string pathToNewFile, string pathToOutputFile, Rules startPoint)
	{
		if (startPoint == null)
		{
			throw new ArgumentNullException("startPoint");
		}
		createResultFile(pathToOutputFile, pathToNewFile);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(pathToNewFile);
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.Load(pathToOldFile);
		XmlDocument xmlDocument3 = new XmlDocument();
		xmlDocument3.Load(pathToOutputFile);
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
		XPathNavigator xPathNavigator2 = xmlDocument2.CreateNavigator();
		XPathNavigator xPathNavigator3 = xmlDocument3.CreateNavigator();
		XPathNodeIterator xPathNodeIterator = xPathNavigator.Select("//*");
		xPathNodeIterator.MoveNext();
		xPathNodeIterator.MoveNext();
		while (xPathNodeIterator.MoveNext())
		{
			string nodeXpath = GetNodeXpath(xPathNodeIterator);
			if (startPoint.UseValueFromNewFile(nodeXpath))
			{
				continue;
			}
			xPathNavigator3.MoveTo(xPathNavigator3.SelectSingleNode(nodeXpath));
			if (xPathNavigator3.HasAttributes)
			{
				XPathNavigator xPathNavigator4 = xPathNavigator3.Clone();
				xPathNavigator4.MoveToFirstAttribute();
				do
				{
					string attributeXpath = GetAttributeXpath(xPathNavigator4);
					if (startPoint.UseValueFromNewFile(attributeXpath))
					{
						continue;
					}
					XPathNavigator xPathNavigator5 = xPathNavigator2.SelectSingleNode(nodeXpath);
					if (xPathNavigator5 == null)
					{
						continue;
					}
					xPathNavigator2.MoveTo(xPathNavigator5);
					if (xPathNavigator2.HasAttributes)
					{
						XPathNavigator xPathNavigator6 = xPathNavigator2.Clone();
						if (xPathNavigator6.MoveToAttribute(xPathNavigator4.Name, xPathNavigator6.NamespaceURI))
						{
							xPathNavigator4.SetValue(xPathNavigator6.Value);
						}
					}
				}
				while (xPathNavigator4.MoveToNextAttribute());
			}
			if (xPathNavigator3.SelectChildren(XPathNodeType.Element).Count == 0)
			{
				XPathNavigator xPathNavigator7 = xPathNavigator2.SelectSingleNode(nodeXpath);
				if (xPathNavigator7 != null && !string.IsNullOrEmpty(xPathNavigator7.InnerXml))
				{
					xPathNavigator3.MoveTo(xPathNavigator3.SelectSingleNode(nodeXpath));
					xPathNavigator3.InnerXml = xPathNavigator7.InnerXml;
				}
			}
		}
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		using XmlWriter xmlWriter = XmlWriter.Create(pathToOutputFile, xmlWriterSettings);
		xmlDocument3.WriteTo(xmlWriter);
		xmlWriter.Flush();
	}

	private static string GetAttributeXpath(XPathNavigator attributes)
	{
		XPathNodeIterator xPathNodeIterator = attributes.SelectAncestors(XPathNodeType.Element, matchSelf: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Insert(0, "/@" + attributes.Name);
		foreach (XPathNavigator item in xPathNodeIterator)
		{
			stringBuilder.Insert(0, "/" + item.Name);
		}
		return stringBuilder.ToString();
	}

	private static string GetNodeXpath(XPathNodeIterator nodes)
	{
		XPathNodeIterator xPathNodeIterator = nodes.Current.SelectAncestors(XPathNodeType.Element, matchSelf: true);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (XPathNavigator item in xPathNodeIterator)
		{
			stringBuilder.Insert(0, "/" + item.Name);
		}
		return stringBuilder.ToString();
	}

	private static void createResultFile(string pathToOutputFile, string pathToNewFile)
	{
		File.Copy(pathToNewFile, pathToOutputFile, overwrite: true);
	}
}

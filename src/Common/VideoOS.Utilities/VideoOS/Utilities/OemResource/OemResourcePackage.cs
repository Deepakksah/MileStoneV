using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using VideoOS.Common.Crypto;

namespace VideoOS.Utilities.OemResource;

public class OemResourcePackage
{
	private readonly Dictionary<string, OemResourceItemBase> _oemResourceItemsDictionary = new Dictionary<string, OemResourceItemBase>();

	private static string _signatureString = string.Empty;

	private string _signatureMethod;

	public IEnumerable OemResourceItems => _oemResourceItemsDictionary;

	public OemResourcePackage(XmlDocument oemPackageXml)
	{
		if (oemPackageXml == null)
		{
			throw new ArgumentNullException("oemPackageXml");
		}
		LoadFromXml(oemPackageXml);
	}

	private bool LoadFromXml(XmlDocument oemPackageXml)
	{
		_oemResourceItemsDictionary.Clear();
		try
		{
			XmlElement xmlElement = oemPackageXml.DocumentElement?["Signature"];
			if (xmlElement == null)
			{
				return false;
			}
			XmlElement xmlElement2 = xmlElement["SignedInfo"]?["SignatureMethod"];
			if (xmlElement2 == null)
			{
				return false;
			}
			XmlAttribute xmlAttribute = xmlElement2.Attributes["Algorithm"];
			if (xmlAttribute == null)
			{
				return false;
			}
			_signatureMethod = xmlAttribute.Value;
			foreach (XmlNode item in oemPackageXml.SelectSingleNode("/OemResourcePackage/ResourceItems"))
			{
				OemResourceItemBase oemResourceItemBase = OemResourceItemBase.GenerateResourceItem(item);
				if (oemResourceItemBase != null)
				{
					_oemResourceItemsDictionary.Add(oemResourceItemBase.Name, oemResourceItemBase);
				}
			}
			XmlNodeList elementsByTagName = oemPackageXml.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
			if (elementsByTagName.Count > 0)
			{
				_signatureString = elementsByTagName[0].OuterXml;
			}
			else
			{
				_signatureString = string.Empty;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		return true;
	}

	public XmlDocument GetOemPackageXml()
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("OemResourcePackage");
			xmlTextWriter.WriteStartElement("ResourceItems");
			foreach (OemResourceItemBase value in _oemResourceItemsDictionary.Values)
			{
				xmlTextWriter.WriteStartElement("ResourceItem");
				value.Serialize(xmlTextWriter);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteRaw(_signatureString);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			memoryStream.Position = 0L;
			xmlDocument.Load(memoryStream);
			memoryStream.Close();
			return xmlDocument;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
	}

	public bool ValidateSignature()
	{
		try
		{
			string keyXml = ((!StringComparer.Ordinal.Equals(_signatureMethod, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")) ? Keys.CompanyPublicKey : Keys.OemResourcePublicKey);
			new XmlSignatureHelper(keyXml).VerifyXmlDocument(GetOemPackageXml());
			return true;
		}
		catch (SecurityException)
		{
		}
		return false;
	}

	public bool IsResourceItemAvailable(string name)
	{
		return _oemResourceItemsDictionary.ContainsKey(name);
	}

	public OemResourceItemBase GetResourceItem(string name)
	{
		_oemResourceItemsDictionary.TryGetValue(name, out var value);
		return value;
	}

	public void Add(OemResourceItemBase oemResourceItem)
	{
		_oemResourceItemsDictionary.Add(oemResourceItem.Name, oemResourceItem);
	}

	[Obsolete]
	public void Insert(int index, OemResourceItemBase oemResourceItem)
	{
		Add(oemResourceItem);
	}

	public void Remove(OemResourceItemBase oemResourceItem)
	{
		_oemResourceItemsDictionary.Remove(oemResourceItem.Name);
	}

	public int ResourceItemsCount()
	{
		return _oemResourceItemsDictionary.Count;
	}
}

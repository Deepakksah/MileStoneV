using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.License.OemResources;

public class OemResourcePackage
{
	private static ArrayList _oemResourceItemsArray = new ArrayList();

	private static string _signatureString = string.Empty;

	public IEnumerable OemResourceItems => _oemResourceItemsArray;

	public bool LoadFromXml(XmlDocument oemPackageXml)
	{
		_oemResourceItemsArray.Clear();
		try
		{
			foreach (XmlNode item in oemPackageXml.SelectSingleNode("/OemResourcePackage/ResourceItems"))
			{
				OemResourceItemBase oemResourceItemBase = OemResourceItemBase.GenerateResourceItem(item);
				if (oemResourceItemBase != null)
				{
					_oemResourceItemsArray.Add(oemResourceItemBase);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public XmlDocument GetOemPackageXml()
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("OemResourcePackage");
			xmlTextWriter.WriteStartElement("ResourceItems");
			foreach (OemResourceItemBase item in _oemResourceItemsArray)
			{
				xmlTextWriter.WriteStartElement("ResourceItem");
				item.Serialize(xmlTextWriter);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteRaw(_signatureString);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			memoryStream.Position = 0L;
			xmlDocument.Load(memoryStream);
			return xmlDocument;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public bool IsResourceItemAvailable(string name)
	{
		foreach (OemResourceItemBase item in _oemResourceItemsArray)
		{
			if (item.Name == name)
			{
				return true;
			}
		}
		return false;
	}

	public OemResourceItemBase GetResourceItem(string name)
	{
		foreach (OemResourceItemBase item in _oemResourceItemsArray)
		{
			if (item.Name == name)
			{
				return item;
			}
		}
		return null;
	}

	public void Add(OemResourceItemBase oemResourceItem)
	{
		_oemResourceItemsArray.Add(oemResourceItem);
	}

	public void Insert(int index, OemResourceItemBase oemResourceItem)
	{
		_oemResourceItemsArray.Insert(index, oemResourceItem);
	}

	public void Remove(OemResourceItemBase oemResourceItem)
	{
		_oemResourceItemsArray.Remove(oemResourceItem);
	}

	public int ResourceItemsCount()
	{
		return _oemResourceItemsArray.Count;
	}
}

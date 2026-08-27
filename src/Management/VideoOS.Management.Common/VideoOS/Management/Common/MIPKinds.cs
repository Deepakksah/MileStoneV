using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public class MIPKinds
{
	private List<MIPKind> mipKinds = new List<MIPKind>();

	public MIPKind[] MIPKindList => mipKinds.ToArray();

	public MIPKinds()
	{
	}

	public MIPKinds(MIPKind[] mipKinds)
	{
		this.mipKinds.AddRange(mipKinds);
	}

	public void Add(MIPKind mipKind)
	{
		mipKinds.Add(mipKind);
	}

	public void Remove(MIPKind mipKind)
	{
		mipKinds.Remove(mipKind);
	}

	public MIPKinds(string mipKindsXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(mipKindsXml);
		foreach (XmlNode item in xmlDocument.SelectNodes("MIPKinds/MIPKind"))
		{
			XmlNode xmlNode2 = item.SelectSingleNode("Kind");
			if (xmlNode2 != null)
			{
				Guid kind = new Guid(xmlNode2.InnerText);
				XmlNodeList xmlNodeList = item.SelectNodes("SubFilters/SubFilter");
				if (xmlNodeList != null)
				{
					List<Guid> list = new List<Guid>();
					foreach (XmlNode item2 in xmlNodeList)
					{
						list.Add(new Guid(item2.InnerText));
					}
					Add(new MIPKind(kind, list.ToArray()));
					continue;
				}
				throw new ArgumentException("Parameter mipKindsXml is not valid. SubFilters not found.");
			}
			throw new ArgumentException("Parameter mipKindsXml is not valid. Kind not found.");
		}
	}

	public void SaveAsXml(XmlWriter writer, bool includeHeader)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		if (includeHeader)
		{
			writer.WriteStartDocument();
		}
		writer.WriteStartElement("MIPKinds");
		foreach (MIPKind mipKind in mipKinds)
		{
			writer.WriteStartElement("MIPKind");
			writer.WriteElementString("Kind", mipKind.Kind.ToString());
			writer.WriteStartElement("SubFilters");
			Guid[] subFilters = mipKind.SubFilters;
			for (int i = 0; i < subFilters.Length; i++)
			{
				Guid guid = subFilters[i];
				writer.WriteElementString("SubFilter", guid.ToString());
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
		if (includeHeader)
		{
			writer.WriteEndDocument();
		}
	}

	public string SaveAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			OmitXmlDeclaration = true,
			Indent = true
		}))
		{
			SaveAsXml(writer, includeHeader: true);
		}
		return stringBuilder.ToString();
	}
}

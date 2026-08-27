using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableProperties<TKey, TValue, TFactory> : Dictionary<TKey, TValue>, IXmlSerializable where TFactory : IKeyValueFactory<TKey, TValue>, new()
{
	private static readonly TFactory Factory = new TFactory();

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			Add(Factory.CreateKey(reader.Name), Factory.CreateValue(reader.Value));
		}
		reader.MoveToElement();
	}

	public void WriteXml(XmlWriter writer)
	{
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<TKey, TValue> current = enumerator.Current;
			if (current.Key != null && current.Value != null)
			{
				writer.WriteAttributeString(current.Key.ToString(), current.Value.ToString());
			}
		}
	}
}

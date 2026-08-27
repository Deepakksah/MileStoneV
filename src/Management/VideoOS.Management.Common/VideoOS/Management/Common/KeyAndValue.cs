using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace VideoOS.Management.Common;

public class KeyAndValue
{
	public string Key { get; set; }

	public object Value { get; set; }

	public KeyAndValue()
	{
	}

	public KeyAndValue(string key, object value)
	{
		Key = key;
		Value = value;
	}

	public static string ToXml(IEnumerable<KeyAndValue> list)
	{
		return ToXml(list.ToArray());
	}

	public static string ToXml(KeyAndValue[] array)
	{
		using MemoryStream memoryStream = new MemoryStream();
		new XmlSerializer(typeof(KeyAndValue[])).Serialize(memoryStream, array);
		memoryStream.Position = 0L;
		using StreamReader streamReader = new StreamReader(memoryStream);
		return streamReader.ReadToEnd();
	}

	public static KeyAndValue[] FromXml(string xml)
	{
		using StringReader textReader = new StringReader(xml);
		return (KeyAndValue[])new XmlSerializer(typeof(KeyAndValue[])).Deserialize(textReader);
	}
}

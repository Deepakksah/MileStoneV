using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace VideoOS.Management.Common;

public static class XmlSerializationHelper
{
	public static T FromXml<T>(string xml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		using StringReader textReader = new StringReader(xml);
		return (T)xmlSerializer.Deserialize(textReader);
	}

	public static string ToXml<T>(T @object)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			using MemoryStream memoryStream = new MemoryStream();
			xmlSerializer.Serialize(memoryStream, @object, xmlSerializerNamespaces);
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}
}

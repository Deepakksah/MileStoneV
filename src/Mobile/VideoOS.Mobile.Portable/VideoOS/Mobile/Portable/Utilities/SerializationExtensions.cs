using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.Utilities;

public static class SerializationExtensions
{
	public static string XmlSerialize<T>(this T toSerialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
		using StringWriter stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, toSerialize);
		return stringWriter.ToString();
	}

	public static T XmlDeserialize<T>(this string toDeserialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		using StringReader textReader = new StringReader(toDeserialize);
		return (T)xmlSerializer.Deserialize(textReader);
	}

	public static byte[] ByteSerialize<T>(this T toSerialize)
	{
		return Encoding.Unicode.GetBytes(toSerialize.XmlSerialize());
	}

	public static T ByteDeserialize<T>(this byte[] toDeserialize)
	{
		return Encoding.Unicode.GetString(toDeserialize).XmlDeserialize<T>();
	}
}

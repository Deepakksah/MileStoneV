using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.Serializers;

public class XmlMessageSerializer<T> : IMessageSerializer<T>
{
	private readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

	public const int MinimalBufferSize = 2048;

	private static XmlReaderSettings XmlReaderSettings => SecurityXmlReaderSettings.Settings;

	public T Deserialize(Stream stream)
	{
		using XmlReader xmlReader = XmlReader.Create(stream, XmlReaderSettings);
		return (T)_serializer.Deserialize(xmlReader);
	}

	public void Serialize(T message, Stream stream)
	{
		using MemoryStream memoryStream = new MemoryStream(2048);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = Encoding.UTF8;
		using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
		{
			_serializer.Serialize(xmlWriter, message);
		}
		byte[] preamble = Encoding.UTF8.GetPreamble();
		memoryStream.SetLength(memoryStream.Position);
		memoryStream.Position = preamble.Length;
		memoryStream.CopyTo(stream, 2048);
	}
}

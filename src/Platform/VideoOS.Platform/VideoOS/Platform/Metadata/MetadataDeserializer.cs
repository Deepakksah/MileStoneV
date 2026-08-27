using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.Metadata;

public class MetadataDeserializer
{
	private static readonly XmlReaderSettings Settings = CreateXmlReaderSettings();

	public MetadataStream ParseMetadataXml(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		StringReader stringReader = null;
		try
		{
			stringReader = new StringReader(xml);
			using XmlReader reader = XmlReader.Create(stringReader, Settings);
			stringReader = null;
			return ParseXml(reader);
		}
		finally
		{
			stringReader?.Dispose();
		}
	}

	public MetadataStream ParseMetadataXml(Stream dataStream)
	{
		if (dataStream == null)
		{
			throw new ArgumentNullException("dataStream");
		}
		using XmlReader reader = XmlReader.Create(dataStream, Settings);
		return ParseXml(reader);
	}

	public MetadataStream ParseMetadataXml(byte[] metadataContent)
	{
		if (metadataContent == null)
		{
			throw new ArgumentNullException("metadataContent");
		}
		MemoryStream memoryStream = null;
		StreamReader streamReader = null;
		try
		{
			memoryStream = new MemoryStream(metadataContent);
			streamReader = new StreamReader(memoryStream, Encoding.UTF8);
			memoryStream = null;
			using XmlReader reader = XmlReader.Create(streamReader, Settings);
			streamReader = null;
			return ParseXml(reader);
		}
		finally
		{
			memoryStream?.Dispose();
			streamReader?.Dispose();
		}
	}

	private static MetadataStream ParseXml(XmlReader reader)
	{
		do
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				return HandleXmlElement(reader);
			}
		}
		while (reader.Read());
		throw new XmlException("Unrecognized input");
	}

	private static MetadataStream HandleXmlElement(XmlReader reader)
	{
		string localName = reader.LocalName;
		if ((object)MetadataXml.MetadataStreamElement == localName)
		{
			if (!reader.NamespaceURI.Equals(MetadataXml.OnvifNamespace))
			{
				string message = "Incorrect namespace in top-level element: " + reader.NamespaceURI;
				IXmlLineInfo xmlLineInfo = (IXmlLineInfo)reader;
				throw new XmlException(message, null, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
			}
			try
			{
				MetadataStream metadataStream = new MetadataStream();
				metadataStream.ReadXml(reader.ReadSubtree());
				return metadataStream;
			}
			catch (Exception ex)
			{
				throw new XmlException("Error while deserializing XML: " + ex.Message, ex);
			}
		}
		string message2 = "Incorrect top-level element: " + localName;
		IXmlLineInfo xmlLineInfo2 = (IXmlLineInfo)reader;
		throw new XmlException(message2, null, xmlLineInfo2.LineNumber, xmlLineInfo2.LinePosition);
	}

	private static XmlReaderSettings CreateXmlReaderSettings()
	{
		return new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreProcessingInstructions = true,
			IgnoreWhitespace = true,
			NameTable = MetadataXml.NameTable
		};
	}
}

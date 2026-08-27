using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.Metadata;

public class MetadataSerializer
{
	public static readonly Encoding MetadataEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	private static readonly XmlWriterSettings Settings = CreateWriterSettings();

	public string WriteMetadataXml(MetadataStream metadata)
	{
		if (metadata == null)
		{
			throw new ArgumentNullException("metadata");
		}
		using MemoryStream memoryStream = new MemoryStream();
		using StreamReader streamReader = new StreamReader(memoryStream, MetadataEncoding);
		using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, Settings))
		{
			WriteXml(xmlWriter, metadata);
		}
		memoryStream.Position = 0L;
		return streamReader.ReadToEnd();
	}

	public void WriteMetadataXml(Stream output, MetadataStream metadata)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (metadata == null)
		{
			throw new ArgumentNullException("metadata");
		}
		using XmlWriter xmlWriter = XmlWriter.Create(output, Settings);
		WriteXml(xmlWriter, metadata);
	}

	private static void WriteXml(XmlWriter xmlWriter, MetadataStream metadata)
	{
		xmlWriter.WriteStartDocument();
		xmlWriter.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.MetadataStreamElement, MetadataXml.OnvifNamespace);
		metadata.WriteXml(xmlWriter);
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndDocument();
	}

	private static XmlWriterSettings CreateWriterSettings()
	{
		return new XmlWriterSettings
		{
			CloseOutput = false,
			ConformanceLevel = ConformanceLevel.Document,
			Encoding = MetadataEncoding,
			Indent = false,
			OmitXmlDeclaration = false
		};
	}
}

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Common.Integration;

public sealed class ArchiveData
{
	public string Path { get; set; }

	public string Folder { get; set; }

	public ScheduleData Schedule { get; set; }

	public CleanupData Cleanup { get; set; }

	public ArchiveData()
	{
		Schedule = new ScheduleData();
		Cleanup = new CleanupData();
	}

	private void LoadData(string archiveDefinition)
	{
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		using XmlReader xmlReader = XmlReader.Create(new StringReader(archiveDefinition), xmlReaderSettings);
		bool isEmptyElement = xmlReader.IsEmptyElement;
		xmlReader.Read();
		if (xmlReader.NodeType == XmlNodeType.XmlDeclaration)
		{
			xmlReader.Read();
		}
		if (isEmptyElement)
		{
			return;
		}
		while (xmlReader.NodeType == XmlNodeType.Element)
		{
			string name = xmlReader.Name;
			if (name != null)
			{
				if (name.Equals("path", StringComparison.OrdinalIgnoreCase))
				{
					Path = Helper.StringFromXml(xmlReader);
					continue;
				}
				if (name.Equals("folder", StringComparison.OrdinalIgnoreCase))
				{
					Folder = Helper.StringFromXml(xmlReader);
					continue;
				}
				if (name.Equals("schedule", StringComparison.OrdinalIgnoreCase))
				{
					Schedule = ScheduleData.FromXml(xmlReader);
					continue;
				}
				if (name.Equals("cleanup", StringComparison.OrdinalIgnoreCase))
				{
					Cleanup = CleanupData.FromXml(xmlReader);
					continue;
				}
			}
			xmlReader.Read();
		}
		xmlReader.ReadEndElement();
	}

	public static ArchiveData FromDefinition(string definition)
	{
		ArchiveData archiveData = new ArchiveData();
		archiveData.LoadData(definition);
		return archiveData;
	}

	public void ToXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.WriteStartElement("archive");
		writer.WriteElementString("path", Path);
		writer.WriteElementString("folder", Folder);
		Schedule.ToXml(writer, "schedule");
		Cleanup.ToXml(writer, "cleanup");
		writer.WriteEndElement();
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			OmitXmlDeclaration = true,
			Encoding = Encoding.Unicode
		});
		ToXml(xmlWriter);
		xmlWriter.Close();
		return stringBuilder.ToString();
	}
}

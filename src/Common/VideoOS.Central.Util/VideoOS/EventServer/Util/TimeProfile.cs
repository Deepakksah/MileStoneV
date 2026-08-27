using System;
using System.Xml;

namespace VideoOS.EventServer.Util;

public class TimeProfile
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Definition { get; set; }

	public TimeProfile(Guid id, string name, string description, string definition)
	{
		Id = id;
		Name = name;
		Description = description;
		Definition = definition;
	}

	public void WriteXml(XmlWriter xmlWriter)
	{
		if (xmlWriter == null)
		{
			throw new ArgumentNullException("xmlWriter");
		}
		xmlWriter.WriteStartElement("timeprofile");
		xmlWriter.WriteElementString("id", Id.ToString());
		xmlWriter.WriteElementString("name", Name);
		xmlWriter.WriteElementString("description", Description);
		xmlWriter.WriteElementString("definition", Definition);
		xmlWriter.WriteEndElement();
	}
}

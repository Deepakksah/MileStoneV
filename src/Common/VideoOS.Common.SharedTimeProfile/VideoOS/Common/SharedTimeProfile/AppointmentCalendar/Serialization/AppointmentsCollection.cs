using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class AppointmentsCollection : IXmlSerializable
{
	public ICollection<string> AppointmentRefs { get; } = new List<string>();

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		int num = reader.ReadElementContentAsInt("Count", "");
		for (int i = 0; i < num; i++)
		{
			AppointmentRefs.Add(reader.GetAttribute("href"));
			reader.Read();
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteElementString("Count", $"{AppointmentRefs.Count}");
		int num = 30;
		foreach (string appointmentRef in AppointmentRefs)
		{
			writer.WriteStartElement($"_x{num++:0000}_");
			writer.WriteAttributeString("href", appointmentRef);
			writer.WriteEndElement();
		}
	}
}

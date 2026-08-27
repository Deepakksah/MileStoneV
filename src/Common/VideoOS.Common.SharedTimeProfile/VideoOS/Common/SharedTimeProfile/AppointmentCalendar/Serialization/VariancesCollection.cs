using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class VariancesCollection : IXmlSerializable
{
	public string OwningRecurrenceRef { get; set; }

	public ICollection<string> AppointmentVarianceRefs { get; } = new List<string>();

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		if (reader.LocalName == "OwningRecurrence")
		{
			OwningRecurrenceRef = reader.GetAttribute("href");
			reader.Read();
		}
		int num = reader.ReadElementContentAsInt("Count", "");
		for (int i = 0; i < num; i++)
		{
			AppointmentVarianceRefs.Add(reader.GetAttribute("href"));
			reader.Read();
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("OwningRecurrence");
		writer.WriteAttributeString("href", OwningRecurrenceRef);
		writer.WriteEndElement();
		writer.WriteElementString("Count", $"{AppointmentVarianceRefs.Count}");
		int num = 30;
		foreach (string appointmentVarianceRef in AppointmentVarianceRefs)
		{
			writer.WriteStartElement($"_x{num++:0000}_");
			writer.WriteAttributeString("href", appointmentVarianceRef);
			writer.WriteEndElement();
		}
	}
}

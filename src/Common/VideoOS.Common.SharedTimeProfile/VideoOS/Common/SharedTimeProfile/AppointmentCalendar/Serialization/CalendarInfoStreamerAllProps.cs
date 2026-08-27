using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class CalendarInfoStreamerAllProps : IXmlSerializable
{
	public bool AllowRecurringAppointments { get; set; }

	public string AppointmentsRef { get; set; }

	public int? WeekRule { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		while (reader.NodeType == XmlNodeType.Element)
		{
			switch (reader.LocalName)
			{
			case "Appointments":
				AppointmentsRef = reader.GetAttribute("href");
				reader.Read();
				break;
			case "AllowRecurringAppointments":
				AllowRecurringAppointments = reader.ReadElementContentAsBoolean();
				break;
			case "WeekRule":
				WeekRule = reader.ReadElementContentAsInt();
				break;
			default:
				throw new InvalidOperationException("Unexpected element " + reader.Name);
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		if (AppointmentsRef != null)
		{
			writer.WriteStartElement("Appointments");
			writer.WriteAttributeString("href", AppointmentsRef);
			writer.WriteEndElement();
		}
		writer.WriteElementString("AllowRecurringAppointments", AllowRecurringAppointments ? "true" : "false");
		if (WeekRule.HasValue)
		{
			writer.WriteElementString("WeekRule", $"{WeekRule.Value}");
		}
	}
}

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class Appointment : IXmlSerializable
{
	public bool AllDayEvent { get; set; }

	public string Subject { get; set; }

	public string SubjectId { get; set; }

	public string SubjectRef { get; set; }

	public DateTime StartDateTime { get; set; }

	public DateTime EndDateTime { get; set; }

	public string RecurrenceRef { get; set; }

	public Guid? RecurrenceId { get; set; }

	public DateTime? OriginalStartDateTime { get; set; }

	public int DirtyProps { get; set; }

	public int OriginalIndex { get; set; }

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
			case "AllDayEvent":
				AllDayEvent = reader.ReadElementContentAsBoolean();
				break;
			case "Subject":
				if (reader.IsEmptyElement)
				{
					SubjectRef = reader.GetAttribute("href");
					reader.Read();
				}
				else
				{
					SubjectId = reader.GetAttribute("id");
					Subject = reader.ReadElementContentAsString();
				}
				break;
			case "StartDateTime":
				StartDateTime = reader.ReadElementContentAsDateTime();
				break;
			case "EndDateTime":
				EndDateTime = reader.ReadElementContentAsDateTime();
				break;
			case "Recurrence":
				RecurrenceRef = reader.GetAttribute("href");
				reader.Read();
				break;
			case "RecurrenceId":
			{
				GuidWrapper guidWrapper = new GuidWrapper();
				reader.ReadStartElement();
				guidWrapper.ReadXml(reader);
				reader.ReadEndElement();
				RecurrenceId = guidWrapper.Value;
				break;
			}
			case "OriginalStartDateTime":
				OriginalStartDateTime = reader.ReadElementContentAsDateTime();
				break;
			case "DirtyProps":
				DirtyProps = reader.ReadElementContentAsInt();
				break;
			case "OriginalIndex":
				OriginalIndex = reader.ReadElementContentAsInt();
				break;
			default:
				throw new InvalidOperationException("Unexpected element " + reader.Name);
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		if (AllDayEvent)
		{
			writer.WriteElementString("AllDayEvent", "true");
		}
		writer.WriteStartElement("Subject");
		if (SubjectId != null)
		{
			writer.WriteAttributeString("id", SubjectId);
			writer.WriteString(Subject);
			writer.WriteEndElement();
		}
		else if (SubjectRef != null)
		{
			writer.WriteAttributeString("href", SubjectRef);
			writer.WriteEndElement();
		}
		writer.WriteStartElement("StartDateTime");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:dateTime");
		writer.WriteString(StartDateTime.ToString("o"));
		writer.WriteEndElement();
		writer.WriteStartElement("EndDateTime");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:dateTime");
		writer.WriteString(EndDateTime.ToString("o"));
		writer.WriteEndElement();
		if (RecurrenceRef != null)
		{
			writer.WriteStartElement("Recurrence");
			writer.WriteAttributeString("href", RecurrenceRef);
			writer.WriteEndElement();
		}
		if (RecurrenceId.HasValue)
		{
			writer.WriteStartElement("RecurrenceId");
			writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "a2:Guid");
			writer.WriteAttributeString("xmlns", "a2", null, "http://schemas.microsoft.com/clr/ns/System");
			GuidWrapper guidWrapper = new GuidWrapper();
			guidWrapper.Value = RecurrenceId.Value;
			guidWrapper.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (OriginalStartDateTime.HasValue)
		{
			writer.WriteStartElement("OriginalStartDateTime");
			writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:dateTime");
			writer.WriteString(OriginalStartDateTime.Value.ToString("o"));
			writer.WriteEndElement();
		}
		writer.WriteElementString("DirtyProps", $"{DirtyProps}");
		writer.WriteElementString("OriginalIndex", $"{OriginalIndex}");
	}
}

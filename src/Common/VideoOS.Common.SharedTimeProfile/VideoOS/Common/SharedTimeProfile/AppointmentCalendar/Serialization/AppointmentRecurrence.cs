using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class AppointmentRecurrence : IXmlSerializable
{
	public Guid Id { get; set; }

	public TimeSpan OccurrenceDuration { get; set; }

	public DateTime OccurrenceStartTime { get; set; }

	public int? RangeLimit { get; set; }

	public int? RangeMaxOccurrences { get; set; }

	public string VariancesRef { get; set; }

	public int? PatternDayOfMonth { get; set; }

	public int? PatternDaysOfWeek { get; set; }

	public int? PatternFrequency { get; set; }

	public int? PatternInterval { get; set; }

	public int? PatternMonthOfYear { get; set; }

	public int? PatternOccurrenceOfDayInMonth { get; set; }

	public int? PatternType { get; set; }

	public DateTime RangeEndDate { get; set; }

	public DateTime RangeStartDate { get; set; }

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
			case "Id":
			{
				reader.ReadStartElement();
				GuidWrapper guidWrapper = new GuidWrapper();
				guidWrapper.ReadXml(reader);
				reader.ReadEndElement();
				Id = guidWrapper.Value;
				break;
			}
			case "OccurrenceDuration":
				OccurrenceDuration = XmlConvert.ToTimeSpan(reader.ReadElementContentAsString());
				break;
			case "OccurrenceStartTime":
				OccurrenceStartTime = reader.ReadElementContentAsDateTime();
				break;
			case "RangeLimit":
				RangeLimit = reader.ReadElementContentAsInt();
				break;
			case "RangeMaxOccurrences":
				RangeMaxOccurrences = reader.ReadElementContentAsInt();
				break;
			case "Variances":
				VariancesRef = reader.GetAttribute("href");
				reader.Read();
				break;
			case "PatternDayOfMonth":
				PatternDayOfMonth = reader.ReadElementContentAsInt();
				break;
			case "PatternDaysOfWeek":
				PatternDaysOfWeek = reader.ReadElementContentAsInt();
				break;
			case "PatternFrequency":
				PatternFrequency = reader.ReadElementContentAsInt();
				break;
			case "PatternInterval":
				PatternInterval = reader.ReadElementContentAsInt();
				break;
			case "PatternMonthOfYear":
				PatternMonthOfYear = reader.ReadElementContentAsInt();
				break;
			case "PatternOccurrenceOfDayInMonth":
				PatternOccurrenceOfDayInMonth = reader.ReadElementContentAsInt();
				break;
			case "PatternType":
				PatternType = reader.ReadElementContentAsInt();
				break;
			case "RangeEndDate":
				RangeEndDate = reader.ReadElementContentAsDateTime();
				break;
			case "RangeStartDate":
				RangeStartDate = reader.ReadElementContentAsDateTime();
				break;
			default:
				throw new InvalidOperationException("Unexpected element " + reader.Name);
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("Id");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "a2:Guid");
		writer.WriteAttributeString("xmlns", "a2", null, "http://schemas.microsoft.com/clr/ns/System");
		GuidWrapper guidWrapper = new GuidWrapper();
		guidWrapper.Value = Id;
		guidWrapper.WriteXml(writer);
		writer.WriteEndElement();
		writer.WriteStartElement("OccurrenceDuration");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:duration");
		writer.WriteString($"P0Y0M{OccurrenceDuration.Days}DT{OccurrenceDuration.Hours}H{OccurrenceDuration.Minutes}M{OccurrenceDuration.Seconds}S");
		writer.WriteEndElement();
		writer.WriteStartElement("OccurrenceStartTime");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:dateTime");
		writer.WriteString(OccurrenceStartTime.ToString("o"));
		writer.WriteEndElement();
		if (RangeLimit.HasValue)
		{
			writer.WriteElementString("RangeLimit", $"{RangeLimit.Value}");
		}
		if (RangeMaxOccurrences.HasValue)
		{
			writer.WriteElementString("RangeMaxOccurrences", $"{RangeMaxOccurrences.Value}");
		}
		if (VariancesRef != null)
		{
			writer.WriteStartElement("Variances");
			writer.WriteAttributeString("href", VariancesRef);
			writer.WriteEndElement();
		}
		if (PatternDayOfMonth.HasValue)
		{
			writer.WriteElementString("PatternDayOfMonth", $"{PatternDayOfMonth.Value}");
		}
		if (PatternDaysOfWeek.HasValue)
		{
			writer.WriteElementString("PatternDaysOfWeek", $"{PatternDaysOfWeek.Value}");
		}
		if (PatternFrequency.HasValue)
		{
			writer.WriteElementString("PatternFrequency", $"{PatternFrequency.Value}");
		}
		if (PatternInterval.HasValue)
		{
			writer.WriteElementString("PatternInterval", $"{PatternInterval.Value}");
		}
		if (PatternMonthOfYear.HasValue)
		{
			writer.WriteElementString("PatternMonthOfYear", $"{PatternMonthOfYear.Value}");
		}
		if (PatternOccurrenceOfDayInMonth.HasValue)
		{
			writer.WriteElementString("PatternOccurrenceOfDayInMonth", $"{PatternOccurrenceOfDayInMonth.Value}");
		}
		if (PatternType.HasValue)
		{
			writer.WriteElementString("PatternType", $"{PatternType.Value}");
		}
		writer.WriteStartElement("RangeEndDate");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:dateTime");
		writer.WriteString(RangeEndDate.ToString("o"));
		writer.WriteEndElement();
		writer.WriteStartElement("RangeStartDate");
		writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:dateTime");
		writer.WriteString(RangeStartDate.ToString("o"));
		writer.WriteEndElement();
	}
}

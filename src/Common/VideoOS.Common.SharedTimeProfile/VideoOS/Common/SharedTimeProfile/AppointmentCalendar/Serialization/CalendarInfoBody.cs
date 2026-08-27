using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class CalendarInfoBody : IXmlSerializable
{
	private const string _prefix = "a1";

	private const string _defaultNamespace = "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2";

	public KeyValuePair<string, CalendarInfoStreamerAllProps> CalendarInfoStreamerAllProps { get; set; }

	public KeyValuePair<string, AppointmentsCollection> AppointmentsCollection { get; set; }

	public IDictionary<string, Appointment> Appointments { get; } = new Dictionary<string, Appointment>();

	public IDictionary<string, AppointmentRecurrence> AppointmentRecurrences { get; } = new Dictionary<string, AppointmentRecurrence>();

	public IDictionary<string, VariancesCollection> VariancesCollections { get; } = new Dictionary<string, VariancesCollection>();

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		while (reader.NodeType == XmlNodeType.Element)
		{
			string attribute = reader.GetAttribute("id");
			string prefix = reader.Prefix;
			string attribute2 = reader.GetAttribute("xmlns:" + prefix);
			switch (reader.LocalName)
			{
			case "CalendarInfoStreamerAllProps":
				reader.ReadStartElement("CalendarInfoStreamerAllProps", attribute2);
				CalendarInfoStreamerAllProps = new KeyValuePair<string, CalendarInfoStreamerAllProps>(attribute, new CalendarInfoStreamerAllProps());
				CalendarInfoStreamerAllProps.Value.ReadXml(reader);
				reader.ReadEndElement();
				break;
			case "AppointmentsCollection":
				reader.ReadStartElement("AppointmentsCollection", attribute2);
				AppointmentsCollection = new KeyValuePair<string, AppointmentsCollection>(attribute, new AppointmentsCollection());
				AppointmentsCollection.Value.ReadXml(reader);
				reader.ReadEndElement();
				break;
			case "Appointment":
				reader.ReadStartElement("Appointment", attribute2);
				Appointments[attribute] = new Appointment();
				Appointments[attribute].ReadXml(reader);
				reader.ReadEndElement();
				break;
			case "AppointmentRecurrence":
				reader.ReadStartElement("AppointmentRecurrence", attribute2);
				AppointmentRecurrences[attribute] = new AppointmentRecurrence();
				AppointmentRecurrences[attribute].ReadXml(reader);
				reader.ReadEndElement();
				break;
			case "VariancesCollection":
				reader.ReadStartElement("VariancesCollection", attribute2);
				VariancesCollections[attribute] = new VariancesCollection();
				VariancesCollections[attribute].ReadXml(reader);
				reader.ReadEndElement();
				break;
			default:
				throw new InvalidOperationException("Unexpected element " + reader.Name);
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("a1", "CalendarInfoStreamerAllProps", "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2");
		writer.WriteAttributeString("id", CalendarInfoStreamerAllProps.Key);
		CalendarInfoStreamerAllProps.Value.WriteXml(writer);
		writer.WriteEndElement();
		if (AppointmentsCollection.Value != null)
		{
			writer.WriteStartElement("a1", "AppointmentsCollection", "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2");
			writer.WriteAttributeString("id", AppointmentsCollection.Key);
			AppointmentsCollection.Value.WriteXml(writer);
			writer.WriteEndElement();
		}
		foreach (KeyValuePair<string, Appointment> appointment in Appointments)
		{
			writer.WriteStartElement("a1", "Appointment", "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2");
			writer.WriteAttributeString("id", appointment.Key);
			appointment.Value.WriteXml(writer);
			writer.WriteEndElement();
		}
		foreach (KeyValuePair<string, AppointmentRecurrence> appointmentRecurrence in AppointmentRecurrences)
		{
			writer.WriteStartElement("a1", "AppointmentRecurrence", "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2");
			writer.WriteAttributeString("id", appointmentRecurrence.Key);
			appointmentRecurrence.Value.WriteXml(writer);
			writer.WriteEndElement();
		}
		foreach (KeyValuePair<string, VariancesCollection> variancesCollection in VariancesCollections)
		{
			writer.WriteStartElement("a1", "VariancesCollection", "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2");
			writer.WriteAttributeString("id", variancesCollection.Key);
			variancesCollection.Value.WriteXml(writer);
			writer.WriteEndElement();
		}
	}
}

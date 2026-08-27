using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

public class GuidWrapper : IXmlSerializable
{
	public Guid Value { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		int a = reader.ReadElementContentAsInt("_a", "");
		short b = (short)reader.ReadElementContentAsInt("_b", "");
		short c = (short)reader.ReadElementContentAsInt("_c", "");
		byte d = (byte)reader.ReadElementContentAsInt("_d", "");
		byte e = (byte)reader.ReadElementContentAsInt("_e", "");
		byte f = (byte)reader.ReadElementContentAsInt("_f", "");
		byte g = (byte)reader.ReadElementContentAsInt("_g", "");
		byte h = (byte)reader.ReadElementContentAsInt("_h", "");
		byte i = (byte)reader.ReadElementContentAsInt("_i", "");
		byte j = (byte)reader.ReadElementContentAsInt("_j", "");
		byte k = (byte)reader.ReadElementContentAsInt("_k", "");
		Value = new Guid(a, b, c, d, e, f, g, h, i, j, k);
	}

	public void WriteXml(XmlWriter writer)
	{
		byte[] array = Value.ToByteArray();
		writer.WriteElementString("_a", $"{(((((array[3] << 8) | array[2]) << 8) | array[1]) << 8) | array[0]}");
		writer.WriteElementString("_b", $"{(short)((array[5] << 8) | array[4])}");
		writer.WriteElementString("_c", $"{(short)((array[7] << 8) | array[6])}");
		writer.WriteElementString("_d", $"{array[8]}");
		writer.WriteElementString("_e", $"{array[9]}");
		writer.WriteElementString("_f", $"{array[10]}");
		writer.WriteElementString("_g", $"{array[11]}");
		writer.WriteElementString("_h", $"{array[12]}");
		writer.WriteElementString("_i", $"{array[13]}");
		writer.WriteElementString("_j", $"{array[14]}");
		writer.WriteElementString("_k", $"{array[15]}");
	}
}

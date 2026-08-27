using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Server;

public sealed class LogRetention
{
	private const string LogRetentionXmlRootName = "retentiontime";

	public long MaxNumberOfLogEntries { get; set; }

	public long DaysToKeepLogEntries { get; set; }

	public SystemLogRetentionData SystemLogRetention { get; set; }

	public AuditLogRetentionData AuditLogRetention { get; set; }

	public RuleLogRetentionData RuleLogRetention { get; set; }

	public LogRetention()
	{
		MaxNumberOfLogEntries = 50000L;
		DaysToKeepLogEntries = 30L;
		SystemLogRetention = new SystemLogRetentionData
		{
			MaxNumberOfLogEntries = MaxNumberOfLogEntries,
			DaysToKeepLogEntries = DaysToKeepLogEntries
		};
		AuditLogRetention = new AuditLogRetentionData
		{
			MaxNumberOfLogEntries = MaxNumberOfLogEntries,
			DaysToKeepLogEntries = DaysToKeepLogEntries
		};
		RuleLogRetention = new RuleLogRetentionData
		{
			MaxNumberOfLogEntries = MaxNumberOfLogEntries,
			DaysToKeepLogEntries = DaysToKeepLogEntries
		};
	}

	private static void WriteMaxageAsXml(XmlWriter wr, long MaxNumberOfLogEntries, long DaysToKeepLogEntries)
	{
		wr.WriteStartElement("maxage");
		wr.WriteAttributeString("days", DaysToKeepLogEntries.ToString(CultureInfo.InvariantCulture));
		wr.WriteAttributeString("count", MaxNumberOfLogEntries.ToString(CultureInfo.InvariantCulture));
		wr.WriteEndElement();
	}

	private static void WriteGroupAsXml(XmlWriter wr, string name, long MaxNumberOfLogEntries, long DaysToKeepLogEntries)
	{
		wr.WriteStartElement("group");
		wr.WriteAttributeString("name", name);
		WriteMaxageAsXml(wr, MaxNumberOfLogEntries, DaysToKeepLogEntries);
		wr.WriteEndElement();
	}

	public string GetLogRetentionAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		using (XmlWriter xmlWriter = XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), xmlWriterSettings))
		{
			xmlWriter.WriteStartElement("retentiontime");
			WriteGroupAsXml(xmlWriter, "system", SystemLogRetention.MaxNumberOfLogEntries, SystemLogRetention.DaysToKeepLogEntries);
			WriteGroupAsXml(xmlWriter, "audit", AuditLogRetention.MaxNumberOfLogEntries, AuditLogRetention.DaysToKeepLogEntries);
			WriteGroupAsXml(xmlWriter, "rule", RuleLogRetention.MaxNumberOfLogEntries, RuleLogRetention.DaysToKeepLogEntries);
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}
		return stringBuilder.ToString();
	}
}

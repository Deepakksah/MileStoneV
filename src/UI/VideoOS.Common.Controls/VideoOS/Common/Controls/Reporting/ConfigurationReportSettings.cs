using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.Controls.Reporting;

public class ConfigurationReportSettings
{
	public FrontPageSettings FrontPageSettings { get; set; }

	public PageSettings PageSettings { get; set; }

	[XmlArrayItem("Report")]
	public Collection<string> SelectedReports { get; set; }

	public static ConfigurationReportSettings Load(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationReportSettings));
				using XmlTextReader xmlReader = new XmlTextReader(path);
				return (ConfigurationReportSettings)xmlSerializer.Deserialize(xmlReader);
			}
		}
		catch (InvalidOperationException)
		{
		}
		return new ConfigurationReportSettings();
	}

	public void Save(string path)
	{
		try
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationReportSettings));
			using (XmlTextWriter xmlWriter = new XmlTextWriter(path, Encoding.UTF8)
			{
				Formatting = Formatting.Indented
			})
			{
				xmlSerializer.Serialize(xmlWriter, this);
			}
			FileSecurity accessControl = File.GetAccessControl(path);
			accessControl.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
			File.SetAccessControl(path, accessControl);
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (InvalidOperationException)
		{
		}
	}
}

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;

namespace VideoOS.Platform.SDK.Config;

public class SpeakerInFile : Speaker, IDeviceInFile
{
	public string DatabasePath { get; set; }

	public string ArchivePath { get; set; }

	public string CameraPath { get; set; }

	public bool IsEncrypted { get; set; }

	public string HostName => _systemInfo.ServerId.ServerHostname;

	public override string ConnectionXml => BuildConfigurationXml(_systemInfo.Token).OuterXml;

	public SpeakerInFile(ISystemInfo systemInfo, IRecorder recorder, Guid guid, string name, string dbExportName, bool isEncrypted)
		: base(systemInfo, recorder, guid, name, dbExportName)
	{
		IsEncrypted = isEncrypted;
		_speakerSecurity = new SpeakerSecurity(new SpeakerSecurityInfo
		{
			Browse = true,
			ExportDatabase = true,
			GetSequences = true
		});
	}

	public override XmlDocument ConfigurationXml()
	{
		throw new NotImplementedException();
	}

	public override XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		if (_systemInfo.ServerId.ServerType == "BLK")
		{
			xmlTextWriter.WriteStartElement("toolkit");
			xmlTextWriter.WriteAttributeString("type", "source");
			xmlTextWriter.WriteElementString("provider", "mpms");
			xmlTextWriter.WriteStartElement("config");
			xmlTextWriter.WriteStartElement("table");
			xmlTextWriter.WriteAttributeString("include_archives", "yes");
			xmlTextWriter.WriteElementString("name", string.IsNullOrEmpty(base.DBExportName) ? base.Guid.ToString() : base.DBExportName);
			xmlTextWriter.WriteStartElement("bank");
			if (string.IsNullOrEmpty(DatabasePath))
			{
				xmlTextWriter.WriteElementString("path", _systemInfo.ServerId.ServerHostname);
			}
			else
			{
				xmlTextWriter.WriteElementString("path", DatabasePath);
			}
			if (IsEncrypted)
			{
				xmlTextWriter.WriteStartElement("authorization");
				xmlTextWriter.WriteAttributeString("method", "anonymous");
				xmlTextWriter.WriteElementString("password", new NetworkCredential(string.Empty, _systemInfo.Password).Password);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument;
	}

	private XmlDocument BuildConfigurationXml(string securityToken)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("audiodevice");
		xmlTextWriter.WriteAttributeString("guid", base.Guid.ToString());
		xmlTextWriter.WriteString(GetToolkitConfigurationXml(base.Guid).InnerXml);
		xmlTextWriter.WriteElementString("guid", base.Guid.ToString());
		xmlTextWriter.WriteElementString("hostname", HostName);
		xmlTextWriter.WriteElementString("port", "80");
		string value = $"id={base.Guid.ToString()}&connectiontoken={securityToken}";
		xmlTextWriter.WriteElementString("connectparam", value);
		xmlTextWriter.WriteStartElement("userrights");
		xmlTextWriter.WriteElementString("aviexport", "yes");
		xmlTextWriter.WriteElementString("jpegexport", "yes");
		xmlTextWriter.WriteElementString("live", "no");
		xmlTextWriter.WriteElementString("browse", "yes");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument;
	}
}

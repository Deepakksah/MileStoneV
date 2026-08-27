using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class MicrophoneInFile : IMicrophone, IStreamDevice, IDevice, IDeviceInFile
{
	private readonly ISystemInfo _systemInfo;

	private MicrophoneSecurity _microphoneSecurity;

	public string DatabasePath { get; set; }

	public string ArchivePath { get; set; }

	public string CameraPath { get; set; }

	public bool Disabled { get; set; }

	public bool IsEncrypted { get; set; }

	public PositioningInformation PositioningInformation => null;

	public ISystemInfo SystemInfo => _systemInfo;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public IRecorder Recorder { get; set; }

	public string DBExportName { get; private set; }

	public ITrackInfo[] Tracks => null;

	public MicrophoneSecurity MicrophoneAccess => _microphoneSecurity;

	public string HostName => _systemInfo.ServerId.ServerHostname;

	public string ConnectionXml => BuildConfigurationXml(_systemInfo.Token).OuterXml;

	public MicrophoneInFile(ISystemInfo systemInfo, IRecorder recorder, Guid guid, string name, string dbExportName, bool isEncrypted)
	{
		_systemInfo = systemInfo;
		Recorder = recorder;
		Guid = guid;
		Name = name;
		DBExportName = dbExportName;
		IsEncrypted = isEncrypted;
		_microphoneSecurity = new MicrophoneSecurity(new MicrophoneSecurityInfo
		{
			Browse = true,
			ExportDatabase = true,
			GetSequences = true
		});
	}

	public XmlDocument ConfigurationXml()
	{
		throw new NotImplementedException();
	}

	public XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "mpms");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteStartElement("table");
		xmlTextWriter.WriteAttributeString("include_archives", "yes");
		xmlTextWriter.WriteElementString("name", string.IsNullOrEmpty(DBExportName) ? Guid.ToString() : DBExportName);
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
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument;
	}

	public SequenceType[] GetSequenceTypes()
	{
		return null;
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid sequenceType)
	{
		return new SequenceCollection();
	}

	private XmlDocument BuildConfigurationXml(string securityToken)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("audiodevice");
		xmlTextWriter.WriteAttributeString("guid", Guid.ToString());
		xmlTextWriter.WriteString(GetToolkitConfigurationXml(Guid).InnerXml);
		xmlTextWriter.WriteElementString("guid", Guid.ToString());
		xmlTextWriter.WriteElementString("hostname", HostName);
		xmlTextWriter.WriteElementString("port", "80");
		string value = $"id={Guid.ToString()}&connectiontoken={securityToken}";
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

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class Speaker : ISpeaker, IStreamDevice, IDevice
{
	protected ISystemInfo _systemInfo;

	protected ISpeakerInfo _speakerInfo;

	private readonly PositioningInformation _posInfo;

	protected SpeakerSecurity _speakerSecurity;

	public ISystemInfo SystemInfo => _systemInfo;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public bool Disabled { get; set; }

	public IRecorder Recorder { get; set; }

	public string DBExportName { get; private set; }

	public PositioningInformation PositioningInformation => _posInfo;

	public virtual string ConnectionXml
	{
		get
		{
			if (_speakerInfo != null)
			{
				return BuildSpeakerConfigurationXml(_systemInfo.Token).OuterXml;
			}
			return null;
		}
	}

	public SpeakerSecurity SpeakerAccess
	{
		get
		{
			if (_speakerSecurity != null)
			{
				return _speakerSecurity;
			}
			_speakerSecurity = new SpeakerSecurity(_speakerInfo.SpeakerSecurity);
			return _speakerSecurity;
		}
	}

	public ITrackInfo[] Tracks
	{
		get
		{
			if (_speakerInfo == null)
			{
				return null;
			}
			return _speakerInfo.Tracks;
		}
	}

	public Speaker(ISystemInfo systemInfo, IRecorder recorder, ISpeakerInfo speakerInfo)
	{
		_systemInfo = systemInfo;
		_speakerInfo = speakerInfo;
		Recorder = recorder;
		Guid = speakerInfo.DeviceId;
		Name = speakerInfo.Name;
		DBExportName = null;
		_posInfo = new PositioningInformation(speakerInfo.GisPoint, speakerInfo.CoverageDirection, speakerInfo.CoverageDepth, speakerInfo.CoverageFieldOfView);
	}

	protected Speaker(ISystemInfo systemInfo, IRecorder recorder, Guid guid, string name, string dbExportName)
	{
		_systemInfo = systemInfo;
		Recorder = recorder;
		Guid = guid;
		Name = name;
		DBExportName = dbExportName;
	}

	public SequenceType[] GetSequenceTypes()
	{
		return Recorder.GetSequenceTypes(Guid);
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid sequenceType)
	{
		return Recorder.GetSequenceAtTimeMinutes(Guid, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceType);
	}

	public virtual XmlDocument ConfigurationXml()
	{
		return BuildSpeakerConfigurationXml(_systemInfo.Token);
	}

	public virtual XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "is");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteElementString("media_type", "Audio");
		xmlTextWriter.WriteElementString("server_uri", Recorder.ServerId.Uri.AbsoluteUri);
		xmlTextWriter.WriteElementString("device_id", Guid.ToString());
		xmlTextWriter.WriteStartElement("authorization");
		xmlTextWriter.WriteAttributeString("method", "token");
		xmlTextWriter.WriteStartElement("token");
		xmlTextWriter.WriteAttributeString("update_key", "istoken");
		xmlTextWriter.WriteString(_systemInfo.Token);
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

	private XmlDocument BuildSpeakerConfigurationXml(string securityToken)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("audiodevice");
		xmlTextWriter.WriteElementString("guid", Guid.ToString());
		xmlTextWriter.WriteElementString("displayname", Name);
		xmlTextWriter.WriteElementString("serveruri", Recorder.ServerId.Uri.AbsoluteUri);
		string value = $"id={Guid.ToString()}&connectiontoken={HttpUtility.UrlEncode(securityToken)}";
		xmlTextWriter.WriteElementString("connectparam", value);
		xmlTextWriter.WriteStartElement("userrights");
		if (_speakerInfo != null && _speakerInfo.SpeakerSecurity != null)
		{
			xmlTextWriter.WriteElementString("live", _speakerInfo.SpeakerSecurity.Live ? "yes" : "no");
			xmlTextWriter.WriteElementString("browse", _speakerInfo.SpeakerSecurity.Browse ? "yes" : "no");
		}
		else
		{
			xmlTextWriter.WriteElementString("live", "yes");
			xmlTextWriter.WriteElementString("browse", "yes");
		}
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

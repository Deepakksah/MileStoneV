using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class Microphone : IMicrophone, IStreamDevice, IDevice
{
	private ISystemInfo _systemInfo;

	private IMicrophoneInfo _microphoneInfo;

	private IRecorderInfo _recorderInfo;

	private PositioningInformation _posInfo;

	private MicrophoneSecurity _micSecurity;

	public ISystemInfo SystemInfo => _systemInfo;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public bool Disabled { get; set; }

	public IRecorder Recorder { get; set; }

	public string DBExportName => null;

	public PositioningInformation PositioningInformation => _posInfo;

	public string HostName => Recorder.ServerId.ServerHostname;

	public string ServerUri => Recorder.ServerId.Uri.AbsoluteUri;

	public string ConnectionXml
	{
		get
		{
			if (_microphoneInfo != null)
			{
				return BuildMicrophoneConfigurationXml(_systemInfo.Token).OuterXml;
			}
			return null;
		}
	}

	public MicrophoneSecurity MicrophoneAccess
	{
		get
		{
			if (_micSecurity != null)
			{
				return _micSecurity;
			}
			_micSecurity = new MicrophoneSecurity(_microphoneInfo.MicrophoneSecurity);
			return _micSecurity;
		}
	}

	public ITrackInfo[] Tracks
	{
		get
		{
			if (_microphoneInfo == null)
			{
				return null;
			}
			return _microphoneInfo.Tracks;
		}
	}

	public Microphone(ISystemInfo systemInfo, IRecorder recorder, IMicrophoneInfo microphoneInfo, IRecorderInfo recorderInfo)
	{
		_systemInfo = systemInfo;
		Recorder = recorder;
		_microphoneInfo = microphoneInfo;
		_recorderInfo = recorderInfo;
		Guid = microphoneInfo.DeviceId;
		Name = microphoneInfo.Name;
		_posInfo = new PositioningInformation(microphoneInfo.GisPoint, microphoneInfo.CoverageDirection, microphoneInfo.CoverageDepth, microphoneInfo.CoverageFieldOfView);
	}

	public XmlDocument ConfigurationXml()
	{
		return BuildMicrophoneConfigurationXml(_systemInfo.Token);
	}

	public SequenceType[] GetSequenceTypes()
	{
		return Recorder.GetSequenceTypes(Guid);
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid sequenceType)
	{
		return Recorder.GetSequenceAtTimeMinutes(Guid, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceType);
	}

	public XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "is");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteElementString("media_type", "Audio");
		xmlTextWriter.WriteElementString("server_uri", ServerUri);
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

	private XmlDocument BuildMicrophoneConfigurationXml(string securityToken)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("audiodevice");
		xmlTextWriter.WriteElementString("guid", Guid.ToString());
		xmlTextWriter.WriteElementString("displayname", Name);
		xmlTextWriter.WriteElementString("serveruri", ServerUri);
		string value = $"id={Guid.ToString()}&connectiontoken={HttpUtility.UrlEncode(securityToken)}";
		xmlTextWriter.WriteElementString("connectparam", value);
		xmlTextWriter.WriteStartElement("userrights");
		if (_microphoneInfo != null && _microphoneInfo.MicrophoneSecurity != null)
		{
			xmlTextWriter.WriteElementString("live", _microphoneInfo.MicrophoneSecurity.Live ? "yes" : "no");
			xmlTextWriter.WriteElementString("browse", _microphoneInfo.MicrophoneSecurity.Browse ? "yes" : "no");
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

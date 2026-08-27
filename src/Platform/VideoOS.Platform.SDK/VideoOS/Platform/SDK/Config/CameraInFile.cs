using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config;

public class CameraInFile : ICamera, IStreamDevice, IDevice, IDeviceInFile
{
	private readonly ISystemInfo _systemInfo;

	private readonly CameraSecurity _cameraSecurity;

	private readonly string _dbExportName;

	public string DatabasePath { get; set; }

	public string ArchivePath { get; set; }

	public string CameraPath { get; set; }

	public bool IsEncrypted { get; set; }

	public string DBExportName => _dbExportName;

	public ISystemInfo SystemInfo => _systemInfo;

	public string Id => Guid.ToString();

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public IRecorder Recorder { get; set; }

	public bool Disabled { get; set; }

	public PositioningInformation PositioningInformation => null;

	public UserDefinedEventCollection Events => new UserDefinedEventCollection();

	public Ptz PtzSettings => new Ptz(new PtzInfo());

	public string HostName => _systemInfo.ServerId.ServerHostname;

	public int Channel => 0;

	public string Shortcut => "";

	public IMicrophone Microphone
	{
		get
		{
			if (_systemInfo.ConfigurationInfo == null || _systemInfo.ConfigurationInfo.DeviceMappings == null || _systemInfo.Microphones == null)
			{
				return null;
			}
			return (from dmi in _systemInfo.ConfigurationInfo.DeviceMappings
				where dmi != null
				where dmi.DeviceA == Guid
				where _systemInfo.Microphones.ContainsKey(dmi.DeviceB)
				select _systemInfo.Microphones[dmi.DeviceB])?.FirstOrDefault();
		}
		set
		{
		}
	}

	public Speaker Speaker
	{
		get
		{
			if (_systemInfo.ConfigurationInfo == null || _systemInfo.ConfigurationInfo.DeviceMappings == null || _systemInfo.Speakers == null)
			{
				return null;
			}
			return (from dmi in _systemInfo.ConfigurationInfo.DeviceMappings
				where dmi != null
				where dmi.DeviceA == Guid
				where _systemInfo.Speakers.ContainsKey(dmi.DeviceB)
				select _systemInfo.Speakers[dmi.DeviceB])?.FirstOrDefault();
		}
	}

	public IEnumerable<IMetadataDevice> RelatedMetadataDevices
	{
		get
		{
			if (_systemInfo.ConfigurationInfo == null || _systemInfo.ConfigurationInfo.DeviceMappings == null || _systemInfo.MetadataDevices == null)
			{
				return Enumerable.Empty<IMetadataDevice>();
			}
			return from dmi in _systemInfo.ConfigurationInfo.DeviceMappings
				where dmi != null
				where dmi.DeviceA == Guid
				where _systemInfo.MetadataDevices.ContainsKey(dmi.DeviceB)
				select _systemInfo.MetadataDevices[dmi.DeviceB];
		}
	}

	public OutputDeviceCollection Outputs => null;

	public PresetCollection Presets => new PresetCollection(this, _systemInfo);

	public CameraSecurity CameraAccess => _cameraSecurity;

	public bool MulticastEnabled => false;

	public bool Browsable => false;

	public bool IpixEnabled => false;

	public bool EdgeSupported => false;

	public PanoramicLens PanoramicLens => new PanoramicLens(new PanoramicLensInfo());

	public ITrackInfo[] Tracks => Array.Empty<ITrackInfo>();

	public CameraInFile(ISystemInfo systemInfo, IRecorder recorder, Guid guid, string name, string dbExportName, bool isEncrypted)
	{
		_systemInfo = systemInfo;
		Recorder = recorder;
		Guid = guid;
		Name = name;
		_dbExportName = dbExportName;
		IsEncrypted = isEncrypted;
		_cameraSecurity = new CameraSecurity(new CameraSecurityInfo
		{
			Browse = true,
			ExportAvi = true,
			ExportDatabase = true,
			GetSequences = true,
			ExportJpeg = true,
			PtzSecurity = new PtzSecurityInfo()
		});
	}

	public XmlDocument ConfigurationXml(Guid streamId)
	{
		return BuildCameraConfigurationXml(_systemInfo.Token);
	}

	public VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType[] GetSequenceTypes()
	{
		return null;
	}

	public StreamType[] GetStreamTypes()
	{
		return Array.Empty<StreamType>();
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime Time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		return new SequenceCollection();
	}

	public void MoveToPreset(string presetName)
	{
		throw new NotImplementedException();
	}

	public void PtzMoveRelative(PtzMoveRelativeTypes moveRelative)
	{
		throw new NotImplementedException();
	}

	public void PtzMoveRelative(PtzMoveRelativeTypes moveRelative, double speed, double step)
	{
		throw new NotImplementedException();
	}

	public void PtzMoveAbsolute(double pan, double tilt, double zoom)
	{
		throw new NotImplementedException();
	}

	public void PtzMoveAbsolute(PTZArgs ptzArgs)
	{
		throw new NotImplementedException();
	}

	public PTZArgs PtzGetAbsolutePosition()
	{
		throw new NotImplementedException();
	}

	public void PtzMoveHome()
	{
		throw new NotImplementedException();
	}

	public void PtzMoveStart(PTZArgs ptzArgs)
	{
		throw new NotImplementedException();
	}

	public void PtzAuxOn(int auxNumber)
	{
		throw new NotImplementedException();
	}

	public void PtzAuxOff(int auxNumber)
	{
		throw new NotImplementedException();
	}

	public void PtzMoveStop()
	{
		throw new NotImplementedException();
	}

	public void PtzCenter(double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		throw new NotImplementedException();
	}

	public void PtzRectangle(double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
	}

	public string GetIpixData()
	{
		return "";
	}

	public string GetPanoramicLensData()
	{
		return "";
	}

	public string GetPanoramicLensDataForXpco()
	{
		return "";
	}

	public XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		throw new NotImplementedException();
	}

	private XmlDocument BuildCameraConfigurationXml(string securityToken)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("camera");
		xmlTextWriter.WriteAttributeString("cameraid", Name);
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
		xmlTextWriter.WriteElementString("guid", Id);
		xmlTextWriter.WriteElementString("hostname", HostName);
		xmlTextWriter.WriteElementString("port", "80");
		string value = $"id={Id}&connectiontoken={securityToken}";
		xmlTextWriter.WriteElementString("connectparam", value);
		xmlTextWriter.WriteStartElement("ptz");
		xmlTextWriter.WriteElementString("pan", "none");
		xmlTextWriter.WriteElementString("tilt", "none");
		xmlTextWriter.WriteElementString("zoom", "none");
		xmlTextWriter.WriteElementString("ptzcenter", "no");
		xmlTextWriter.WriteElementString("ptzscalezoom", "no");
		xmlTextWriter.WriteElementString("ptzrectangle", "no");
		xmlTextWriter.WriteElementString("ptzjoystick", "no");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("presets");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("userrights");
		xmlTextWriter.WriteElementString("aviexport", "yes");
		xmlTextWriter.WriteElementString("jpegexport", "yes");
		xmlTextWriter.WriteElementString("live", "no");
		xmlTextWriter.WriteElementString("browse", "yes");
		xmlTextWriter.WriteElementString("ptz", "no");
		xmlTextWriter.WriteElementString("outputs", "no");
		xmlTextWriter.WriteElementString("presets", "no");
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

#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config;

public class CameraOnServer : ICamera, IStreamDevice, IDevice
{
	private ISystemInfo _systemInfo;

	private ICameraInfo _cameraInfo;

	private SequenceCollection _sequence;

	private CameraSecurity _cameraAccess;

	private Ptz _ptzSettings;

	private string _guidString = "";

	private int _channel;

	private TimeLineCache _timeLineCacheRecordings;

	private TimeLineCache _timeLineCacheMotions;

	private PresetCollection _presetCollection;

	private PositioningInformation _posInfo;

	public ISystemInfo SystemInfo => _systemInfo;

	public string Id => _guidString;

	public string Name { get; set; }

	public IRecorder Recorder { get; set; }

	public Guid Guid { get; set; }

	public bool Disabled { get; set; }

	public string DBExportName => null;

	public PositioningInformation PositioningInformation => _posInfo;

	public UserDefinedEventCollection Events
	{
		get
		{
			if (CameraAccess.Events)
			{
				return new UserDefinedEventCollection(_systemInfo);
			}
			return new UserDefinedEventCollection();
		}
	}

	public bool IpixEnabled => _cameraInfo.IpixSettings.IpixEnabled;

	public PanoramicLens PanoramicLens
	{
		get
		{
			if (_cameraInfo != null)
			{
				return new PanoramicLens(_cameraInfo.PanoramicLensSettings);
			}
			return new PanoramicLens(new PanoramicLensInfo());
		}
	}

	public bool PtzGetPositionEnabled => _cameraInfo.PtzSettings.PtzEnabled;

	private bool PtzEnabled => PtzSettings.PtzEnabled;

	private bool CenterOnPositionInViewSupported => PtzSettings.IsCenterOnPositionInViewSupported;

	public Ptz PtzSettings
	{
		get
		{
			if (_ptzSettings != null)
			{
				return _ptzSettings;
			}
			_ptzSettings = new Ptz(_cameraInfo.PtzSettings);
			return _ptzSettings;
		}
	}

	public string HostName => Recorder.ServerId.ServerHostname;

	public int Channel => _channel;

	public string Shortcut => _cameraInfo.Shortcut;

	public bool EdgeSupported
	{
		get
		{
			if (_cameraInfo != null)
			{
				return _cameraInfo.EdgeStorageSupported;
			}
			return false;
		}
	}

	public string ServerUri => Recorder.ServerId.Uri.AbsoluteUri;

	public IMicrophone Microphone
	{
		get
		{
			try
			{
				IDeviceMappingInfo[] deviceMappings = _systemInfo.ConfigurationInfo.DeviceMappings;
				foreach (IDeviceMappingInfo deviceMappingInfo in deviceMappings)
				{
					if (deviceMappingInfo.DeviceA == Guid && _systemInfo.Microphones.ContainsKey(deviceMappingInfo.DeviceB))
					{
						return _systemInfo.Microphones[deviceMappingInfo.DeviceB];
					}
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "SDK.Platform.Camera.Microphone", ex.Message);
			}
			return null;
		}
		set
		{
		}
	}

	public OutputDeviceCollection Outputs => null;

	public Speaker Speaker
	{
		get
		{
			try
			{
				IDeviceMappingInfo[] deviceMappings = _systemInfo.ConfigurationInfo.DeviceMappings;
				foreach (IDeviceMappingInfo deviceMappingInfo in deviceMappings)
				{
					if (deviceMappingInfo.DeviceA == Guid && _systemInfo.Speakers.ContainsKey(deviceMappingInfo.DeviceB))
					{
						return _systemInfo.Speakers[deviceMappingInfo.DeviceB];
					}
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "SDK.Platform.Camera.Speaker", ex.Message);
			}
			return null;
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

	public PresetCollection Presets => _presetCollection;

	public bool Browsable => PtzSettings.Browsable;

	public CameraSecurity CameraAccess
	{
		get
		{
			if (_cameraAccess == null)
			{
				_cameraAccess = new CameraSecurity(_cameraInfo.CameraSecurity);
			}
			return _cameraAccess;
		}
	}

	public bool MulticastEnabled
	{
		get
		{
			if (_cameraInfo != null)
			{
				return _cameraInfo.MulticastEnabled;
			}
			return false;
		}
	}

	public ITrackInfo[] Tracks
	{
		get
		{
			if (_cameraInfo == null)
			{
				return Array.Empty<ITrackInfo>();
			}
			return _cameraInfo.Tracks;
		}
	}

	public CameraOnServer(ISystemInfo systemInfo, IRecorder recorder, ICameraInfo cameraInfo)
	{
		_systemInfo = systemInfo;
		_cameraInfo = cameraInfo;
		Recorder = recorder;
		Guid = _cameraInfo.DeviceId;
		_guidString = Guid.ToString();
		Name = _cameraInfo.Name;
		_channel = cameraInfo.DeviceIndex;
		_presetCollection = new PresetCollection(this, _systemInfo, _cameraInfo);
		_posInfo = new PositioningInformation(_cameraInfo.GisPoint, _cameraInfo.CoverageDirection, _cameraInfo.CoverageDepth, _cameraInfo.CoverageFieldOfView);
	}

	public XmlDocument ConfigurationXml(Guid streamId)
	{
		return BuildCameraConfigurationXml(_systemInfo.Token, streamId);
	}

	public string GetIpixData()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("IpixData");
		WriteIpixInnerXml(xmlTextWriter);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument.OuterXml;
	}

	public string GetPanoramicLensData()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("PanoramicLensData");
		WritePanoramicLensInnerXml(xmlTextWriter, PanoramicLens);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument.OuterXml;
	}

	public string GetPanoramicLensDataForXpco()
	{
		if (PanoramicLens != null && PanoramicLens.PanoramicLensEnabled)
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xmlTextWriter.WriteStartDocument(standalone: true);
			WritePanoramicLensInformation(xmlTextWriter);
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			memoryStream.Position = 0L;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(memoryStream);
			return xmlDocument.OuterXml;
		}
		return "";
	}

	public XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		throw new NotImplementedException();
	}

	private XmlDocument BuildCameraConfigurationXml(string securityToken, Guid streamId)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("camera");
		xmlTextWriter.WriteAttributeString("cameraid", Name);
		xmlTextWriter.WriteElementString("guid", Id);
		xmlTextWriter.WriteElementString("serveruri", ServerUri);
		IStreamInfo streamInfo = null;
		if (_cameraInfo != null && streamId != Guid.Empty)
		{
			IStreamInfo[] streams = _cameraInfo.Streams;
			foreach (IStreamInfo streamInfo2 in streams)
			{
				if (streamInfo2.StreamId == streamId)
				{
					streamInfo = streamInfo2;
					break;
				}
			}
		}
		string value = ((streamInfo == null) ? $"id={Id}&connectiontoken={Uri.EscapeDataString(securityToken)}" : $"id={Id}&streamid={streamInfo.StreamId}&connectiontoken={Uri.EscapeDataString(securityToken)}");
		xmlTextWriter.WriteElementString("connectparam", value);
		if (_cameraInfo != null)
		{
			xmlTextWriter.WriteElementString("maxfps", _cameraInfo.MaxFPS.ToString());
		}
		xmlTextWriter.WriteStartElement("transcode");
		xmlTextWriter.WriteElementString("allframes", "yes");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("ipix");
		if (IpixEnabled && _cameraInfo != null)
		{
			xmlTextWriter.WriteElementString("enabled", "yes");
			WriteIpixInnerXml(xmlTextWriter);
		}
		else
		{
			xmlTextWriter.WriteElementString("enabled", "no");
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("panoramiclens");
		PanoramicLens panoramicLens = PanoramicLens;
		if (panoramicLens.PanoramicLensEnabled)
		{
			xmlTextWriter.WriteElementString("enabled", "yes");
			WritePanoramicLensInnerXml(xmlTextWriter, panoramicLens);
		}
		else
		{
			xmlTextWriter.WriteElementString("enabled", "no");
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("ptz");
		if (PtzEnabled)
		{
			if (IpixEnabled)
			{
				xmlTextWriter.WriteElementString("pan", "ipix");
				xmlTextWriter.WriteElementString("tilt", "ipix");
				xmlTextWriter.WriteElementString("zoom", "ipix");
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				Capability capability = PtzSettings.GetCapability("pan");
				if (capability != null)
				{
					if (capability.Relative)
					{
						xmlTextWriter.WriteElementString("pan", "relative");
					}
					else if (capability.Absolute)
					{
						xmlTextWriter.WriteElementString("pan", "absolute");
					}
					flag = capability.Relative;
				}
				capability = PtzSettings.GetCapability("tilt");
				if (capability != null)
				{
					if (capability.Relative)
					{
						xmlTextWriter.WriteElementString("tilt", "relative");
					}
					else if (capability.Absolute)
					{
						xmlTextWriter.WriteElementString("tilt", "absolute");
					}
					flag2 = capability.Relative;
				}
				capability = PtzSettings.GetCapability("zoom");
				if (capability != null)
				{
					if (capability.Relative)
					{
						xmlTextWriter.WriteElementString("zoom", "relative");
					}
					else if (capability.Absolute)
					{
						xmlTextWriter.WriteElementString("zoom", "absolute");
					}
				}
				if (_cameraInfo != null)
				{
					string value2 = (_cameraInfo.PtzSettings.IsCenterOnPositionInViewSupported ? "yes" : "no");
					xmlTextWriter.WriteElementString("ptzcenter", value2);
					if (_cameraInfo.PtzSettings.IsCenterOnPositionInViewSupported)
					{
						capability = PtzSettings.GetCapability("zoom");
						string value3 = ((capability != null && capability.Absolute) ? "yes" : "no");
						xmlTextWriter.WriteElementString("ptzscalezoom", value3);
					}
					string value4 = (_cameraInfo.PtzSettings.IsPtzCenterAndZoomToRectangleSupported ? "yes" : "no");
					xmlTextWriter.WriteElementString("ptzrectangle", value4);
				}
				if (flag && flag2)
				{
					xmlTextWriter.WriteElementString("ptzvirtualjoystick", "yes");
				}
			}
			xmlTextWriter.WriteElementString("ptzjoystick", "no");
		}
		else
		{
			xmlTextWriter.WriteElementString("pan", "none");
			xmlTextWriter.WriteElementString("tilt", "none");
			xmlTextWriter.WriteElementString("zoom", "none");
			xmlTextWriter.WriteElementString("ptzcenter", "no");
			xmlTextWriter.WriteElementString("ptzscalezoom", "no");
			xmlTextWriter.WriteElementString("ptzrectangle", "no");
			xmlTextWriter.WriteElementString("ptzjoystick", "no");
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("presets");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("userrights");
		xmlTextWriter.WriteElementString("aviexport", (_cameraInfo == null || _cameraInfo.CameraSecurity.ExportAvi) ? "yes" : "no");
		xmlTextWriter.WriteElementString("jpegexport", (_cameraInfo == null || _cameraInfo.CameraSecurity.ExportJpeg) ? "yes" : "no");
		xmlTextWriter.WriteElementString("live", (_cameraInfo == null || _cameraInfo.CameraSecurity.Live) ? "yes" : "no");
		xmlTextWriter.WriteElementString("browse", (_cameraInfo == null || _cameraInfo.CameraSecurity.Browse) ? "yes" : "no");
		xmlTextWriter.WriteElementString("ptz", (_cameraInfo == null || _cameraInfo.CameraSecurity.PtzSecurity.ManualControl) ? "yes" : "no");
		xmlTextWriter.WriteElementString("outputs", "yes");
		xmlTextWriter.WriteElementString("presets", (_cameraInfo == null || _cameraInfo.CameraSecurity.PtzSecurity.PresetControl) ? "yes" : "no");
		xmlTextWriter.WriteEndElement();
		if (Microphone != null || Speaker != null)
		{
			xmlTextWriter.WriteStartElement("devicemappings");
			IMicrophone microphone = Microphone;
			if (microphone != null)
			{
				xmlTextWriter.WriteStartElement("devicemapping");
				xmlTextWriter.WriteElementString("device1guid", Guid.ToString().ToUpper());
				xmlTextWriter.WriteElementString("device2guid", microphone.Guid.ToString().ToUpper());
				xmlTextWriter.WriteEndElement();
			}
			Speaker speaker = Speaker;
			if (speaker != null)
			{
				xmlTextWriter.WriteStartElement("devicemapping");
				xmlTextWriter.WriteElementString("device1guid", Guid.ToString().ToUpper());
				xmlTextWriter.WriteElementString("device2guid", speaker.Guid.ToString().ToUpper());
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument;
	}

	private void WriteIpixInnerXml(XmlTextWriter xmlTextWriter)
	{
		if (_cameraInfo != null)
		{
			IIpixInfo ipixSettings = _cameraInfo.IpixSettings;
			xmlTextWriter.WriteStartElement("hemisphere");
			xmlTextWriter.WriteElementString("radiusx", Convert.ToString(ipixSettings.Hemisphere.RadiusX, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteElementString("radiusy", Convert.ToString(ipixSettings.Hemisphere.RadiusY, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteElementString("centerx", Convert.ToString(ipixSettings.Hemisphere.CenterX, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteElementString("centery", Convert.ToString(ipixSettings.Hemisphere.CenterY, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteEndElement();
			if (ipixSettings.CeilingMounted)
			{
				xmlTextWriter.WriteElementString("ceilingmount", "yes");
			}
			else
			{
				xmlTextWriter.WriteElementString("ceilingmount", "no");
			}
			xmlTextWriter.WriteStartElement("homeposistion");
			xmlTextWriter.WriteElementString("pan", Convert.ToString(ipixSettings.Homeposition.Pan, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteElementString("tilt", Convert.ToString(ipixSettings.Homeposition.Tilt, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteElementString("zoom", Convert.ToString(ipixSettings.Homeposition.Zoom, NumberFormatInfo.InvariantInfo));
			xmlTextWriter.WriteEndElement();
		}
	}

	private static void WritePanoramicLensInnerXml(XmlTextWriter xmlTextWriter, PanoramicLens panoramicLens)
	{
		PanoramicLensType panoramicLensType = panoramicLens.PanoramicLensType;
		if ((uint)panoramicLensType <= 1u)
		{
			xmlTextWriter.WriteElementString("type", "immervision");
		}
		switch (panoramicLens.CameraMount)
		{
		case CameraMountType.Wall:
			xmlTextWriter.WriteElementString("cameramount", "wall");
			break;
		case CameraMountType.Ground:
			xmlTextWriter.WriteElementString("cameramount", "ground");
			break;
		case CameraMountType.Ceiling:
			xmlTextWriter.WriteElementString("cameramount", "ceiling");
			break;
		default:
			xmlTextWriter.WriteElementString("cameramount", "ceiling");
			break;
		}
		xmlTextWriter.WriteStartElement("immervision");
		xmlTextWriter.WriteStartElement("lensprofile");
		xmlTextWriter.WriteElementString("rpl", panoramicLens.ImmerVision.LensProfileRpl);
		xmlTextWriter.WriteElementString("name", panoramicLens.ImmerVision.LensProfileName);
		xmlTextWriter.WriteElementString("data", ImmerVision.LensProfileData);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
	}

	private void WritePanoramicLensInformation(XmlTextWriter writer)
	{
		if (PanoramicLens == null || !PanoramicLens.PanoramicLensEnabled)
		{
			return;
		}
		writer.WriteStartElement("entry");
		writer.WriteAttributeString("key", "panoramicLensInformation");
		writer.WriteStartElement("PanoramicLensSettings");
		if (PanoramicLens.PanoramicLensEnabled)
		{
			writer.WriteStartElement("PanoramicLensEnabled");
			writer.WriteValue(PanoramicLens.PanoramicLensEnabled);
			writer.WriteEndElement();
			writer.WriteStartElement("PanoramicLensType");
			writer.WriteValue(PanoramicLens.PanoramicLensType.ToString());
			writer.WriteEndElement();
			writer.WriteStartElement("CameraMount");
			writer.WriteValue(PanoramicLens.CameraMount.ToString());
			writer.WriteEndElement();
			if (PanoramicLens.ImmerVision != null)
			{
				writer.WriteStartElement("ImmerVision");
				writer.WriteStartElement("LensProfileName");
				writer.WriteValue(PanoramicLens.ImmerVision.LensProfileName);
				writer.WriteEndElement();
				writer.WriteStartElement("LensProfileData");
				writer.WriteValue(ImmerVision.LensProfileData);
				writer.WriteEndElement();
				writer.WriteStartElement("LensProfileRpl");
				writer.WriteValue(PanoramicLens.ImmerVision.LensProfileRpl);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	public override string ToString()
	{
		return Name;
	}

	public VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType[] GetSequenceTypes()
	{
		return Recorder.GetSequenceTypes(Guid);
	}

	public StreamType[] GetStreamTypes()
	{
		if (_cameraInfo != null && _cameraInfo.Streams != null)
		{
			StreamType[] array = new StreamType[_cameraInfo.Streams.Length];
			for (int i = 0; i < _cameraInfo.Streams.Length; i++)
			{
				IStreamInfo streamInfo = _cameraInfo.Streams[i];
				array[i] = new StreamType
				{
					Id = streamInfo.StreamId,
					Name = streamInfo.Name,
					Properties = new Dictionary<string, string> { 
					{
						"Default",
						streamInfo.Default ? "Yes" : "No"
					} }
				};
			}
			return array;
		}
		return Array.Empty<StreamType>();
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		try
		{
			_sequence = new SequenceCollection();
			if (seqType == DataType.SequenceTypeGuids.TimelineRecording || seqType == DataType.SequenceTypeGuids.TimelineMotionDetected)
			{
				time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0, time.Kind);
				if (seqType == DataType.SequenceTypeGuids.TimelineRecording)
				{
					if (_timeLineCacheRecordings == null)
					{
						_timeLineCacheRecordings = new TimeLineCache(Recorder, Guid, DataType.TimeLineInformationTypeGuids.RecordedDataAvailable);
					}
					_sequence = _timeLineCacheRecordings.GetSequenceAtTimeMinutes(time, minutesBefore, maxCountBefore, minutesAfter, maxCountAfter);
				}
				else if (seqType == DataType.SequenceTypeGuids.TimelineMotionDetected)
				{
					if (_timeLineCacheMotions == null)
					{
						_timeLineCacheMotions = new TimeLineCache(Recorder, Guid, DataType.TimeLineInformationTypeGuids.ActivitiesDetected);
					}
					_sequence = _timeLineCacheMotions.GetSequenceAtTimeMinutes(time, minutesBefore, maxCountBefore, minutesAfter, maxCountAfter);
				}
			}
			else
			{
				_sequence = Recorder.GetSequenceAtTimeMinutes(Guid, time, minutesBefore, maxCountBefore, minutesAfter, maxCountAfter, seqType);
				if (_sequence == null)
				{
					return null;
				}
				if (_sequence.Count > maxCountBefore + maxCountAfter)
				{
					_sequence.Remove(_sequence.Count - 1);
				}
			}
			return _sequence;
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.GetType()?.ToString() + ": " + ex.Message + "\n" + ex.StackTrace);
		}
		return null;
	}

	public void MoveToPreset(string presetName)
	{
		Recorder.MoveToPreset(Id, presetName, throwOnError: false);
	}

	public void PtzMoveRelative(PtzMoveRelativeTypes moveRelative)
	{
		Recorder.CameraPtzMoveAction(this, moveRelative);
	}

	public void PtzMoveRelative(PtzMoveRelativeTypes moveRelative, double speed, double step)
	{
		Recorder.CameraPtzMoveAction(this, moveRelative, speed, step);
	}

	public void PtzMoveAbsolute(double pan, double tilt, double zoom)
	{
		Recorder.CameraPtzMoveActionAbsolute(this, pan, tilt, zoom);
	}

	public void PtzMoveAbsolute(PTZArgs ptzArgs)
	{
		Recorder.CameraPtzMoveActionAbsolute(this, ptzArgs, throwOnError: false);
	}

	public PTZArgs PtzGetAbsolutePosition()
	{
		if (PtzGetPositionEnabled)
		{
			return Recorder.PtzGetAbsolutePosition(this, throwOnError: false);
		}
		return null;
	}

	public void PtzMoveHome()
	{
		if (PtzGetPositionEnabled)
		{
			Recorder.PtzMoveHome(this);
		}
	}

	public void PtzMoveStart(PTZArgs ptzArgs)
	{
		Recorder.CameraPtzMoveStart(this, ptzArgs, throwOnError: false);
	}

	public void PtzAuxOn(int auxNumber)
	{
		if (IpixEnabled)
		{
			Trace.WriteLine("Unable to aux for ipix camera!");
		}
		else if (auxNumber > 0 && auxNumber < 9)
		{
			Recorder.CameraPtzAuxOn(this, auxNumber, throwOnError: false);
		}
		else
		{
			Trace.WriteLine("Unknown aux command: Aux" + auxNumber);
		}
	}

	public void PtzAuxOff(int auxNumber)
	{
		if (IpixEnabled)
		{
			Trace.WriteLine("Unable to aux for ipix camera!");
		}
		else if (auxNumber > 0 && auxNumber <= 8)
		{
			Recorder.CameraPtzAuxOff(this, auxNumber, throwOnError: false);
		}
		else
		{
			Trace.WriteLine("Unknown aux command: Aux" + auxNumber);
		}
	}

	public void PtzMoveStop()
	{
		Recorder.CameraPtzMoveStop(this);
	}

	public void PtzCenter(double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		if (CenterOnPositionInViewSupported)
		{
			Recorder.CameraPtzCenter(this, refWidth, refHeight, centerX, centerY, zoom);
		}
	}

	public void PtzRectangle(double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
		Recorder.CameraPtzRectangle(this, refWidth, refHeight, left, top, bottom, right);
	}
}

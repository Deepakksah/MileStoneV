using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class CameraInfo : DeviceInfo, ICameraInfo, IMediaDeviceInfo, IDeviceInfo
{
	private double maxFPSField;

	private bool browsableStreamField;

	private StreamInfo[] streamsField;

	private TrackInfo[] tracksField;

	private CameraSecurityInfo cameraSecurityField;

	private PtzInfo ptzSettingsField;

	private IpixInfo ipixSettingsField;

	private PanoramicLensInfo panoramicLensSettingsField;

	private bool multicastEnabledField;

	private bool edgeStorageSupportedField;

	private bool edgeStoragePlaybackField;

	private int stopManualRecordingSecondsField;

	ITrackInfo[] IMediaDeviceInfo.Tracks
	{
		get
		{
			return Tracks;
		}
		set
		{
			Tracks = (TrackInfo[])value;
		}
	}

	IStreamInfo[] ICameraInfo.Streams
	{
		get
		{
			return Streams;
		}
		set
		{
			Streams = (StreamInfo[])value;
		}
	}

	ICameraSecurityInfo ICameraInfo.CameraSecurity
	{
		get
		{
			return CameraSecurity;
		}
		set
		{
			CameraSecurity = (CameraSecurityInfo)value;
		}
	}

	IPtzInfo ICameraInfo.PtzSettings
	{
		get
		{
			return PtzSettings;
		}
		set
		{
			PtzSettings = (PtzInfo)value;
		}
	}

	IIpixInfo ICameraInfo.IpixSettings
	{
		get
		{
			return IpixSettings;
		}
		set
		{
			IpixSettings = (IpixInfo)value;
		}
	}

	IPanoramicLensInfo ICameraInfo.PanoramicLensSettings
	{
		get
		{
			return PanoramicLensSettings;
		}
		set
		{
			PanoramicLensSettings = (PanoramicLensInfo)value;
		}
	}

	public double MaxFPS
	{
		get
		{
			return maxFPSField;
		}
		set
		{
			maxFPSField = value;
		}
	}

	public bool BrowsableStream
	{
		get
		{
			return browsableStreamField;
		}
		set
		{
			browsableStreamField = value;
		}
	}

	public StreamInfo[] Streams
	{
		get
		{
			return streamsField;
		}
		set
		{
			streamsField = value;
		}
	}

	public TrackInfo[] Tracks
	{
		get
		{
			return tracksField;
		}
		set
		{
			tracksField = value;
		}
	}

	public CameraSecurityInfo CameraSecurity
	{
		get
		{
			return cameraSecurityField;
		}
		set
		{
			cameraSecurityField = value;
		}
	}

	public PtzInfo PtzSettings
	{
		get
		{
			return ptzSettingsField;
		}
		set
		{
			ptzSettingsField = value;
		}
	}

	public IpixInfo IpixSettings
	{
		get
		{
			return ipixSettingsField;
		}
		set
		{
			ipixSettingsField = value;
		}
	}

	public PanoramicLensInfo PanoramicLensSettings
	{
		get
		{
			return panoramicLensSettingsField;
		}
		set
		{
			panoramicLensSettingsField = value;
		}
	}

	public bool MulticastEnabled
	{
		get
		{
			return multicastEnabledField;
		}
		set
		{
			multicastEnabledField = value;
		}
	}

	public bool EdgeStorageSupported
	{
		get
		{
			return edgeStorageSupportedField;
		}
		set
		{
			edgeStorageSupportedField = value;
		}
	}

	public bool EdgeStoragePlayback
	{
		get
		{
			return edgeStoragePlaybackField;
		}
		set
		{
			edgeStoragePlaybackField = value;
		}
	}

	public int StopManualRecordingSeconds
	{
		get
		{
			return stopManualRecordingSecondsField;
		}
		set
		{
			stopManualRecordingSecondsField = value;
		}
	}
}

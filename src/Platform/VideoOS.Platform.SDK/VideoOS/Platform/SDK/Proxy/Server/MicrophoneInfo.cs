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
public class MicrophoneInfo : DeviceInfo, IMicrophoneInfo, IMediaDeviceInfo, IDeviceInfo
{
	private TrackInfo[] tracksField;

	private MicrophoneSecurityInfo microphoneSecurityField;

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

	IMicrophoneSecurityInfo IMicrophoneInfo.MicrophoneSecurity
	{
		get
		{
			return MicrophoneSecurity;
		}
		set
		{
			MicrophoneSecurity = (MicrophoneSecurityInfo)value;
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

	public MicrophoneSecurityInfo MicrophoneSecurity
	{
		get
		{
			return microphoneSecurityField;
		}
		set
		{
			microphoneSecurityField = value;
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

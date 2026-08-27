using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[XmlInclude(typeof(MetadataStreamStatistics))]
[XmlInclude(typeof(AudioStreamStatistics))]
[XmlInclude(typeof(VideoStreamStatistics))]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class MediaStreamStatisticsBase
{
	private Guid streamIdField;

	private string nameField;

	private bool recordingStreamField;

	private bool liveStreamField;

	private bool liveStreamDefaultField;

	private bool liveStreamRunAlwaysField;

	private ulong bPSField;

	private double fPSField;

	public Guid StreamId
	{
		get
		{
			return streamIdField;
		}
		set
		{
			streamIdField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public bool RecordingStream
	{
		get
		{
			return recordingStreamField;
		}
		set
		{
			recordingStreamField = value;
		}
	}

	public bool LiveStream
	{
		get
		{
			return liveStreamField;
		}
		set
		{
			liveStreamField = value;
		}
	}

	public bool LiveStreamDefault
	{
		get
		{
			return liveStreamDefaultField;
		}
		set
		{
			liveStreamDefaultField = value;
		}
	}

	public bool LiveStreamRunAlways
	{
		get
		{
			return liveStreamRunAlwaysField;
		}
		set
		{
			liveStreamRunAlwaysField = value;
		}
	}

	public ulong BPS
	{
		get
		{
			return bPSField;
		}
		set
		{
			bPSField = value;
		}
	}

	public double FPS
	{
		get
		{
			return fPSField;
		}
		set
		{
			fPSField = value;
		}
	}
}

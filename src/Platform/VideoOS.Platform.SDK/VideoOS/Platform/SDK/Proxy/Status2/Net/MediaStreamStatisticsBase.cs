using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MediaStreamStatisticsBase", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
[KnownType(typeof(VideoStreamStatistics))]
[KnownType(typeof(MetadataStreamStatistics))]
[KnownType(typeof(AudioStreamStatistics))]
public class MediaStreamStatisticsBase : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid StreamIdField;

	private string NameField;

	private bool RecordingStreamField;

	private bool LiveStreamField;

	private bool LiveStreamDefaultField;

	private bool LiveStreamRunAlwaysField;

	private ulong BPSField;

	private double FPSField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public Guid StreamId
	{
		get
		{
			return StreamIdField;
		}
		set
		{
			StreamIdField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 1)]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 2)]
	public bool RecordingStream
	{
		get
		{
			return RecordingStreamField;
		}
		set
		{
			RecordingStreamField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public bool LiveStream
	{
		get
		{
			return LiveStreamField;
		}
		set
		{
			LiveStreamField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 4)]
	public bool LiveStreamDefault
	{
		get
		{
			return LiveStreamDefaultField;
		}
		set
		{
			LiveStreamDefaultField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 5)]
	public bool LiveStreamRunAlways
	{
		get
		{
			return LiveStreamRunAlwaysField;
		}
		set
		{
			LiveStreamRunAlwaysField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 6)]
	public ulong BPS
	{
		get
		{
			return BPSField;
		}
		set
		{
			BPSField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 7)]
	public double FPS
	{
		get
		{
			return FPSField;
		}
		set
		{
			FPSField = value;
		}
	}
}

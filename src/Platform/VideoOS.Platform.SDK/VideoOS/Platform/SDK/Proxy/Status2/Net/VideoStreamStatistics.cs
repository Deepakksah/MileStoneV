using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "VideoStreamStatistics", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class VideoStreamStatistics : MediaStreamStatisticsBase
{
	private Size ImageResolutionField;

	private string VideoFormatField;

	private ulong ImageSizeInBytesField;

	private double FPSRequestedField;

	[DataMember(IsRequired = true, EmitDefaultValue = false)]
	public Size ImageResolution
	{
		get
		{
			return ImageResolutionField;
		}
		set
		{
			ImageResolutionField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public string VideoFormat
	{
		get
		{
			return VideoFormatField;
		}
		set
		{
			VideoFormatField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 2)]
	public ulong ImageSizeInBytes
	{
		get
		{
			return ImageSizeInBytesField;
		}
		set
		{
			ImageSizeInBytesField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public double FPSRequested
	{
		get
		{
			return FPSRequestedField;
		}
		set
		{
			FPSRequestedField = value;
		}
	}
}

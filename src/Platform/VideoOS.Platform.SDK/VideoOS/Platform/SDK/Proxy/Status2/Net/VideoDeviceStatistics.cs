using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "VideoDeviceStatistics", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class VideoDeviceStatistics : DeviceStatisticsBase
{
	private VideoStreamStatistics[] VideoStreamStatisticsArrayField;

	[DataMember(EmitDefaultValue = false)]
	public VideoStreamStatistics[] VideoStreamStatisticsArray
	{
		get
		{
			return VideoStreamStatisticsArrayField;
		}
		set
		{
			VideoStreamStatisticsArrayField = value;
		}
	}
}

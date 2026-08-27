using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AudioDeviceStatistics", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class AudioDeviceStatistics : DeviceStatisticsBase
{
	private AudioStreamStatistics[] AudioStreamStatisticsArrayField;

	[DataMember(EmitDefaultValue = false)]
	public AudioStreamStatistics[] AudioStreamStatisticsArray
	{
		get
		{
			return AudioStreamStatisticsArrayField;
		}
		set
		{
			AudioStreamStatisticsArrayField = value;
		}
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IODeviceStatusBase", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
[KnownType(typeof(OutputDeviceStatus))]
[KnownType(typeof(InputDeviceStatus))]
public class IODeviceStatusBase : DeviceStatusBase
{
	private IOState StateField;

	[DataMember(IsRequired = true)]
	public IOState State
	{
		get
		{
			return StateField;
		}
		set
		{
			StateField = value;
		}
	}
}

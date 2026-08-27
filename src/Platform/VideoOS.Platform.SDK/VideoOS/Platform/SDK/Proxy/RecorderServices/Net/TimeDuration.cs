using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class TimeDuration
{
	private long microSecondsField;

	[XmlElement(Order = 0)]
	public long MicroSeconds
	{
		get
		{
			return microSecondsField;
		}
		set
		{
			microSecondsField = value;
		}
	}
}

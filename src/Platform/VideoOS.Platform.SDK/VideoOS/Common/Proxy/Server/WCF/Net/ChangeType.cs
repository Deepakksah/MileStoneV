using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ChangeType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ChangeType : IChangeType
{
	private bool HardwareField;

	private bool RecorderField;

	[DataMember]
	public bool Hardware
	{
		get
		{
			return HardwareField;
		}
		set
		{
			HardwareField = value;
		}
	}

	[DataMember]
	public bool Recorder
	{
		get
		{
			return RecorderField;
		}
		set
		{
			RecorderField = value;
		}
	}
}

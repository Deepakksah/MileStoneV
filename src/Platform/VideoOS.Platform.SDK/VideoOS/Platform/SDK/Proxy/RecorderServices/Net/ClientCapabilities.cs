using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class ClientCapabilities
{
	private bool privacyMaskField;

	private int privacyMaskVersionField;

	[XmlElement(Order = 0)]
	public bool PrivacyMask
	{
		get
		{
			return privacyMaskField;
		}
		set
		{
			privacyMaskField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int PrivacyMaskVersion
	{
		get
		{
			return privacyMaskVersionField;
		}
		set
		{
			privacyMaskVersionField = value;
		}
	}
}

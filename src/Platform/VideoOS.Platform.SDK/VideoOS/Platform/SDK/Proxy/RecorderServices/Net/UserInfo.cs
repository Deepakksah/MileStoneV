using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class UserInfo
{
	private string sidField;

	private string displayNameField;

	[XmlElement(Order = 0)]
	public string Sid
	{
		get
		{
			return sidField;
		}
		set
		{
			sidField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string DisplayName
	{
		get
		{
			return displayNameField;
		}
		set
		{
			displayNameField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class AttachAndConnectionState
{
	private string attachStateField;

	private string connectionStateField;

	public string AttachState
	{
		get
		{
			return attachStateField;
		}
		set
		{
			attachStateField = value;
		}
	}

	public string ConnectionState
	{
		get
		{
			return connectionStateField;
		}
		set
		{
			connectionStateField = value;
		}
	}
}

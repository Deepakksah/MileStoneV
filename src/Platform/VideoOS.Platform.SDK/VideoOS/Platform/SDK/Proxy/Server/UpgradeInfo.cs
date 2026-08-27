using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class UpgradeInfo : IUpgradeInfo
{
	private string hostnameField;

	private int portField;

	private string pathField;

	private string messageField;

	public string Hostname
	{
		get
		{
			return hostnameField;
		}
		set
		{
			hostnameField = value;
		}
	}

	public int Port
	{
		get
		{
			return portField;
		}
		set
		{
			portField = value;
		}
	}

	public string Path
	{
		get
		{
			return pathField;
		}
		set
		{
			pathField = value;
		}
	}

	public string Message
	{
		get
		{
			return messageField;
		}
		set
		{
			messageField = value;
		}
	}
}

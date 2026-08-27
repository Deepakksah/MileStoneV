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
public class StreamInfo : IStreamInfo
{
	private Guid streamIdField;

	private string nameField;

	private bool defaultField;

	public Guid StreamId
	{
		get
		{
			return streamIdField;
		}
		set
		{
			streamIdField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public bool Default
	{
		get
		{
			return defaultField;
		}
		set
		{
			defaultField = value;
		}
	}
}

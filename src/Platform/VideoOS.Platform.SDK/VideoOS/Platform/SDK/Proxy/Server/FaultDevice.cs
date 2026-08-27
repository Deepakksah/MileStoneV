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
public class FaultDevice : IFaultDevice
{
	private Guid deviceIdField;

	private int errorCodeField;

	private string messageField;

	private object sourceField;

	public Guid DeviceId
	{
		get
		{
			return deviceIdField;
		}
		set
		{
			deviceIdField = value;
		}
	}

	public int ErrorCode
	{
		get
		{
			return errorCodeField;
		}
		set
		{
			errorCodeField = value;
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

	public object Source
	{
		get
		{
			return sourceField;
		}
		set
		{
			sourceField = value;
		}
	}
}

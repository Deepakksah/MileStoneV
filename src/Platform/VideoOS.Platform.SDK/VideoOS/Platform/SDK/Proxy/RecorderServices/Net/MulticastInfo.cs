using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class MulticastInfo
{
	private Guid deviceIdField;

	private string iPAddressField;

	private int serverPortField;

	private int clientPortField;

	private TimeDuration keepAliveIntervalField;

	private bool encryptedField;

	private Guid multicastSessionIdField;

	private uint sSRCField;

	private string privacyMaskField;

	private int privacyMaskVersionField;

	private bool successField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string IPAddress
	{
		get
		{
			return iPAddressField;
		}
		set
		{
			iPAddressField = value;
		}
	}

	[XmlElement(Order = 2)]
	public int ServerPort
	{
		get
		{
			return serverPortField;
		}
		set
		{
			serverPortField = value;
		}
	}

	[XmlElement(Order = 3)]
	public int ClientPort
	{
		get
		{
			return clientPortField;
		}
		set
		{
			clientPortField = value;
		}
	}

	[XmlElement(Order = 4)]
	public TimeDuration KeepAliveInterval
	{
		get
		{
			return keepAliveIntervalField;
		}
		set
		{
			keepAliveIntervalField = value;
		}
	}

	[XmlElement(Order = 5)]
	public bool Encrypted
	{
		get
		{
			return encryptedField;
		}
		set
		{
			encryptedField = value;
		}
	}

	[XmlElement(Order = 6)]
	public Guid MulticastSessionId
	{
		get
		{
			return multicastSessionIdField;
		}
		set
		{
			multicastSessionIdField = value;
		}
	}

	[XmlElement(Order = 7)]
	public uint SSRC
	{
		get
		{
			return sSRCField;
		}
		set
		{
			sSRCField = value;
		}
	}

	[XmlElement(Order = 8)]
	public string PrivacyMask
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

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
	public bool Success
	{
		get
		{
			return successField;
		}
		set
		{
			successField = value;
		}
	}
}

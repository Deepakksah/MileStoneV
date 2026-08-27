using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
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

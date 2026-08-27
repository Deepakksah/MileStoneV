using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "UpgradeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UpgradeInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string HostnameField;

	private string MessageField;

	private string PathField;

	private int PortField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string Hostname
	{
		get
		{
			return HostnameField;
		}
		set
		{
			HostnameField = value;
		}
	}

	[DataMember]
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			MessageField = value;
		}
	}

	[DataMember]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}

	[DataMember]
	public int Port
	{
		get
		{
			return PortField;
		}
		set
		{
			PortField = value;
		}
	}
}

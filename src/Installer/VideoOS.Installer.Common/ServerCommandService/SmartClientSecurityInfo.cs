using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SmartClientSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SmartClientSecurityInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool SmartClientBrowseField;

	private bool SmartClientLiveField;

	private bool SmartClientReportField;

	private bool SmartClientSetupField;

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
	public bool SmartClientBrowse
	{
		get
		{
			return SmartClientBrowseField;
		}
		set
		{
			SmartClientBrowseField = value;
		}
	}

	[DataMember]
	public bool SmartClientLive
	{
		get
		{
			return SmartClientLiveField;
		}
		set
		{
			SmartClientLiveField = value;
		}
	}

	[DataMember]
	public bool SmartClientReport
	{
		get
		{
			return SmartClientReportField;
		}
		set
		{
			SmartClientReportField = value;
		}
	}

	[DataMember]
	public bool SmartClientSetup
	{
		get
		{
			return SmartClientSetupField;
		}
		set
		{
			SmartClientSetupField = value;
		}
	}
}

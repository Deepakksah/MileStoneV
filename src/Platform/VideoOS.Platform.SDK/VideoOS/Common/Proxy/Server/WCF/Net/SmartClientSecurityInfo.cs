using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "SmartClientSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SmartClientSecurityInfo : ISmartClientSecurityInfo
{
	private bool SmartClientBrowseField;

	private bool SmartClientLiveField;

	private bool SmartClientReportField;

	private bool SmartClientSetupField;

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

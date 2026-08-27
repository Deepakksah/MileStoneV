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
public class SmartClientSecurityInfo : ISmartClientSecurityInfo
{
	private bool smartClientLiveField;

	private bool smartClientBrowseField;

	private bool smartClientSetupField;

	private bool smartClientReportField;

	public bool SmartClientLive
	{
		get
		{
			return smartClientLiveField;
		}
		set
		{
			smartClientLiveField = value;
		}
	}

	public bool SmartClientBrowse
	{
		get
		{
			return smartClientBrowseField;
		}
		set
		{
			smartClientBrowseField = value;
		}
	}

	public bool SmartClientSetup
	{
		get
		{
			return smartClientSetupField;
		}
		set
		{
			smartClientSetupField = value;
		}
	}

	public bool SmartClientReport
	{
		get
		{
			return smartClientReportField;
		}
		set
		{
			smartClientReportField = value;
		}
	}
}

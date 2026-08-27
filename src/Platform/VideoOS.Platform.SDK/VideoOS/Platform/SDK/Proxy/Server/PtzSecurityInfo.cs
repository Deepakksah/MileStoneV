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
public class PtzSecurityInfo : IPtzSecurityInfo
{
	private bool manualControlField;

	private bool presetControlField;

	private bool presetManageField;

	private bool presetManageLockField;

	public bool ReserveControl { get; set; }

	public bool ManualControl
	{
		get
		{
			return manualControlField;
		}
		set
		{
			manualControlField = value;
		}
	}

	public bool PresetControl
	{
		get
		{
			return presetControlField;
		}
		set
		{
			presetControlField = value;
		}
	}

	public bool PresetManage
	{
		get
		{
			return presetManageField;
		}
		set
		{
			presetManageField = value;
		}
	}

	public bool PresetManageLock
	{
		get
		{
			return presetManageLockField;
		}
		set
		{
			presetManageLockField = value;
		}
	}
}

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
public class PtzInfo : IPtzInfo
{
	private bool ptzEnabledField;

	private PresetInfo[] presetsField;

	private CapabilityInfo[] capabilitiesField;

	private bool isCenterOnPositionInViewSupportedField;

	private bool isPtzCenterAndZoomToRectangleSupportedField;

	private bool isPtzHomeSupportedField;

	private bool isPtzDiagonalSupportedField;

	private bool editPresetField;

	IPresetInfo[] IPtzInfo.Presets
	{
		get
		{
			return Presets;
		}
		set
		{
			Presets = (PresetInfo[])value;
		}
	}

	ICapabilityInfo[] IPtzInfo.Capabilities
	{
		get
		{
			return Capabilities;
		}
		set
		{
			Capabilities = (CapabilityInfo[])value;
		}
	}

	public bool PtzEnabled
	{
		get
		{
			return ptzEnabledField;
		}
		set
		{
			ptzEnabledField = value;
		}
	}

	public PresetInfo[] Presets
	{
		get
		{
			return presetsField;
		}
		set
		{
			presetsField = value;
		}
	}

	public CapabilityInfo[] Capabilities
	{
		get
		{
			return capabilitiesField;
		}
		set
		{
			capabilitiesField = value;
		}
	}

	public bool IsCenterOnPositionInViewSupported
	{
		get
		{
			return isCenterOnPositionInViewSupportedField;
		}
		set
		{
			isCenterOnPositionInViewSupportedField = value;
		}
	}

	public bool IsPtzCenterAndZoomToRectangleSupported
	{
		get
		{
			return isPtzCenterAndZoomToRectangleSupportedField;
		}
		set
		{
			isPtzCenterAndZoomToRectangleSupportedField = value;
		}
	}

	public bool IsPtzHomeSupported
	{
		get
		{
			return isPtzHomeSupportedField;
		}
		set
		{
			isPtzHomeSupportedField = value;
		}
	}

	public bool IsPtzDiagonalSupported
	{
		get
		{
			return isPtzDiagonalSupportedField;
		}
		set
		{
			isPtzDiagonalSupportedField = value;
		}
	}

	public bool EditPreset
	{
		get
		{
			return editPresetField;
		}
		set
		{
			editPresetField = value;
		}
	}
}

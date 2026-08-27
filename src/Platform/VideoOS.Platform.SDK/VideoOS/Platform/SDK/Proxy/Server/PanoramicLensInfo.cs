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
public class PanoramicLensInfo : IPanoramicLensInfo
{
	private bool panoramicLensEnabledField;

	private string panoramicLensTypeField;

	private string cameraMountField;

	private ImmerVisionInfo immerVisionField;

	IImmerVisionInfo IPanoramicLensInfo.ImmerVision
	{
		get
		{
			return ImmerVision;
		}
		set
		{
			ImmerVision = (ImmerVisionInfo)value;
		}
	}

	public bool PanoramicLensEnabled
	{
		get
		{
			return panoramicLensEnabledField;
		}
		set
		{
			panoramicLensEnabledField = value;
		}
	}

	public string PanoramicLensType
	{
		get
		{
			return panoramicLensTypeField;
		}
		set
		{
			panoramicLensTypeField = value;
		}
	}

	public string CameraMount
	{
		get
		{
			return cameraMountField;
		}
		set
		{
			cameraMountField = value;
		}
	}

	public ImmerVisionInfo ImmerVision
	{
		get
		{
			return immerVisionField;
		}
		set
		{
			immerVisionField = value;
		}
	}
}

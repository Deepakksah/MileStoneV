using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class dynamiccapabilitiesTypePtz
{
	private bool centerOnPositionInViewField;

	private bool centerAndZoomToRectangleField;

	private bool enabledField;

	public bool centerOnPositionInView
	{
		get
		{
			return centerOnPositionInViewField;
		}
		set
		{
			centerOnPositionInViewField = value;
		}
	}

	public bool centerAndZoomToRectangle
	{
		get
		{
			return centerAndZoomToRectangleField;
		}
		set
		{
			centerAndZoomToRectangleField = value;
		}
	}

	public bool enabled
	{
		get
		{
			return enabledField;
		}
		set
		{
			enabledField = value;
		}
	}
}

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
public class dynamiccapabilitiesTypePresets
{
	private dynamiccapabilitiesTypePresetsPresettype presettypeField;

	private dynamiccapabilitiesTypePresetsRelative relativeField;

	public dynamiccapabilitiesTypePresetsPresettype presettype
	{
		get
		{
			return presettypeField;
		}
		set
		{
			presettypeField = value;
		}
	}

	public dynamiccapabilitiesTypePresetsRelative relative
	{
		get
		{
			return relativeField;
		}
		set
		{
			relativeField = value;
		}
	}
}

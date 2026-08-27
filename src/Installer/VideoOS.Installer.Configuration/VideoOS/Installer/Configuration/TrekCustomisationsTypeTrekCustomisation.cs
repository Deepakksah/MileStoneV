using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class TrekCustomisationsTypeTrekCustomisation
{
	private string trekIdField;

	private ConditionType conditionField;

	private bool conditionFieldSpecified;

	private VisibilityType visibilityField;

	private bool visibilityFieldSpecified;

	[XmlAttribute]
	public string TrekId
	{
		get
		{
			return trekIdField;
		}
		set
		{
			trekIdField = value;
		}
	}

	[XmlAttribute]
	public ConditionType Condition
	{
		get
		{
			return conditionField;
		}
		set
		{
			conditionField = value;
		}
	}

	[XmlIgnore]
	public bool ConditionSpecified
	{
		get
		{
			return conditionFieldSpecified;
		}
		set
		{
			conditionFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public VisibilityType Visibility
	{
		get
		{
			return visibilityField;
		}
		set
		{
			visibilityField = value;
		}
	}

	[XmlIgnore]
	public bool VisibilitySpecified
	{
		get
		{
			return visibilityFieldSpecified;
		}
		set
		{
			visibilityFieldSpecified = value;
		}
	}
}

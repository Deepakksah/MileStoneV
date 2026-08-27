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
[XmlType(Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class capabilityType : InformationType
{
	private InformationType categoryField;

	private capabilityTypeType typeField;

	private access accessField;

	private object itemField;

	private bool reconnectrequiredField;

	private bool reconnectrequiredFieldSpecified;

	private bool complextypeField;

	private bool complextypeFieldSpecified;

	private bool hiddenField;

	private bool hiddenFieldSpecified;

	public InformationType category
	{
		get
		{
			return categoryField;
		}
		set
		{
			categoryField = value;
		}
	}

	public capabilityTypeType type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	public access access
	{
		get
		{
			return accessField;
		}
		set
		{
			accessField = value;
		}
	}

	[XmlElement("data", typeof(dataType))]
	[XmlElement("enumeration", typeof(enumerationType))]
	public object Item
	{
		get
		{
			return itemField;
		}
		set
		{
			itemField = value;
		}
	}

	public bool reconnectrequired
	{
		get
		{
			return reconnectrequiredField;
		}
		set
		{
			reconnectrequiredField = value;
		}
	}

	[XmlIgnore]
	public bool reconnectrequiredSpecified
	{
		get
		{
			return reconnectrequiredFieldSpecified;
		}
		set
		{
			reconnectrequiredFieldSpecified = value;
		}
	}

	public bool complextype
	{
		get
		{
			return complextypeField;
		}
		set
		{
			complextypeField = value;
		}
	}

	[XmlIgnore]
	public bool complextypeSpecified
	{
		get
		{
			return complextypeFieldSpecified;
		}
		set
		{
			complextypeFieldSpecified = value;
		}
	}

	public bool hidden
	{
		get
		{
			return hiddenField;
		}
		set
		{
			hiddenField = value;
		}
	}

	[XmlIgnore]
	public bool hiddenSpecified
	{
		get
		{
			return hiddenFieldSpecified;
		}
		set
		{
			hiddenFieldSpecified = value;
		}
	}
}

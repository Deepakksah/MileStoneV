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
public class dynamiccapabilitiesTypeCapabilityControl
{
	private bool relativesupportField;

	private bool speedsupportField;

	private bool automaticsupportField;

	private bool continuessupportField;

	private bool absolutesupportField;

	private dynamiccapabilitiesTypeCapabilityControlSpeedsettings speedsettingsField;

	private bool stopsupportField;

	private bool homesupportField;

	private bool diagonalsupportField;

	public bool relativesupport
	{
		get
		{
			return relativesupportField;
		}
		set
		{
			relativesupportField = value;
		}
	}

	public bool speedsupport
	{
		get
		{
			return speedsupportField;
		}
		set
		{
			speedsupportField = value;
		}
	}

	public bool automaticsupport
	{
		get
		{
			return automaticsupportField;
		}
		set
		{
			automaticsupportField = value;
		}
	}

	public bool continuessupport
	{
		get
		{
			return continuessupportField;
		}
		set
		{
			continuessupportField = value;
		}
	}

	public bool absolutesupport
	{
		get
		{
			return absolutesupportField;
		}
		set
		{
			absolutesupportField = value;
		}
	}

	public dynamiccapabilitiesTypeCapabilityControlSpeedsettings speedsettings
	{
		get
		{
			return speedsettingsField;
		}
		set
		{
			speedsettingsField = value;
		}
	}

	public bool stopsupport
	{
		get
		{
			return stopsupportField;
		}
		set
		{
			stopsupportField = value;
		}
	}

	public bool homesupport
	{
		get
		{
			return homesupportField;
		}
		set
		{
			homesupportField = value;
		}
	}

	public bool diagonalsupport
	{
		get
		{
			return diagonalsupportField;
		}
		set
		{
			diagonalsupportField = value;
		}
	}
}

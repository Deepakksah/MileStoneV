using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class capabilityType
{
	private string nameField;

	private string getField;

	private string setField;

	private dependantType[] dependantsField;

	private string typeField;

	private bool resetfeedField;

	private bool resetfeedFieldSpecified;

	private string resetwaitmsField;

	private bool initsetField;

	private bool initsetFieldSpecified;

	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string get
	{
		get
		{
			return getField;
		}
		set
		{
			getField = value;
		}
	}

	public string set
	{
		get
		{
			return setField;
		}
		set
		{
			setField = value;
		}
	}

	[XmlArrayItem("dependant", IsNullable = false)]
	public dependantType[] dependants
	{
		get
		{
			return dependantsField;
		}
		set
		{
			dependantsField = value;
		}
	}

	[XmlAttribute]
	public string type
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

	[XmlAttribute]
	public bool resetfeed
	{
		get
		{
			return resetfeedField;
		}
		set
		{
			resetfeedField = value;
		}
	}

	[XmlIgnore]
	public bool resetfeedSpecified
	{
		get
		{
			return resetfeedFieldSpecified;
		}
		set
		{
			resetfeedFieldSpecified = value;
		}
	}

	[XmlAttribute(DataType = "integer")]
	public string resetwaitms
	{
		get
		{
			return resetwaitmsField;
		}
		set
		{
			resetwaitmsField = value;
		}
	}

	[XmlAttribute]
	public bool initset
	{
		get
		{
			return initsetField;
		}
		set
		{
			initsetField = value;
		}
	}

	[XmlIgnore]
	public bool initsetSpecified
	{
		get
		{
			return initsetFieldSpecified;
		}
		set
		{
			initsetFieldSpecified = value;
		}
	}
}

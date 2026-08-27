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
public class dataType
{
	private dataTypeDatatype datatypeField;

	private object minvalueField;

	private object maxvalueField;

	private dataTypeMinvaluedisplayname[] minvaluedisplaynameField;

	private dataTypeMaxvaluedisplayname[] maxvaluedisplaynameField;

	private dataTypeUnit unitField;

	private bool unitFieldSpecified;

	private object resolutionField;

	private dataTypeUnitdisplayname[] unitdisplaynameField;

	private object defaultvalueField;

	public dataTypeDatatype datatype
	{
		get
		{
			return datatypeField;
		}
		set
		{
			datatypeField = value;
		}
	}

	public object minvalue
	{
		get
		{
			return minvalueField;
		}
		set
		{
			minvalueField = value;
		}
	}

	public object maxvalue
	{
		get
		{
			return maxvalueField;
		}
		set
		{
			maxvalueField = value;
		}
	}

	[XmlElement("minvaluedisplayname")]
	public dataTypeMinvaluedisplayname[] minvaluedisplayname
	{
		get
		{
			return minvaluedisplaynameField;
		}
		set
		{
			minvaluedisplaynameField = value;
		}
	}

	[XmlElement("maxvaluedisplayname")]
	public dataTypeMaxvaluedisplayname[] maxvaluedisplayname
	{
		get
		{
			return maxvaluedisplaynameField;
		}
		set
		{
			maxvaluedisplaynameField = value;
		}
	}

	public dataTypeUnit unit
	{
		get
		{
			return unitField;
		}
		set
		{
			unitField = value;
		}
	}

	[XmlIgnore]
	public bool unitSpecified
	{
		get
		{
			return unitFieldSpecified;
		}
		set
		{
			unitFieldSpecified = value;
		}
	}

	public object resolution
	{
		get
		{
			return resolutionField;
		}
		set
		{
			resolutionField = value;
		}
	}

	[XmlElement("unitdisplayname")]
	public dataTypeUnitdisplayname[] unitdisplayname
	{
		get
		{
			return unitdisplaynameField;
		}
		set
		{
			unitdisplaynameField = value;
		}
	}

	public object defaultvalue
	{
		get
		{
			return defaultvalueField;
		}
		set
		{
			defaultvalueField = value;
		}
	}
}

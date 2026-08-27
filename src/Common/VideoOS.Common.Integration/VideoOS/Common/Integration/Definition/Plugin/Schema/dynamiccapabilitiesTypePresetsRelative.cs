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
public class dynamiccapabilitiesTypePresetsRelative
{
	private bool isnumberedField;

	private int minnumberField;

	private bool minnumberFieldSpecified;

	private int maxnumberField;

	private bool maxnumberFieldSpecified;

	private bool loadfromdeviceField;

	private bool devicelistonlyField;

	public bool isnumbered
	{
		get
		{
			return isnumberedField;
		}
		set
		{
			isnumberedField = value;
		}
	}

	public int minnumber
	{
		get
		{
			return minnumberField;
		}
		set
		{
			minnumberField = value;
		}
	}

	[XmlIgnore]
	public bool minnumberSpecified
	{
		get
		{
			return minnumberFieldSpecified;
		}
		set
		{
			minnumberFieldSpecified = value;
		}
	}

	public int maxnumber
	{
		get
		{
			return maxnumberField;
		}
		set
		{
			maxnumberField = value;
		}
	}

	[XmlIgnore]
	public bool maxnumberSpecified
	{
		get
		{
			return maxnumberFieldSpecified;
		}
		set
		{
			maxnumberFieldSpecified = value;
		}
	}

	public bool loadfromdevice
	{
		get
		{
			return loadfromdeviceField;
		}
		set
		{
			loadfromdeviceField = value;
		}
	}

	public bool devicelistonly
	{
		get
		{
			return devicelistonlyField;
		}
		set
		{
			devicelistonlyField = value;
		}
	}
}

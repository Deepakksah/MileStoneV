using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class datacontentType : typedefcontentType
{
	private string minvalueField;

	private string maxvalueField;

	private string resolutionField;

	private string defaultvalueField;

	private string conversionfactorField;

	private string conversionoffsetField;

	private string normalizationformatstringField;

	public string minvalue
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

	public string maxvalue
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

	public string resolution
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

	public string defaultvalue
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

	public string conversionfactor
	{
		get
		{
			return conversionfactorField;
		}
		set
		{
			conversionfactorField = value;
		}
	}

	public string conversionoffset
	{
		get
		{
			return conversionoffsetField;
		}
		set
		{
			conversionoffsetField = value;
		}
	}

	public string normalizationformatstring
	{
		get
		{
			return normalizationformatstringField;
		}
		set
		{
			normalizationformatstringField = value;
		}
	}
}

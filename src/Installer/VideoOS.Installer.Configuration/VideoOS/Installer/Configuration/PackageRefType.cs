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
[XmlType(Namespace = "urn:milestone-systems")]
[DebuggerDisplay("{Name} - {ID}")]
public class PackageRefType
{
	private TrekCustomisationsTypeTrekCustomisation[] trekCustomisationsField;

	private bool sSRegistrationField;

	private string nameField;

	private string targetParameterField;

	private string defaultTargetValueField;

	private string idField;

	private ConditionType conditionField;

	private bool conditionFieldSpecified;

	private VisibilityType visibilityField;

	private bool visibilityFieldSpecified;

	private bool includeInARPField;

	private bool includeInARPFieldSpecified;

	private bool allowUnInstallField;

	private bool allowUnInstallFieldSpecified;

	private bool runOnlyPluginField;

	[XmlArrayItem("TrekCustomisation", IsNullable = false)]
	public TrekCustomisationsTypeTrekCustomisation[] TrekCustomisations
	{
		get
		{
			return trekCustomisationsField;
		}
		set
		{
			trekCustomisationsField = value;
		}
	}

	[DefaultValue(false)]
	public bool SSRegistration
	{
		get
		{
			return sSRegistrationField;
		}
		set
		{
			sSRegistrationField = value;
		}
	}

	[XmlAttribute]
	public string Name
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

	[XmlAttribute]
	public string TargetParameter
	{
		get
		{
			return targetParameterField;
		}
		set
		{
			targetParameterField = value;
		}
	}

	[XmlAttribute]
	public string DefaultTargetValue
	{
		get
		{
			return defaultTargetValueField;
		}
		set
		{
			defaultTargetValueField = value;
		}
	}

	[XmlAttribute]
	public string ID
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
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

	[XmlAttribute]
	public bool IncludeInARP
	{
		get
		{
			return includeInARPField;
		}
		set
		{
			includeInARPField = value;
		}
	}

	[XmlIgnore]
	public bool IncludeInARPSpecified
	{
		get
		{
			return includeInARPFieldSpecified;
		}
		set
		{
			includeInARPFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public bool AllowUnInstall
	{
		get
		{
			return allowUnInstallField;
		}
		set
		{
			allowUnInstallField = value;
		}
	}

	[XmlIgnore]
	public bool AllowUnInstallSpecified
	{
		get
		{
			return allowUnInstallFieldSpecified;
		}
		set
		{
			allowUnInstallFieldSpecified = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool RunOnlyPlugin
	{
		get
		{
			return runOnlyPluginField;
		}
		set
		{
			runOnlyPluginField = value;
		}
	}

	public PackageRefType()
	{
		sSRegistrationField = false;
		runOnlyPluginField = false;
	}

	public override int GetHashCode()
	{
		return ID.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is PackageRefType packageRefType)
		{
			return packageRefType.ID == ID;
		}
		return false;
	}
}

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class PackagePropertyType
{
	private StringType displayNameField;

	private StringType descriptionField;

	private string nameField;

	private DefaultPropertyValueType defaultValueField;

	private bool visibleField;

	private bool readOnlyField;

	private bool emptyValueAllowedField;

	private bool omitIfEmptyValueField;

	public StringType DisplayName
	{
		get
		{
			return displayNameField;
		}
		set
		{
			displayNameField = value;
		}
	}

	public StringType Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

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

	public DefaultPropertyValueType DefaultValue
	{
		get
		{
			return defaultValueField;
		}
		set
		{
			defaultValueField = value;
		}
	}

	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			return visibleField;
		}
		set
		{
			visibleField = value;
		}
	}

	[DefaultValue(false)]
	public bool ReadOnly
	{
		get
		{
			return readOnlyField;
		}
		set
		{
			readOnlyField = value;
		}
	}

	[DefaultValue(true)]
	public bool EmptyValueAllowed
	{
		get
		{
			return emptyValueAllowedField;
		}
		set
		{
			emptyValueAllowedField = value;
		}
	}

	[DefaultValue(true)]
	public bool OmitIfEmptyValue
	{
		get
		{
			return omitIfEmptyValueField;
		}
		set
		{
			omitIfEmptyValueField = value;
		}
	}

	public PackagePropertyType()
	{
		visibleField = true;
		readOnlyField = false;
		emptyValueAllowedField = true;
		omitIfEmptyValueField = true;
	}
}

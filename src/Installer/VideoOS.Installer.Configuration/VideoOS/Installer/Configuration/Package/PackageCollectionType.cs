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
public class PackageCollectionType
{
	private ProductInfoType productInfoField;

	private ProductVersionInfoType minVersionField;

	private ProductVersionInfoType maxVersionField;

	private ProductVersionInfoType[] excludedVersionsField;

	private ProductVersionInfoType[] includedVersionsField;

	private PrerequisiteType typeField;

	public ProductInfoType ProductInfo
	{
		get
		{
			return productInfoField;
		}
		set
		{
			productInfoField = value;
		}
	}

	public ProductVersionInfoType MinVersion
	{
		get
		{
			return minVersionField;
		}
		set
		{
			minVersionField = value;
		}
	}

	public ProductVersionInfoType MaxVersion
	{
		get
		{
			return maxVersionField;
		}
		set
		{
			maxVersionField = value;
		}
	}

	[XmlArrayItem("Version", IsNullable = false)]
	public ProductVersionInfoType[] ExcludedVersions
	{
		get
		{
			return excludedVersionsField;
		}
		set
		{
			excludedVersionsField = value;
		}
	}

	[XmlArrayItem("Version", IsNullable = false)]
	public ProductVersionInfoType[] IncludedVersions
	{
		get
		{
			return includedVersionsField;
		}
		set
		{
			includedVersionsField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(PrerequisiteType.Required)]
	public PrerequisiteType Type
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

	public PackageCollectionType()
	{
		typeField = PrerequisiteType.Required;
	}
}

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
public class PrerequisiteInfoType
{
	private string productCodeField;

	private string upgradeCodeField;

	private string packageCodeField;

	private StringType displayNameField;

	private string manufactureField;

	private string manufactureLinkField;

	private string installationFolderField;

	private string manifestResourceFolderNameField;

	private long spaceRequiredField;

	private bool spaceRequiredFieldSpecified;

	private bool languagePackField;

	private StringType descriptionField;

	private PrerequisiteInfoTypeProductManifest productManifestField;

	public string ProductCode
	{
		get
		{
			return productCodeField;
		}
		set
		{
			productCodeField = value;
		}
	}

	public string UpgradeCode
	{
		get
		{
			return upgradeCodeField;
		}
		set
		{
			upgradeCodeField = value;
		}
	}

	public string PackageCode
	{
		get
		{
			return packageCodeField;
		}
		set
		{
			packageCodeField = value;
		}
	}

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

	public string Manufacture
	{
		get
		{
			return manufactureField;
		}
		set
		{
			manufactureField = value;
		}
	}

	public string ManufactureLink
	{
		get
		{
			return manufactureLinkField;
		}
		set
		{
			manufactureLinkField = value;
		}
	}

	public string InstallationFolder
	{
		get
		{
			return installationFolderField;
		}
		set
		{
			installationFolderField = value;
		}
	}

	public string ManifestResourceFolderName
	{
		get
		{
			return manifestResourceFolderNameField;
		}
		set
		{
			manifestResourceFolderNameField = value;
		}
	}

	public long SpaceRequired
	{
		get
		{
			return spaceRequiredField;
		}
		set
		{
			spaceRequiredField = value;
		}
	}

	[XmlIgnore]
	public bool SpaceRequiredSpecified
	{
		get
		{
			return spaceRequiredFieldSpecified;
		}
		set
		{
			spaceRequiredFieldSpecified = value;
		}
	}

	[DefaultValue(false)]
	public bool LanguagePack
	{
		get
		{
			return languagePackField;
		}
		set
		{
			languagePackField = value;
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

	public PrerequisiteInfoTypeProductManifest ProductManifest
	{
		get
		{
			return productManifestField;
		}
		set
		{
			productManifestField = value;
		}
	}

	public PrerequisiteInfoType()
	{
		languagePackField = false;
	}
}

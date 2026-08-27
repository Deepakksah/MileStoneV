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
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class LicenseCustomisationsTypeLicenseCustom
{
	private string displayNameField;

	private string displayVersionField;

	private string captionField;

	private LicenseCustomisationsTypeLicenseCustomOpenLinkOnFinish openLinkOnFinishField;

	private LicenseCustomisationsTypeLicenseCustomShowWhyUninstalling showWhyUninstallingField;

	private string productKeyField;

	public string DisplayName
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

	public string DisplayVersion
	{
		get
		{
			return displayVersionField;
		}
		set
		{
			displayVersionField = value;
		}
	}

	public string Caption
	{
		get
		{
			return captionField;
		}
		set
		{
			captionField = value;
		}
	}

	public LicenseCustomisationsTypeLicenseCustomOpenLinkOnFinish OpenLinkOnFinish
	{
		get
		{
			return openLinkOnFinishField;
		}
		set
		{
			openLinkOnFinishField = value;
		}
	}

	public LicenseCustomisationsTypeLicenseCustomShowWhyUninstalling ShowWhyUninstalling
	{
		get
		{
			return showWhyUninstallingField;
		}
		set
		{
			showWhyUninstallingField = value;
		}
	}

	[XmlAttribute]
	public string ProductKey
	{
		get
		{
			return productKeyField;
		}
		set
		{
			productKeyField = value;
		}
	}
}

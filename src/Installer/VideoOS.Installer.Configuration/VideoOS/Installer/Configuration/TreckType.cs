using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class TreckType
{
	private ShowPageType[] showPagesField;

	private ShowPageType[] pagesToShowPostInstallField;

	private string descriptionIdField;

	private TreckTypeFinishSetup finishSetupField;

	private string idField;

	private string allowUpgradeFromField;

	[XmlArrayItem("Page", IsNullable = false)]
	public ShowPageType[] ShowPages
	{
		get
		{
			return showPagesField;
		}
		set
		{
			showPagesField = value;
		}
	}

	[XmlArrayItem("Page", IsNullable = false)]
	public ShowPageType[] PagesToShowPostInstall
	{
		get
		{
			return pagesToShowPostInstallField;
		}
		set
		{
			pagesToShowPostInstallField = value;
		}
	}

	public string DescriptionId
	{
		get
		{
			return descriptionIdField;
		}
		set
		{
			descriptionIdField = value;
		}
	}

	public TreckTypeFinishSetup FinishSetup
	{
		get
		{
			return finishSetupField;
		}
		set
		{
			finishSetupField = value;
		}
	}

	[XmlAttribute]
	public string Id
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
	public string AllowUpgradeFrom
	{
		get
		{
			return allowUpgradeFromField;
		}
		set
		{
			allowUpgradeFromField = value;
		}
	}

	public bool DoesAllowUpgradeFrom(string prevTreckId)
	{
		if (prevTreckId == Id)
		{
			return true;
		}
		if (string.IsNullOrEmpty(AllowUpgradeFrom))
		{
			return false;
		}
		return AllowUpgradeFrom.Split(';').Contains(prevTreckId);
	}
}

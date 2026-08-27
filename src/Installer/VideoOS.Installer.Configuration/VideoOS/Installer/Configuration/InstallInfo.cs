using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class InstallInfo
{
	private List<PackageRefType> _installedPackages;

	private List<PackageRefType> _notInstalledPackages;

	private List<PackageRefType> _notUninstalledPackages;

	private InstallEnvironment _installEnvironment;

	private string _productLanguage;

	private string _installerLanguage;

	private bool _isTrialVersion;

	[XmlArrayItem("InstalledPackages")]
	public List<PackageRefType> InstalledPackages
	{
		get
		{
			return _installedPackages;
		}
		set
		{
			_installedPackages = value;
		}
	}

	[XmlArrayItem("NotInstalledPackages")]
	public List<PackageRefType> NotInstalledPackages
	{
		get
		{
			return _notInstalledPackages;
		}
		set
		{
			_notInstalledPackages = value;
		}
	}

	[XmlArrayItem("NotUninstalledPackages")]
	public List<PackageRefType> NotUninstalledPackages
	{
		get
		{
			return _notUninstalledPackages;
		}
		set
		{
			_notUninstalledPackages = value;
		}
	}

	public InstallEnvironment InstallEnvironment
	{
		get
		{
			return _installEnvironment;
		}
		set
		{
			_installEnvironment = value;
		}
	}

	public string ProductLanguage
	{
		get
		{
			return _productLanguage;
		}
		set
		{
			_productLanguage = value;
		}
	}

	public string InstallerLanguage
	{
		get
		{
			return _installerLanguage;
		}
		set
		{
			_installerLanguage = value;
		}
	}

	public bool IsTrialVersion
	{
		get
		{
			return _isTrialVersion;
		}
		set
		{
			_isTrialVersion = value;
		}
	}
}

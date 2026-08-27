using System;
using System.Drawing;
using VideoOS.Installer.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.Manager.CertificateCommon;

public class ServerConfiguratorApplicationResources : IApplicationResources
{
	public string CompanyName => GlobalData.OemHelper.CompanyName;

	public string ProductNameWithoutCompany => GlobalData.OemHelper.ProductNameWithoutCompany;

	public string CertificateGuideUri => GlobalData.OemHelper.CertificateGuideUri;

	public string ServerConfiguratorInstallFolder
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public string ServerConfiguratorApplicationName
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public Icon ServerConfiguratorIcon
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public string RegisterTabLearnMoreLink
	{
		get
		{
			throw new NotSupportedException();
		}
	}
}

using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security;
using System.Xml;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.Utilities.OemResource;

namespace VideoOS.ServerConfigurator.Common;

public class ApplicationResources : IApplicationResources
{
	private OemResourcePackage _oemResourcePackage;

	private const string ResourceFilename = "resources.xml";

	private IResourcesFolderLocator resourcesFolderLocator;

	public string CompanyName => GetResource<string>("CompanyName");

	public string ProductNameWithoutCompany => GetResource<string>("ProductNameWithoutCompany");

	public string ServerConfiguratorInstallFolder => GetResource<string>("ServerConfiguratorInstallFolder");

	public string ServerConfiguratorApplicationName => GetResource<string>("ServerConfiguratorApplicationName");

	public Icon ServerConfiguratorIcon => GetResource<Icon>("ServerConfiguratorIcon");

	public string CertificateGuideUri => GetResource<string>("CertificateGuideUri");

	public string RegisterTabLearnMoreLink => GetResource<string>("RegisterTabLearnMoreLink");

	public ApplicationResources(IResourcesFolderLocator locator)
	{
		resourcesFolderLocator = locator;
	}

	public ApplicationResources()
	{
	}

	public void LoadAndValidate()
	{
		IResourcesFolderLocator locator = resourcesFolderLocator ?? new ResourcesFolderLocator("resources.xml");
		LoadAndValidate(locator);
	}

	private T GetResource<T>([CallerMemberName] string name = "")
	{
		if (_oemResourcePackage == null)
		{
			LoadAndValidate();
		}
		return (T)_oemResourcePackage.GetResourceItem(name).ResourceValueAsObject;
	}

	private void LoadAndValidate(IResourcesFolderLocator locator)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(locator.GetResourceFilePath);
		_oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!_oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
	}
}

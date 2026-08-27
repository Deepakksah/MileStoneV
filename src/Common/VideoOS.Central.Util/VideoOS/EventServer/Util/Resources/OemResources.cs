using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.EventServer.Util.Resources;

public static class OemResources
{
	private const string ResourceFilename = "resources.xml";

	private static OemResourcePackage _oemResourcePackage;

	private static Icon _icon;

	public static string DataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyNameShort, EventServerApplicationNameShort);

	public static string CompanyNameShort => GetResourceString("CompanyNameShort");

	public static string CompanyName => GetResourceString("CompanyName");

	public static string Manufacturer => GetResourceString("Manufacturer");

	public static string ProductVersionText => GetResourceString("ProductVersionText");

	public static string ManagerApplicationHelpUrl => GetResourceString("ManagerApplicationHelpUrl");

	public static string EventServerApplicationNamePlain => GetResourceString("EventServerApplicationNamePlain");

	public static string EventServerApplicationNameShort => GetResourceString("EventServerApplicationNameShort");

	public static string EventServerApplicationName => GetResourceString("EventServerApplicationName");

	public static string EventServerServiceName => GetResourceString("EventServerServiceName");

	public static string EventServerServiceDisplayName => GetResourceString("EventServerServiceDisplayName");

	public static string ProductNameCorporate => GetResourceString("ProductNameCorporate");

	public static string ProductNameExpert => GetResourceString("ProductNameExpert");

	public static string ProductNameEnterprise => GetResourceString("ProductNameEnterprise");

	public static string ProductNameProfessional => GetResourceString("ProductNameProfessional");

	public static string ProductNameExpress => GetResourceString("ProductNameExpress");

	public static string ProductNameEssential => GetResourceString("ProductNameEssential");

	public static string ProductNameGO => GetResourceString("ProductNameGO");

	public static Icon EventServerTrayIcon
	{
		get
		{
			if (_icon == null)
			{
				_icon = GetResourceIcon("EventServerTrayIcon");
			}
			return _icon;
		}
	}

	public static Image WizardSmallLogo => GetResourceImage("WizardSmallLogo");

	public static Image WizardSmallTopBar => GetResourceImage("WizardSmallTopBar");

	public static Color WizardSmallTopBarTextColorName => Color.FromName(GetResourceString("WizardSmallTopBarTextColorName"));

	public static void LoadAndValidate()
	{
		LoadAndValidate(new ResourcesFolderLocator("resources.xml"));
	}

	public static void LoadAndValidate(IResourcesFolderLocator resourcesFolderLocator)
	{
		if (resourcesFolderLocator == null)
		{
			throw new ArgumentNullException("resourcesFolderLocator");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(resourcesFolderLocator.GetResourceFilePath);
		_oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!_oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
	}

	private static string GetResourceString([CallerMemberName] string name = "")
	{
		if (_oemResourcePackage == null)
		{
			LoadAndValidate();
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return "[" + name + "]";
		}
		return resourceItem.ResourceValueAsObject as string;
	}

	private static Icon GetResourceIcon([CallerMemberName] string name = "")
	{
		if (_oemResourcePackage == null)
		{
			LoadAndValidate();
		}
		return _oemResourcePackage.GetResourceItem(name).ResourceValueAsObject as Icon;
	}

	private static Image GetResourceImage([CallerMemberName] string name = "")
	{
		if (_oemResourcePackage == null)
		{
			LoadAndValidate();
		}
		return ((OemResourceItemImage)_oemResourcePackage.GetResourceItem(name)).ResourceValue;
	}
}

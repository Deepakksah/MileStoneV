using System.IO;
using System.Security;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.SCH.Base;

public class OemResources
{
	private IApplicationFolderLocater _applicationFolderLocater;

	private static object instanceSync = new object();

	private static volatile OemResources instance;

	private string _resourceFileLocation;

	public OemResourcePackage _resourcePackage;

	public string _companyName;

	private string _serviceChannelInstallDir;

	private string _applicationName;

	public static OemResources Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			lock (instanceSync)
			{
				if (instance == null)
				{
					instance = new OemResources(new ApplicationFolderLocater());
				}
			}
			return instance;
		}
	}

	public string ResourceFileLocation
	{
		get
		{
			if (_resourceFileLocation == null)
			{
				_resourceFileLocation = Path.Combine(_applicationFolderLocater.GetApplicationFolder("resources.xml"), "resources.xml");
			}
			return _resourceFileLocation;
		}
	}

	public OemResourcePackage ResourcePackage
	{
		get
		{
			if (_resourcePackage == null)
			{
				_resourcePackage = LoadAndValidateOemResourceFile(ResourceFileLocation);
			}
			return _resourcePackage;
		}
	}

	public string CompanyName
	{
		get
		{
			if (_companyName == null)
			{
				_companyName = ((OemResourceItemString)ResourcePackage.GetResourceItem("CompanyName")).ResourceValue;
			}
			return _companyName;
		}
	}

	public string ServiceChannelInstallDir
	{
		get
		{
			if (_serviceChannelInstallDir == null)
			{
				_serviceChannelInstallDir = ((OemResourceItemString)ResourcePackage.GetResourceItem("ServiceChannelInstallDir")).ResourceValue;
			}
			return _serviceChannelInstallDir;
		}
	}

	public string ApplicationName
	{
		get
		{
			if (_applicationName == null)
			{
				_applicationName = ((OemResourceItemString)ResourcePackage.GetResourceItem("ServiceChannelShortName")).ResourceValue;
			}
			return _applicationName;
		}
	}

	public OemResources(IApplicationFolderLocater locater)
	{
		_applicationFolderLocater = locater;
		instance = this;
	}

	private OemResourcePackage LoadAndValidateOemResourceFile(string fileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(fileName);
		OemResourcePackage oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The oem resource file is signed with an invalid signature.");
		}
		return oemResourcePackage;
	}
}

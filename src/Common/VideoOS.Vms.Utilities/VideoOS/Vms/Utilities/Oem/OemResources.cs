using System;
using System.IO;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Vms.Utilities.Oem;

public class OemResources : IOemResources
{
	private IApplicationFolderLocater applicationFolderLocater;

	private static object instanceSync = new object();

	private static volatile OemResources instance;

	private OemResourcePackage xpcoResourcePackage;

	private OemResourcePackage scResourcePackage;

	private object sync = new object();

	private string resourcePackageDirectory;

	private AppResources appResources;

	private SqlServerResources sqlResources;

	private ManagerResources managerResources;

	private ServerResources serverResources;

	private FailoverServerResources failoverServerResources;

	private RecorderResources recorderResources;

	private MobileServerResources mobileServerResources;

	private ReportWebServerResources reportWebServerResources;

	private VideoWallResources videoWallResources;

	private ServiceChannelResources serviceChannelResources;

	private SmartClientResources smartClientResources;

	IAppResources IOemResources.App => Instance.App;

	public static bool InDemoMode { get; private set; }

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

	public string ResourcePackageDirectory
	{
		get
		{
			OemResourcePackage oemResourcePackage = null;
			if (resourcePackageDirectory == null)
			{
				oemResourcePackage = XPCOResourcePackage;
			}
			return resourcePackageDirectory;
		}
		set
		{
			lock (sync)
			{
				resourcePackageDirectory = value;
			}
		}
	}

	private OemResourcePackage XPCOResourcePackage
	{
		get
		{
			if (xpcoResourcePackage != null)
			{
				return xpcoResourcePackage;
			}
			lock (sync)
			{
				if (xpcoResourcePackage != null)
				{
					return xpcoResourcePackage;
				}
				xpcoResourcePackage = LoadAndValidateOemResourceFile("resources.xml");
			}
			return xpcoResourcePackage;
		}
	}

	private OemResourcePackage SCResourcePackage
	{
		get
		{
			if (scResourcePackage != null)
			{
				return scResourcePackage;
			}
			lock (sync)
			{
				if (scResourcePackage != null)
				{
					return scResourcePackage;
				}
				scResourcePackage = LoadAndValidateOemResourceFile("resourcesSharedComponent.xml");
			}
			return scResourcePackage;
		}
	}

	public AppResources App
	{
		get
		{
			if (appResources != null)
			{
				return appResources;
			}
			lock (sync)
			{
				if (appResources != null)
				{
					return appResources;
				}
				appResources = new AppResources(XPCOResourcePackage);
				return appResources;
			}
		}
	}

	public SqlServerResources SqlServer
	{
		get
		{
			if (sqlResources != null)
			{
				return sqlResources;
			}
			lock (sync)
			{
				if (sqlResources != null)
				{
					return sqlResources;
				}
				sqlResources = new SqlServerResources(XPCOResourcePackage);
				return sqlResources;
			}
		}
	}

	public IManagerResources Manager
	{
		get
		{
			if (managerResources != null)
			{
				return managerResources;
			}
			lock (sync)
			{
				if (managerResources != null)
				{
					return managerResources;
				}
				managerResources = new ManagerResources(XPCOResourcePackage);
				return managerResources;
			}
		}
	}

	public IServerResources Server
	{
		get
		{
			if (serverResources != null)
			{
				return serverResources;
			}
			lock (sync)
			{
				if (serverResources != null)
				{
					return serverResources;
				}
				serverResources = new ServerResources(XPCOResourcePackage);
				return serverResources;
			}
		}
	}

	public FailoverServerResources FailoverServer
	{
		get
		{
			if (failoverServerResources != null)
			{
				return failoverServerResources;
			}
			lock (sync)
			{
				if (failoverServerResources != null)
				{
					return failoverServerResources;
				}
				failoverServerResources = new FailoverServerResources(XPCOResourcePackage);
				return failoverServerResources;
			}
		}
	}

	public IRecorderResources Recorder
	{
		get
		{
			if (recorderResources != null)
			{
				return recorderResources;
			}
			lock (sync)
			{
				if (recorderResources != null)
				{
					return recorderResources;
				}
				recorderResources = new RecorderResources(XPCOResourcePackage);
				return recorderResources;
			}
		}
	}

	public MobileServerResources MobileServer
	{
		get
		{
			if (mobileServerResources != null)
			{
				return mobileServerResources;
			}
			lock (sync)
			{
				if (mobileServerResources != null)
				{
					return mobileServerResources;
				}
				mobileServerResources = new MobileServerResources(XPCOResourcePackage);
				return mobileServerResources;
			}
		}
	}

	public ReportWebServerResources ReportWebServer
	{
		get
		{
			if (reportWebServerResources != null)
			{
				return reportWebServerResources;
			}
			lock (sync)
			{
				if (reportWebServerResources != null)
				{
					return reportWebServerResources;
				}
				reportWebServerResources = new ReportWebServerResources(XPCOResourcePackage);
				return reportWebServerResources;
			}
		}
	}

	public VideoWallResources VideoWall
	{
		get
		{
			if (videoWallResources != null)
			{
				return videoWallResources;
			}
			lock (sync)
			{
				if (videoWallResources != null)
				{
					return videoWallResources;
				}
				videoWallResources = new VideoWallResources(XPCOResourcePackage);
				return videoWallResources;
			}
		}
	}

	public ServiceChannelResources ServiceChannel
	{
		get
		{
			if (serviceChannelResources != null)
			{
				return serviceChannelResources;
			}
			lock (sync)
			{
				if (serviceChannelResources != null)
				{
					return serviceChannelResources;
				}
				serviceChannelResources = new ServiceChannelResources(XPCOResourcePackage);
				return serviceChannelResources;
			}
		}
	}

	public SmartClientResources SmartClient
	{
		get
		{
			if (smartClientResources != null)
			{
				return smartClientResources;
			}
			lock (sync)
			{
				if (smartClientResources != null)
				{
					return smartClientResources;
				}
				smartClientResources = new SmartClientResources(SCResourcePackage);
				return smartClientResources;
			}
		}
	}

	public OemResources(IApplicationFolderLocater locater)
	{
		applicationFolderLocater = locater;
		instance = this;
	}

	public static void Refresh()
	{
		instance = null;
	}

	private OemResourcePackage LoadAndValidateOemResourceFile(string fileName)
	{
		if (resourcePackageDirectory == null)
		{
			resourcePackageDirectory = applicationFolderLocater.GetApplicationFolder(fileName);
		}
		string filename = Path.Combine(resourcePackageDirectory, fileName);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(filename);
		OemResourcePackage oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!oemResourcePackage.ValidateSignature())
		{
			InDemoMode = true;
		}
		return oemResourcePackage;
	}

	public static string ReplaceAliases(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		return value.Replace("{PRODUCT NAME}", "").Replace("Matrix", Instance.Manager.MatrixApplicationName).Replace("{MANAGEMENT CLIENT}", Instance.Manager.ApplicationNameShort)
			.Replace("{SMARTWALL}", Instance.VideoWall.ApplicationNameShort)
			.Replace("{SC}", Instance.SmartClient.ClientName);
	}
}

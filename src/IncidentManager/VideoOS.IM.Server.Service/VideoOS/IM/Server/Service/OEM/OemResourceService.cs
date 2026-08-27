using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.Utilities.OemResource;

namespace VideoOS.IM.Server.Service.OEM;

public sealed class OemResourceService : IOemResourceService
{
	private const string ResourceFilename = "resources.xml";

	private static readonly Lazy<OemResourcePackage> _oemResourcePackage = new Lazy<OemResourcePackage>(delegate
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(ResourceFilePath);
		return new OemResourcePackage(xmlDocument);
	});

	private static readonly Lazy<IOemResourceService> LazyInstance = new Lazy<IOemResourceService>(() => new OemResourceService());

	public static IOemResourceService Instance => LazyInstance.Value;

	[NotNull]
	public string IncidentManagerApplicationName => GetResourceString("IncidentManagerApplicationName");

	[NotNull]
	public string CompanyNameShort => GetResourceString("CompanyNameShort");

	[NotNull]
	public string IncidentManagerApplicationNameLong => GetResourceString("IncidentManagerApplicationNameLong");

	[NotNull]
	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyNameShort), IncidentManagerApplicationName);

	private static string ResourceFilePath
	{
		get
		{
			string path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
			if (!File.Exists(Path.Combine(path, "resources.xml")))
			{
				path = AppDomain.CurrentDomain.BaseDirectory;
				if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
				{
					path = ((!Path.IsPathRooted(AppDomain.CurrentDomain.RelativeSearchPath)) ? new DirectoryInfo(Path.Combine(path, AppDomain.CurrentDomain.RelativeSearchPath)).FullName : AppDomain.CurrentDomain.RelativeSearchPath);
				}
			}
			return Path.Combine(path, "resources.xml");
		}
	}

	private OemResourceService()
	{
	}

	private static string GetResourceString([CallerMemberName] string name = "")
	{
		return _oemResourcePackage.Value.GetResourceItem(name).ResourceValueAsObject as string;
	}
}

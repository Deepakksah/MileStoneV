using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.IdentityServer.Oem;

public class OemResources
{
	private const string ResourceFilename = "resources.xml";

	private OemResourcePackage _oemResourcePackage;

	private static OemResources _instance;

	private static readonly object InstanceLocker = new object();

	public static OemResources Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (InstanceLocker)
				{
					if (_instance == null)
					{
						_instance = new OemResources();
					}
				}
			}
			return _instance;
		}
	}

	private static string ResourceFilePath
	{
		get
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (!File.Exists(Path.Combine(path, "resources.xml")))
			{
				path = AppDomain.CurrentDomain.BaseDirectory;
				if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
				{
					if (Path.IsPathRooted(AppDomain.CurrentDomain.RelativeSearchPath))
					{
						path = AppDomain.CurrentDomain.RelativeSearchPath;
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(path, AppDomain.CurrentDomain.RelativeSearchPath));
						path = directoryInfo.FullName;
					}
				}
			}
			return Path.Combine(path, "resources.xml");
		}
	}

	public string CompanyName => GetResourceString("CompanyNameShort");

	private OemResources()
	{
		LoadAndValidate();
	}

	private void LoadAndValidate()
	{
		bool flag = false;
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			if (!flag)
			{
				xmlDocument.Load(ResourceFilePath);
				flag = true;
			}
		}
		catch (Exception)
		{
		}
		_oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!_oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
	}

	public string GetResourceString(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return "[_oemResourcePackage is null ]";
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return "[" + name + "]";
		}
		return resourceItem.ResourceValueAsObject as string;
	}

	public string GetResourceBase64VectorGraphics(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return null;
		}
		VectorGraphicsResource resourceVectorGraphics = GetResourceVectorGraphics(name);
		if (resourceVectorGraphics != null)
		{
			byte[] bytes = Encoding.Default.GetBytes(resourceVectorGraphics.Value.ToString());
			return Convert.ToBase64String(bytes);
		}
		return "";
	}

	public VectorGraphicsResource GetResourceVectorGraphics(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return null;
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return null;
		}
		return resourceItem.ResourceValueAsObject as VectorGraphicsResource;
	}

	public bool GetResourceBool(string name = "")
	{
		string resourceString = GetResourceString(name);
		return string.Compare(resourceString, "true", StringComparison.InvariantCultureIgnoreCase) == 0;
	}

	public int GetResourceInt(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return 0;
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return 0;
		}
		return (int)resourceItem.ResourceValueAsObject;
	}

	public Color ColorRGB(string rName, string gName, string bName)
	{
		if (_oemResourcePackage == null)
		{
			return Color.Empty;
		}
		return Color.FromArgb(0, Instance.GetResourceInt(rName), Instance.GetResourceInt(gName), Instance.GetResourceInt(bName));
	}
}

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using VideoOS.Platform.License.OemResources;

namespace VideoOS.Platform.License;

public class OemResourceManager
{
	private static Guid CoBrandingResourcesPluginId = new Guid("7cd1166b-c00b-4ecd-8030-cf6e062a2978");

	private static string LicenseId = "CoBrandingLicense";

	private LicenseInformation _myServerLicense;

	private OemResourcePackage _package;

	private bool _loaded;

	public OemResourceManager()
	{
	}

	private void CheckForLoad()
	{
		if (_loaded)
		{
			return;
		}
		try
		{
			_myServerLicense = EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(CoBrandingResourcesPluginId, LicenseId);
			if (_myServerLicense != null)
			{
				string customData = _myServerLicense.CustomData;
				if (customData != null)
				{
					string xml = UnZipResource(customData);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					_package = new OemResourcePackage();
					_package.LoadFromXml(xmlDocument);
					_loaded = true;
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("OemResourceManager", exception);
		}
	}

	public object GetOemResource(string key)
	{
		CheckForLoad();
		if (_loaded)
		{
			try
			{
				if (_package.IsResourceItemAvailable(key))
				{
					return _package.GetResourceItem(key).ResourceValueAsObject;
				}
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("OemResourceManager:GetOemResource", exception);
			}
		}
		return null;
	}

	private static string UnZipResource(string inputStr)
	{
		Convert.FromBase64String(inputStr);
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			using MemoryStream memoryStream2 = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
			{
				memoryStream = null;
				gZipStream.CopyTo(memoryStream2);
			}
			return Encoding.Unicode.GetString(memoryStream2.ToArray());
		}
		finally
		{
			memoryStream?.Dispose();
		}
	}
}

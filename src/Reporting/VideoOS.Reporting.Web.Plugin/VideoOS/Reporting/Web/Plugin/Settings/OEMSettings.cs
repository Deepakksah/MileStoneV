using System.IO;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Reporting.Web.Plugin.Settings;

public class OEMSettings
{
	private string _companyName;

	private string _loggingDir;

	public string CompanyName
	{
		get
		{
			if (_companyName == null)
			{
				SetupFile();
			}
			return _companyName;
		}
		set
		{
			_companyName = value;
		}
	}

	public string LoggingDir
	{
		get
		{
			if (_loggingDir == null)
			{
				SetupFile();
			}
			return _loggingDir;
		}
		set
		{
			_loggingDir = value;
		}
	}

	public void SetupFile()
	{
		ApplicationFolderLocater applicationFolderLocater = new ApplicationFolderLocater();
		string text = "resources.xml";
		string filename = Path.Combine(applicationFolderLocater.GetApplicationFolder(text), text);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(filename);
		OemResourcePackage oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (oemResourcePackage.ValidateSignature())
		{
			_companyName = (string)oemResourcePackage.GetResourceItem("CompanyName").ResourceValueAsObject;
			_loggingDir = (string)oemResourcePackage.GetResourceItem("ReportWebServerLoggingDir").ResourceValueAsObject;
		}
	}
}

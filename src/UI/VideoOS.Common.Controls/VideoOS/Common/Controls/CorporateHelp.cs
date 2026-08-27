using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Help.UI;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Controls;

public class CorporateHelp : ICorporateHelp
{
	private ILogger Logger = Telemetry.GetILogger("MainApplication");

	private static object sync = new object();

	private static volatile CorporateHelp instance;

	private IntPtr _ownerPointer;

	private const string HELP_URL_FORMAT_TOPIC = "{0}://{1}:{2}/htmlhelp/managementclient/{3}/default.htm#cshid={4}";

	private const int IndexPageTopicID = 90001;

	private const int WelcomePageTopicID = 2000;

	private const string HELP_TOPICS_FILENAME = "Topics.xml";

	private const string HELP_TOPICS_RESOURCE_NAME = "VideoOS.Common.Controls.Help.Topics.xml";

	private const string _defaultServerName = "localhost";

	private const int _defaultServerPort = 80;

	private const string _defaultCultureName = "en-US";

	private readonly HelpManager _helpManager = new HelpManager(HelpDisplayer.Instance.Value, HelpUriChecker.Instance.Value);

	public static CorporateHelp Instance
	{
		[DebuggerStepThrough]
		get
		{
			if (instance != null)
			{
				return instance;
			}
			lock (sync)
			{
				if (instance != null)
				{
					return instance;
				}
				instance = new CorporateHelp();
			}
			return instance;
		}
	}

	public static bool UseOnlineHelp
	{
		get
		{
			bool flag = true;
			bool result = flag;
			string value = ConfigurationManager.AppSettings["UseOnlineHelp"];
			if (!string.IsNullOrEmpty(value) && !bool.TryParse(value, out result))
			{
				result = flag;
			}
			return result;
		}
	}

	public void Initialize(string currentCulture, IntPtr ownerPointer)
	{
		Initialize("localhost", 80, currentCulture, ownerPointer);
	}

	public void Initialize(string managementServerName, int managementServerPort, IntPtr ownerPointer)
	{
		Initialize(managementServerName, managementServerPort, "en-US", ownerPointer);
	}

	public void Initialize(string managementServerName, int managementServerPort, string currentCulture, IntPtr ownerPointer)
	{
		string offlineUrl = string.Empty;
		if (!string.IsNullOrEmpty(managementServerName))
		{
			string text = ((managementServerPort == 443) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
			offlineUrl = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/htmlhelp/managementclient/{3}/default.htm#cshid={4}", text, managementServerName, managementServerPort, "{locale}", "{topic}");
		}
		string version = OemResources.Instance.Manager.ProductVersion.Replace(" ", string.Empty);
		_ownerPointer = ownerPointer;
		_helpManager.Close();
		_helpManager.Initialize(OemResources.Instance.Manager.ApplicationHelpUrl, offlineUrl, version, currentCulture, !UseOnlineHelp, Logger);
	}

	public void ShowContentHelp()
	{
		ShowHelpTopic(2000);
	}

	public void ShowFindHelp()
	{
		ShowHelp(2000, IntPtr.Zero);
	}

	public void ShowHelpTopic(string internalTopicId)
	{
		ShowHelpTopic(internalTopicId, null);
	}

	public void ShowHelpTopic(string internalTopicId, string defaultFallbackTopicId)
	{
		ShowHelpTopic(internalTopicId, defaultFallbackTopicId, IntPtr.Zero);
	}

	public void ShowHelpTopic(string internalTopicId, string defaultFallbackTopicId, IntPtr ownerPointer)
	{
		string xml = ReadTopicsXml();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		string format = "//topic[@id='{0}']";
		format = string.Format(CultureInfo.InvariantCulture, format, internalTopicId);
		XmlNode xmlNode = xmlDocument.SelectSingleNode(format);
		if ((xmlNode == null || xmlNode.InnerText.Trim().Length == 0) && !string.IsNullOrEmpty(defaultFallbackTopicId))
		{
			format = "//topic[@id='{0}']";
			format = string.Format(CultureInfo.InvariantCulture, format, defaultFallbackTopicId);
			xmlNode = xmlDocument.SelectSingleNode(format);
		}
		if (xmlNode == null || xmlNode.InnerText.Trim().Length == 0)
		{
			ShowHelp(90001, ownerPointer);
			return;
		}
		if (!int.TryParse(xmlNode.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			result = 90001;
		}
		ShowHelp(result, ownerPointer);
	}

	private static string ReadTopicsXml()
	{
		string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Topics.xml");
		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VideoOS.Common.Controls.Help.Topics.xml");
		using StreamReader streamReader = new StreamReader(stream);
		return streamReader.ReadToEnd();
	}

	public void ShowHelpTopic(int id)
	{
		ShowHelp(id, IntPtr.Zero);
	}

	private void ShowHelp(int topicId, IntPtr ownerPointer)
	{
		try
		{
			_helpManager.ShowHelp(topicId);
		}
		catch (HelpManagerHelpNotAvailableException ex)
		{
			if (ownerPointer == IntPtr.Zero)
			{
				HelpNotAvailableDialog.ShowDialog(_ownerPointer, ex.OnlineUri);
			}
			else
			{
				HelpNotAvailableDialog.ShowDialog(ownerPointer, ex.OnlineUri);
			}
		}
		catch (HelpManagerException inner)
		{
			throw new HelpException("An error occurred when opening the help", inner);
		}
	}
}

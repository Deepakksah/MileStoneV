using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.LogServerWarningPlugin.Properties;
using VideoOS.Installer.LogServerWarningPlugin.SReg;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Installer.LogServerWarningPlugin;

public class LogServerWarningPageMoreSecureLog : IWizardCommonPageSubscription<WarningPageContent>
{
	private static Guid LegacyLogServerServiceType = new Guid("002B1B2F-0817-4592-8C62-2FE5107C160C");

	private static Guid LogServerServiceType = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public Guid Id => new Guid("4FA7A2BD-E616-42F8-818F-37DA532C53A7");

	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		if (parameters != null && parameters.ContainsKey("MANAGEMENTAPPNAME"))
		{
			string text = parameters["MANAGEMENTAPPNAME"];
			if (!string.IsNullOrEmpty(text))
			{
				ServiceInfo[] array = null;
				try
				{
					array = CreateClient(text)?.GetServices();
				}
				catch (Exception)
				{
					return false;
				}
				if (array != null)
				{
					ServiceInfo serviceInfo = array.FirstOrDefault((ServiceInfo x) => x.type == LegacyLogServerServiceType);
					ServiceInfo serviceInfo2 = array.FirstOrDefault((ServiceInfo x) => x.type == LogServerServiceType);
					if (serviceInfo != null && serviceInfo2 == null)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public WarningPageContent GetContent()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		try
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(executingAssembly.GetManifestResourceStream("VideoOS.Installer.LogServerWarningPlugin.installres.xml"));
			OemResourcePackage oemResourcePackage = new OemResourcePackage(xmlDocument);
			text = (string)oemResourcePackage.GetResourceItem("ManagementClientHelpLink").ResourceValueAsObject;
			text2 = (string)oemResourcePackage.GetResourceItem("SystemName").ResourceValueAsObject;
			text3 = (string)oemResourcePackage.GetResourceItem("ManagementClientName").ResourceValueAsObject;
		}
		catch (Exception innerException)
		{
			throw new OemResourceInvalidException("Something went wrong when trying to get oem resources", innerException);
		}
		string heading = string.Format(CultureInfo.CurrentUICulture, VideoOS_LogServer_LogServerWarningPlugin.HeadlineMoreSecureLog, text2);
		ContentLinkText details = new ContentLinkText(VideoOS_LogServer_LogServerWarningPlugin.BodyTextMoreSecureLog);
		ContentLinkText subsection = new ContentLinkText(string.Format(CultureInfo.CurrentUICulture, VideoOS_LogServer_LogServerWarningPlugin.DetailTextMoreSecureLog, text3, text3), (string.Format(CultureInfo.CurrentUICulture, VideoOS_LogServer_LogServerWarningPlugin.HelpText, text3), new Uri(text)));
		return new WarningPageContent(heading, details, VideoOS_LogServer_LogServerWarningPlugin.SubHeading, subsection, 101);
	}

	public Dictionary<string, string> CollectParameters(WarningPageContent content)
	{
		return null;
	}

	private static ServiceRegistrationServiceClient CreateClient(string managementServerUri)
	{
		string path = "/ManagementServer/ServiceRegistrationService.svc";
		Uri uri = new UriBuilder(managementServerUri)
		{
			Path = path
		}.Uri;
		Binding binding = CreateWsHttpBinding();
		EndpointAddress remoteAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(uri)));
		return new ServiceRegistrationServiceClient(binding, remoteAddress);
	}

	private static Binding CreateWsHttpBinding()
	{
		return new WSHttpBinding
		{
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			CloseTimeout = TimeSpan.FromMinutes(2.0),
			ReceiveTimeout = TimeSpan.FromMinutes(2.0),
			SendTimeout = TimeSpan.FromMinutes(2.0),
			BypassProxyOnLocal = false,
			TransactionFlow = false,
			HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max,
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			}
		};
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Xml;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Server.UACParts;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Security;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Server.CertificateRegistration;

public class ManagementServerEndpointRegistrar : ManagementServerEndpointRegistrarBase
{
	private const string IDPApplicationName = "IDP";

	private const string ManagementServerApplicationName = "ManagementServer";

	private readonly IServerResources _oemResources;

	private readonly IRegistryService _registryService;

	private string _thumbprint;

	public ManagementServerEndpointRegistrar()
		: this(new HttpSysSettingsFactory(), OemResources.Instance.Server, new FileService(), new RegistryService())
	{
	}

	public ManagementServerEndpointRegistrar(IHttpSysSettingsFactory httpSysSettingsFactory, IServerResources oemResources, IFileService fileService, IRegistryService registryService)
		: base(httpSysSettingsFactory, oemResources, fileService)
	{
		_oemResources = oemResources ?? throw new ArgumentNullException("oemResources");
		_registryService = registryService ?? throw new ArgumentNullException("registryService");
	}

	protected override Status ModifyConfiguration(bool encryptionEnabled, string thumbprint)
	{
		_thumbprint = thumbprint;
		Status status = DoWork(encryptionEnabled);
		if (status.StatusCode != StatusCode.Success)
		{
			return status;
		}
		return Status.SuccessStatus();
	}

	public Status DoWork(bool encryptionEnabled)
	{
		try
		{
			SslHelper.Status += base.SslHelperStatus;
			base.IisProgress += ManagementServerEndpointRegistrarIisProgress;
			Status status = ConfigureIis(encryptionEnabled, _thumbprint);
			if (status.StatusCode != StatusCode.Success)
			{
				return status;
			}
			status = GetIisPort(!string.IsNullOrEmpty(_thumbprint), out var port);
			if (status.StatusCode != StatusCode.Success)
			{
				return status;
			}
			status = ConfigureIDPAuthoritySetting(encryptionEnabled, port);
			if (status.StatusCode != StatusCode.Success)
			{
				return status;
			}
			status = ConfigureManagementServerWebConfig(encryptionEnabled);
			if (status.StatusCode != StatusCode.Success)
			{
				return status;
			}
			status = ConfigureRecorderApiService(encryptionEnabled, _thumbprint, base.FileService);
			if (status.StatusCode != StatusCode.Success)
			{
				return status;
			}
			status = ConfigurServerPort(base.FileService);
			if (status.StatusCode != StatusCode.Success)
			{
				return status;
			}
		}
		finally
		{
			SslHelper.Status -= base.SslHelperStatus;
			base.IisProgress -= ManagementServerEndpointRegistrarIisProgress;
		}
		return Status.SuccessStatus();
	}

	public void ManagementServerEndpointRegistrarIisProgress(object sender, IisProgressEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		switch (e.Step)
		{
		case IisProgressStep.CreateSelfSignedCertificate:
			Log("Creating self-signed certificate...");
			break;
		case IisProgressStep.CreateSslCertificateConfiguration:
			Log("Configuring certificate in Internet Information Services...");
			break;
		case IisProgressStep.DeleteSslCertificateConfiguration:
			Log("Removing certificate in Internet Information Services...");
			break;
		case IisProgressStep.CreateSslBinding:
			Log("Configuring secure binding in Internet Information Services...");
			break;
		case IisProgressStep.DeleteSslBinding:
			Log("Removing secure binding in Internet Information Services...");
			break;
		case IisProgressStep.DisableSsl:
			Log("Disabling SSL in Internet Information Services...");
			break;
		default:
			Log(FormattableString.Invariant($"Unexpected IISProgressStep : {e.Step}"));
			break;
		}
	}

	private Status ConfigureIis(bool encryptionEnabled, string thumbprint)
	{
		string configuredWebsiteId = IisHelper.GetConfiguredWebsiteId(_registryService);
		if (encryptionEnabled)
		{
			try
			{
				ManagementServerEndpointRegistrarBase.IisEnableSsl(configuredWebsiteId, thumbprint, "IDP");
			}
			catch (Exception exception)
			{
				return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, "Enable IIS failed.", "ConfigureIis");
			}
		}
		else
		{
			try
			{
				IisDisableSsl(configuredWebsiteId, "IDP");
			}
			catch (Exception exception2)
			{
				return LoggedErrorStatus(exception2, StatusCode.UnspecifiedErrorEncryption, "Disable IIS failed.", "ConfigureIis");
			}
		}
		return LoggedStatus(Status.SuccessStatus(), null, "ConfigureIis");
	}

	private Status GetIisPort(bool encryptionEnabled, out int port)
	{
		try
		{
			string configuredWebsiteId = IisHelper.GetConfiguredWebsiteId(_registryService);
			if (encryptionEnabled)
			{
				port = base.IisHelper.GetHttpsPort(configuredWebsiteId);
			}
			else
			{
				port = base.IisHelper.GetHttpPort(configuredWebsiteId);
			}
		}
		catch (Exception exception)
		{
			port = -1;
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, null, "GetIisPort");
		}
		return LoggedStatus(Status.SuccessStatus(), null, "GetIisPort");
	}

	private Status ConfigureIDPAuthoritySetting(bool encryptionEnabled, int port)
	{
		try
		{
			string host = LocalMachineGetHostName();
			UriBuilder uriBuilder = new UriBuilder(encryptionEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, host, port, "IDP");
			string path = Path.Combine(IisGetApplicationPhysicalPath("/IDP"), "appsettings.json");
			if (!base.FileService.Exists(path))
			{
				return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), "Writing authorization server configuration failed", "ConfigureIDPAuthoritySetting");
			}
			dynamic val = JsonConvert.DeserializeObject(base.FileService.ReadAllText(path));
			dynamic val2 = val["IdentityProviderSettings"]["Authority"];
			if (!((val2 == null) ? true : false))
			{
				val["IdentityProviderSettings"]["Authority"] = IdpUriParser.FormatAuthority(uriBuilder.Uri, val2.ToString());
			}
			else
			{
				val["IdentityProviderSettings"]["Authority"] = IdpUriParser.FormatAuthority(uriBuilder.Uri);
			}
			string contents = JsonConvert.SerializeObject(val, Newtonsoft.Json.Formatting.Indented);
			base.FileService.WriteAllText(path, contents);
		}
		catch (Exception exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, "Writing authorization server configuration failed", "ConfigureIDPAuthoritySetting");
		}
		return LoggedStatus(Status.SuccessStatus(), null, "ConfigureIDPAuthoritySetting");
	}

	public static string LocalMachineGetHostName()
	{
		return Dns.GetHostEntry("").HostName;
	}

	public string IisGetApplicationPhysicalPath(string applicationPath)
	{
		return new IisHelper(NullLogger.Instance).GetPhysicalPath(IisHelper.GetConfiguredWebsiteId(_registryService), applicationPath);
	}

	public Status ConfigureManagementServerWebConfig(bool encryptionEnabled, string webConfigFileName = "Web.config")
	{
		try
		{
			string text = Path.Combine(IisGetApplicationPhysicalPath("/ManagementServer"), webConfigFileName);
			if (!base.FileService.Exists(text))
			{
				return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), FormattableString.Invariant($"Updating Management server Web config failed - Could not find {text}"), "ConfigureManagementServerWebConfig");
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text);
			foreach (XmlElement childNode in xmlDocument.DocumentElement["system.serviceModel"]["services"].ChildNodes)
			{
				XmlNodeList elementsByTagName = childNode.GetElementsByTagName("endpoint");
				XmlElement xmlElement2 = null;
				XmlElement xmlElement3 = null;
				foreach (XmlElement item in elementsByTagName)
				{
					string attribute = item.GetAttribute("bindingConfiguration");
					if ("OAuthAuthentication".Equals(attribute, StringComparison.OrdinalIgnoreCase))
					{
						xmlElement2 = item;
					}
					else if ("OAuthAuthenticationTransport".Equals(attribute, StringComparison.OrdinalIgnoreCase))
					{
						xmlElement3 = item;
					}
				}
				if (encryptionEnabled && xmlElement2 != null && xmlElement3 != null)
				{
					XmlComment newChild = xmlDocument.CreateComment(xmlElement2.OuterXml);
					xmlElement2.ParentNode.ReplaceChild(newChild, xmlElement2);
				}
				else
				{
					if (encryptionEnabled || xmlElement3 == null || xmlElement2 != null)
					{
						continue;
					}
					XmlComment xmlComment = FindCommentedEndpoint(childNode);
					if (xmlComment == null)
					{
						return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), "Expected to find a commented endpoint element in web.config, but it was not there", "ConfigureManagementServerWebConfig");
					}
					StringReader stringReader = null;
					try
					{
						stringReader = new StringReader(xmlComment.Value.Trim());
						using XmlReader xmlReader = XmlReader.Create(stringReader);
						if (!xmlReader.Read())
						{
							return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), "Unable to read the commented endpoint element in web.config", "ConfigureManagementServerWebConfig");
						}
						XmlNode newChild2 = xmlDocument.ReadNode(xmlReader);
						xmlComment.ParentNode.ReplaceChild(newChild2, xmlComment);
					}
					finally
					{
						stringReader?.Dispose();
					}
				}
			}
			xmlDocument.Save(text);
		}
		catch (Exception exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, "Updating Management server Web config failed", "ConfigureManagementServerWebConfig");
		}
		return LoggedStatus(Status.SuccessStatus(), null, "ConfigureManagementServerWebConfig");
	}

	private static XmlComment FindCommentedEndpoint(XmlElement service)
	{
		foreach (object childNode in service.ChildNodes)
		{
			if (!(childNode is XmlComment xmlComment))
			{
				continue;
			}
			string xml = xmlComment.InnerText.Trim();
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				if (xmlDocument.FirstChild is XmlElement xmlElement)
				{
					string attribute = xmlElement.GetAttribute("bindingConfiguration");
					if ("OAuthAuthentication".Equals(attribute, StringComparison.OrdinalIgnoreCase))
					{
						return xmlComment;
					}
				}
			}
			catch (XmlException)
			{
			}
		}
		return null;
	}

	private Status ConfigureRecorderApiService(bool encryptionEnabled, string thumbprint, IFileService fileService)
	{
		if (encryptionEnabled)
		{
			Log("Configuring encryption for recording server communication....");
		}
		else
		{
			Log("Disabling encryption for recording server communication...");
		}
		ServerConfig serverConfig = new ServerConfig(fileService, null);
		List<ISslConfiguration> list;
		using (IHttpSysConfigReader httpSysConfigReader = CreateHttpSysConfigReader())
		{
			try
			{
				list = (from c in httpSysConfigReader.ReadSslConfigurations()
					where c.Port == serverConfig.RecorderCommunicationApiPort || c.ApplicationId == WellKnownAppIds.ServerApiService
					select c).ToList();
			}
			catch (Exception exception)
			{
				return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, "Reading the current encryption configuration failed.", "ConfigureRecorderApiService");
			}
		}
		using (IHttpSysConfig httpSysConfig = CreateHttpSysConfig())
		{
			foreach (ISslConfiguration item in list)
			{
				try
				{
					httpSysConfig.DeleteSslConfiguration(item);
				}
				catch (Win32Exception exception2)
				{
					return LoggedErrorStatus(exception2, StatusCode.UnspecifiedErrorEncryption, "Applying encryption settings failed", "ConfigureRecorderApiService");
				}
			}
			if (encryptionEnabled)
			{
				ISslConfiguration configuration = CreateDefaultConfigFromCertificate(IPAddress.Any, serverConfig.RecorderCommunicationApiPort, WellKnownAppIds.ServerApiService, StoreName.My, thumbprint);
				try
				{
					httpSysConfig.EnforceSslConfiguration(configuration);
				}
				catch (Win32Exception exception3)
				{
					return LoggedErrorStatus(exception3, StatusCode.UnspecifiedErrorEncryption, "Applying encryption settings failed", "ConfigureRecorderApiService");
				}
			}
			SecurityIdentifier securityIdentifier = null;
			try
			{
				securityIdentifier = FindServerServiceUser();
			}
			catch (Exception exception4)
			{
				return LoggedErrorStatus(exception4, StatusCode.UnspecifiedErrorEncryption, "Find Server service user failed", "ConfigureRecorderApiService");
			}
			if (securityIdentifier == null)
			{
				return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), "Server service user not found", "ConfigureRecorderApiService");
			}
			try
			{
				UrlPrefix prefix = UrlPrefix.CreateStrongWildcard(encryptionEnabled ? UrlScheme.Https : UrlScheme.Http, serverConfig.RecorderCommunicationApiPort, "/");
				UrlReservationAccessControlEntry urlReservationAccessControlEntry = new UrlReservationAccessControlEntry(securityIdentifier, allowListen: true, allowDelegate: false);
				httpSysConfig.EnforceUrlReservation(prefix, urlReservationAccessControlEntry);
			}
			catch (Win32Exception exception5)
			{
				return LoggedErrorStatus(exception5, StatusCode.UnspecifiedErrorEncryption, "Enforce URL reservation failed", "ConfigureRecorderApiService");
			}
		}
		return LoggedStatus(Status.SuccessStatus(), null, "ConfigureRecorderApiService");
	}

	private Status ConfigurServerPort(IFileService fileService)
	{
		ServerConfig serverConfig = new ServerConfig(fileService, null);
		using (IHttpSysConfig httpSysConfig = CreateHttpSysConfig())
		{
			SecurityIdentifier securityIdentifier = null;
			try
			{
				securityIdentifier = FindServerServiceUser();
			}
			catch (Exception exception)
			{
				return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, "Find Server service user failed", "ConfigurServerPort");
			}
			if (securityIdentifier == null)
			{
				return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), "Server service user not found", "ConfigurServerPort");
			}
			try
			{
				UrlPrefix prefix = UrlPrefix.CreateStrongWildcard(UrlScheme.Http, serverConfig.ServerPort, "/");
				UrlReservationAccessControlEntry urlReservationAccessControlEntry = new UrlReservationAccessControlEntry(securityIdentifier, allowListen: true, allowDelegate: false);
				httpSysConfig.EnforceUrlReservation(prefix, urlReservationAccessControlEntry);
			}
			catch (Win32Exception exception2)
			{
				return LoggedErrorStatus(exception2, StatusCode.UnspecifiedErrorEncryption, "Enforce URL reservation failed", "ConfigurServerPort");
			}
		}
		return LoggedStatus(Status.SuccessStatus(), null, "ConfigurServerPort");
	}

	public static IHttpSysConfigReader CreateHttpSysConfigReader()
	{
		return HttpConfigFactory.Instance.CreateConfigReader();
	}

	public static IHttpSysConfig CreateHttpSysConfig()
	{
		return HttpConfigFactory.Instance.CreateConfig();
	}

	public static ISslConfiguration CreateDefaultConfigFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, string thumbprint)
	{
		return SslConfigurationFactory.Instance.GetDefaultConfigurationFromCertificate(address, port, applicationId, certificateStore, thumbprint);
	}

	public SecurityIdentifier FindServerServiceUser()
	{
		string text = Account.GetServiceLogOnUser(_oemResources.ServiceName);
		if (text != null)
		{
			if (text.StartsWith(".\\", StringComparison.Ordinal))
			{
				text = Environment.MachineName + text.TrimStart('.');
			}
			return new NTAccount(text).Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
		}
		return null;
	}

	private void Log(string logEntry)
	{
		base.Logger?.LogInfo(logEntry);
	}
}

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Server.UACParts;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common;

namespace VideoOS.Server.CertificateRegistration;

public class ParametersHandler : IRegistrationParametersHandler
{
	private readonly IFileService _fileService;

	private readonly string _configFilePath;

	private readonly string _clientName;

	public ParametersHandler(IFileService fileService, IServerResources oemServerResources, string clientName)
	{
		_fileService = fileService ?? throw new ArgumentNullException("fileService");
		if (oemServerResources == null)
		{
			throw new ArgumentNullException("oemServerResources");
		}
		_configFilePath = ResolveConfigFilePath(oemServerResources.ConfigFilePath);
		_clientName = clientName ?? throw new ArgumentNullException("clientName");
	}

	public VideoOS.CertificateRegistration.Common.RegistrationParameters ReadRegistrationParameters(Uri authorizationServerAddress)
	{
		return new VideoOS.CertificateRegistration.Common.RegistrationParameters(GetClientId(_configFilePath), _clientName, ScopeConstants.ServerServiceScopes, authorizationServerAddress);
	}

	public bool WriteRegistrationParameters(VideoOS.CertificateRegistration.Common.RegistrationParameters parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		using (Stream stream = GetConfigFileStream(_configFilePath))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.Load(stream);
			xmlDocument["server"]["ClientRegistrationId"].InnerText = parameters.ClientId.ToString();
			xmlDocument["server"]["WebApiConfig"]["AuthorizationServerUri"].InnerText = parameters.AuthorizationServerAddress.ToString();
			stream.SetLength(0L);
			xmlDocument.Save(stream);
		}
		return true;
	}

	private string ResolveConfigFilePath(string installedFilePath)
	{
		try
		{
			string fileName = Path.GetFileName(installedFilePath);
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string path = directoryName.Remove(0, Path.GetFullPath(Path.Combine(directoryName, "..\\..\\..\\..")).Length + 1);
			int num = directoryName.IndexOf("\\Corporate\\", StringComparison.OrdinalIgnoreCase);
			if (num > 0)
			{
				string text = Path.Combine(directoryName.Substring(0, num + "\\Corporate".Length), "VideoOS.Server.Service", path, fileName);
				if (_fileService.Exists(text))
				{
					return text;
				}
			}
		}
		catch
		{
		}
		return installedFilePath;
	}

	private Stream GetConfigFileStream(string serverConfigFilePath)
	{
		if (serverConfigFilePath == null)
		{
			throw new ArgumentNullException("serverConfigFilePath");
		}
		if (!_fileService.Exists(serverConfigFilePath))
		{
			throw new FileNotFoundException("Cannot find the configuration file", serverConfigFilePath);
		}
		return _fileService.Open(serverConfigFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
	}

	private Guid GetClientId(string serverConfigFilePath)
	{
		using Stream stream = GetConfigFileStream(serverConfigFilePath);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(stream);
		stream.Seek(0L, SeekOrigin.Begin);
		XmlElement xmlElement = xmlDocument["server"]["ClientRegistrationId"];
		if (string.IsNullOrEmpty(xmlElement.InnerText))
		{
			return Guid.NewGuid();
		}
		if (Guid.TryParse(xmlElement.InnerText, out var result))
		{
			return result;
		}
		throw new RegistrationFailedException("The management server configuration file contains an invalid registration id");
	}
}

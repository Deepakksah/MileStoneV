using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServiceHost.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.ServiceHost.EventServer;

public sealed class CertificateRepository : ICertificateRepository
{
	private readonly string _fileDirectory;

	private readonly IFileService _fileService;

	private readonly ICommunicationServiceHostLog _eventServerCommunicationServiceHostLog;

	private static bool? _hasCertificate;

	private static bool? _hasServerCertificate;

	private readonly XmlReaderSettings _secureXmlReaderSettings = new XmlReaderSettings
	{
		CheckCharacters = true,
		DtdProcessing = DtdProcessing.Prohibit,
		MaxCharactersFromEntities = 1024L
	};

	public bool HasCertificate => GetHasCertificate();

	public bool HasServerCertificate => GetHasServerCertificate();

	private bool GetHasCertificate()
	{
		if (!_hasCertificate.HasValue)
		{
			try
			{
				string path = Path.Combine(_fileDirectory, "config", "ServiceEndpoints.xml");
				XmlDocument xmlDocument = new XmlDocument();
				using Stream input = _fileService.Open(path, FileMode.Open, FileAccess.Read);
				xmlDocument.Load(XmlReader.Create(input, _secureXmlReaderSettings));
				XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
				if (xPathNavigator != null)
				{
					XPathNavigator xPathNavigator2 = xPathNavigator.CreateNavigator().SelectSingleNode("/eventserverconfig/CertificateEnabled");
					_hasCertificate = xPathNavigator2 != null && Convert.ToBoolean(xPathNavigator2.InnerXml, CultureInfo.InvariantCulture);
				}
			}
			catch (Exception logError)
			{
				_eventServerCommunicationServiceHostLog.Error(this, "Error reading endpoint configuration file. Verify it exists and has the proper format", logError);
				_hasCertificate = false;
			}
		}
		if (_hasCertificate.HasValue)
		{
			return _hasCertificate.Value;
		}
		return false;
	}

	private bool GetHasServerCertificate()
	{
		if (!_hasServerCertificate.HasValue)
		{
			try
			{
				string path = Path.Combine(_fileDirectory, "config", "ServiceEndpoints.xml");
				XmlDocument xmlDocument = new XmlDocument();
				using Stream input = _fileService.Open(path, FileMode.Open, FileAccess.Read);
				xmlDocument.Load(XmlReader.Create(input, _secureXmlReaderSettings));
				XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
				if (xPathNavigator != null)
				{
					XPathNavigator xPathNavigator2 = xPathNavigator.CreateNavigator().SelectSingleNode("/eventserverconfig/ServerEndpointsCertificateHash");
					_hasServerCertificate = xPathNavigator2 != null && !string.IsNullOrEmpty(xPathNavigator2.InnerXml);
				}
			}
			catch (Exception logError)
			{
				_eventServerCommunicationServiceHostLog.Error(this, "Error reading endpoint configuration file. Verify it exists and has the proper format", logError);
				_hasServerCertificate = false;
			}
		}
		if (_hasServerCertificate.HasValue)
		{
			return _hasServerCertificate.Value;
		}
		return false;
	}

	public CertificateRepository(string fileDirectory, IFileService fileService, ICommunicationServiceHostLog eventServerCommunicationServiceHostLog)
	{
		fileDirectory.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.EventServer\\CertificateRepository.cs", 91);
		fileService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.EventServer\\CertificateRepository.cs", 92);
		eventServerCommunicationServiceHostLog.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.EventServer\\CertificateRepository.cs", 93);
		_fileDirectory = fileDirectory;
		_fileService = fileService;
		_eventServerCommunicationServiceHostLog = eventServerCommunicationServiceHostLog;
	}
}

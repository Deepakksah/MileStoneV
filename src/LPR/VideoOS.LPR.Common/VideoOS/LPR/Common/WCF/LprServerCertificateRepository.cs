using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices;
using VideoOS.LPR.Common.OEM;
using VideoOS.ServiceHost.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.LPR.Common.WCF;

public sealed class LprServerCertificateRepository : ICertificateRepository
{
	public const string ConfigurationFileDirectory = "config";

	public const string ConfigurationFileName = "ServiceEndpoints.xml";

	private const string CertificateNode = "/lprserverconfig/CertificateEnabled";

	private readonly IFileService _fileService;

	private readonly ILogger _lprLog;

	public bool HasCertificate => GetHasCertificate();

	public LprServerCertificateRepository(IFileService fileService, ILogger lprLog)
	{
		fileService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\WCF\\LprServerCertificateRepository.cs", 23);
		lprLog.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\WCF\\LprServerCertificateRepository.cs", 24);
		_lprLog = lprLog;
		_fileService = fileService;
	}

	private bool GetHasCertificate()
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument
			{
				XmlResolver = null
			};
			string path = Path.Combine(OemResources.DataDirectory, "config", "ServiceEndpoints.xml");
			using Stream input = _fileService.Open(path, FileMode.Open, FileAccess.Read);
			XmlReader reader = XmlReader.Create(input, new XmlReaderSettings
			{
				XmlResolver = null
			});
			xmlDocument.Load(reader);
			return Convert.ToBoolean(xmlDocument.CreateNavigator().CreateNavigator().SelectSingleNode("/lprserverconfig/CertificateEnabled")
				.InnerXml, CultureInfo.InvariantCulture);
			}
			catch (Exception exception)
			{
				_lprLog.LogError(exception, "Error reading endpoint configuration file. Verify it exists and has the proper format");
				throw;
			}
		}
	}

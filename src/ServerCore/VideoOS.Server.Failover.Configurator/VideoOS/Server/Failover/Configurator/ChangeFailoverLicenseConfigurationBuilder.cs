using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class ChangeFailoverLicenseConfigurationBuilder : IConfigurationBuilder
{
	private readonly string _safekitInstallFolder;

	private readonly string _licenseFilePath;

	private readonly ILogger _log;

	public ChangeFailoverLicenseConfigurationBuilder(string safekitInstallFolder, string licenseFilePath, ILogger log)
	{
		_safekitInstallFolder = safekitInstallFolder;
		_licenseFilePath = licenseFilePath;
		_log = log;
	}

	public IEnumerable<IConfigurationStep> Build()
	{
		yield return new ChangeFailoverLicense(_log, _licenseFilePath, _safekitInstallFolder);
	}
}

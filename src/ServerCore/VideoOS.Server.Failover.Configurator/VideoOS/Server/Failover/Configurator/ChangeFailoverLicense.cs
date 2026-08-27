using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class ChangeFailoverLicense : IConfigurationStep
{
	private const string SafekitLicenseFolder = "conf";

	private const string SafekitDefaultLicenseFileName = "license.txt";

	private readonly ILogger _log;

	private readonly string _licenseFilePath;

	private readonly string _safekitInstallFolder;

	public ChangeFailoverLicense(ILogger log, string licenseFilePath, string safekitInstallFolder)
	{
		_log = log;
		_licenseFilePath = licenseFilePath;
		_safekitInstallFolder = safekitInstallFolder;
	}

	public async Task ExecuteAsync()
	{
		await Task.Factory.StartNew(CopyLicenseFile);
	}

	private void CopyLicenseFile()
	{
		if (string.IsNullOrEmpty(_licenseFilePath))
		{
			_log.LogInformation("[ChangeFailoverLicense] No license file specified");
			return;
		}
		string text = Path.Combine(_safekitInstallFolder, "conf");
		if (!Directory.Exists(text))
		{
			throw new DirectoryNotFoundException("License directory not found");
		}
		try
		{
			string text2 = Path.Combine(text, "license.txt");
			if (File.Exists(text2))
			{
				string destFileName = Path.Combine(text, $"license_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
				File.Move(text2, destFileName);
			}
			File.Copy(_licenseFilePath, text2, overwrite: true);
			_log.LogInformation("[ChangeFailoverLicense] License file '{LicenseFilePath}' copied to '{LicenseFileDestination}' and renamed to '{LicenseFileName}'", Path.GetFileName(_licenseFilePath), text, "license.txt");
		}
		catch (UnauthorizedAccessException ex)
		{
			_log.LogError("[ChangeFailoverLicense] {ExceptionMessage}", ex.Message);
			throw;
		}
		catch (FileNotFoundException ex2)
		{
			_log.LogError("[ChangeFailoverLicense] {ExceptionMessage}", ex2.Message);
			throw;
		}
		catch (IOException ex3)
		{
			_log.LogError("[ChangeFailoverLicense] {ExceptionMessage}", ex3.Message);
			throw;
		}
	}
}

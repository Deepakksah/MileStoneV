using System;
using System.IO;
using System.Reflection;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public class DownloadManagerSetManagementServerRegistrar : SetManagementServerRegistrarBase
{
	private readonly Func<string, IIniFile> _iniFileFactory;

	public DownloadManagerSetManagementServerRegistrar()
		: this(null, (string filename) => new IniFile(filename), new TimeSpan(0, 3, 0), new ServiceControllerFactory(), new WebRequestFactory())
	{
	}

	public DownloadManagerSetManagementServerRegistrar(string serviceName, Func<string, IIniFile> iniFileFactory, TimeSpan timeout, IServiceControllerFactory serviceControllerFactory, IWebRequestFactory webRequestFactory)
		: base(serviceName, timeout, serviceControllerFactory, webRequestFactory)
	{
		_iniFileFactory = iniFileFactory ?? throw new ArgumentNullException("iniFileFactory");
	}

	protected override Status Execute(Uri newManagementServerAddress)
	{
		Status status = UpdateDownloadManagerUrlFiles(newManagementServerAddress);
		return LoggedStatus(status, FormattableString.Invariant($"Management Server Address section successfully updated ({1})"), "Execute");
	}

	private Status UpdateDownloadManagerUrlFiles(Uri newManagementServerAddress)
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"Updating Management Server Address in URL files, setting the address to: {newManagementServerAddress}"));
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		UpdateAddressInURLFile(Path.Combine(directoryName, "Installation.url"), newManagementServerAddress);
		UpdateAddressInURLFile(Path.Combine(directoryName, "AdminInstallation.url"), newManagementServerAddress);
		return Status.SuccessStatus();
	}

	private void UpdateAddressInURLFile(string path, Uri websiteAddress)
	{
		IIniFile iniFile = _iniFileFactory(path);
		iniFile.WriteValue("InternetShortcut", "URL", new UriBuilder(iniFile.ReadValue("InternetShortcut", "URL"))
		{
			Host = websiteAddress.Host,
			Port = websiteAddress.Port,
			Scheme = websiteAddress.Scheme
		}.Uri.AbsoluteUri);
	}
}

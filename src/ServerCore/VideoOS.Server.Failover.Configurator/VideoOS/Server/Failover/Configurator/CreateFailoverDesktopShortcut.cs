using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class CreateFailoverDesktopShortcut : IConfigurationStep
{
	private readonly bool _useHttps;

	private readonly string _hostMachineFqdn;

	private readonly string _iconPath;

	public CreateFailoverDesktopShortcut(bool useHttps, string hostMachineFqdn, string iconPath)
	{
		_useHttps = useHttps;
		_hostMachineFqdn = hostMachineFqdn;
		_iconPath = iconPath;
	}

	public async Task ExecuteAsync()
	{
		await Task.Factory.StartNew(CreateDesktopShortcut);
	}

	private void CreateDesktopShortcut()
	{
		using StreamWriter streamWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory) + "\\" + ProductLocationHelper.WebConsoleIconName);
		string webConsoleAddress = WebConsoleHelper.GetWebConsoleAddress(_hostMachineFqdn, _useHttps);
		string value = string.Format(CultureInfo.InvariantCulture, "URL={0}", webConsoleAddress);
		streamWriter.WriteLine("[InternetShortcut]");
		streamWriter.WriteLine(value);
		streamWriter.WriteLine("IconIndex=0");
		if (!File.Exists(_iconPath))
		{
			using FileStream outputStream = new FileStream(_iconPath, FileMode.Create);
			OemResources.Instance.ManagementFailoverServer.FailoverManagementServerDesktopIcon.Save(outputStream);
		}
		streamWriter.WriteLine("IconFile=" + _iconPath);
	}
}

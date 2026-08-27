using System;
using System.IO;
using System.Threading.Tasks;

namespace VideoOS.Server.Failover.Configurator;

public class DeleteFailoverDesktopShortcut : IConfigurationStep
{
	public async Task ExecuteAsync()
	{
		await Task.Factory.StartNew(DeleteDesktopShortcut);
	}

	private void DeleteDesktopShortcut()
	{
		File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), ProductLocationHelper.WebConsoleIconName));
	}
}

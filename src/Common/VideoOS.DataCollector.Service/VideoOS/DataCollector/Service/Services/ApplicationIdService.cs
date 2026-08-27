using Microsoft.Win32;
using VideoOS.DataCollector.Service.Common.Exceptions;

namespace VideoOS.DataCollector.Service.Services;

public class ApplicationIdService : IApplicationIdService
{
	private string _applicationId = string.Empty;

	public string ApplicationId
	{
		get
		{
			if (!string.IsNullOrEmpty(_applicationId))
			{
				return _applicationId;
			}
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
			{
				RegistryKey registryKey2 = registryKey.OpenSubKey("Software\\VideoOS\\Installation\\DataCollector");
				object obj = null;
				if (registryKey2 != null)
				{
					obj = registryKey2.GetValue("DataCollectorAppId", string.Empty);
				}
				_applicationId = (obj as string) ?? throw new ApplicationIdStringNotFoundException("Application identification string not found in registry");
			}
			return _applicationId;
		}
	}
}

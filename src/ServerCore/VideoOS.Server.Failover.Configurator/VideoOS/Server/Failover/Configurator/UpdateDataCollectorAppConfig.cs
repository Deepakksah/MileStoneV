using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class UpdateDataCollectorAppConfig : IConfigurationStep
{
	private readonly string localFqdn;

	private readonly string virtualIP;

	private readonly string configFilePath;

	private ILogger Log { get; }

	public bool Deconfigure { get; set; }

	public UpdateDataCollectorAppConfig(string localFqdn, string virtualIP, string configFilePath, ILogger log)
	{
		this.localFqdn = localFqdn;
		this.virtualIP = virtualIP;
		this.configFilePath = configFilePath;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		await Task.Run(delegate
		{
			try
			{
				UpdateAppConfig();
			}
			catch (Exception ex)
			{
				Log.LogError("UpdateDataCollectorAppConfig failed with exception: {ExceptionMessage} {Exception}", ex.Message, ex);
				throw;
			}
		});
	}

	private void UpdateAppConfig()
	{
		JObject jObject = JObject.Parse(File.ReadAllText(configFilePath));
		if (!Deconfigure)
		{
			jObject["ClusterSettings"]["ClusterAddress"] = localFqdn;
			jObject["ClusterSettings"]["ClusterApplicationId"] = FailoverConfigurationHelper.GuidFromString(virtualIP);
		}
		else
		{
			jObject["ClusterSettings"]["ClusterAddress"] = string.Empty;
			jObject["ClusterSettings"]["ClusterApplicationId"] = string.Empty;
		}
		string contents = jObject.ToString(Formatting.Indented);
		File.WriteAllText(configFilePath, contents);
		FailoverConfigurationHelper.RestartWindowsServiceIfRunning(OemResources.Instance.Server.DataCollectorServiceName);
	}
}

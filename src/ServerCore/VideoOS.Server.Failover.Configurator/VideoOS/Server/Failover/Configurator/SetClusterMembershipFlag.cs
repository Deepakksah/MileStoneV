using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class SetClusterMembershipFlag : IConfigurationStep
{
	public string ServerConfigPath { get; set; }

	public bool IsClusterMember { get; set; }

	public string ClusterLocalWebserverUriString { get; set; }

	public string ClusterModuleName { get; set; }

	private ILogger Log { get; }

	private Runspace Runspace { get; }

	public string SafekitVirtualAddress { get; public set; }

	public SetClusterMembershipFlag(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\SetClusterMembership.ps1");
		powerShell.AddParameter("-ServerConfigPath", ServerConfigPath);
		powerShell.AddParameter("-IsClusterMember", IsClusterMember.ToString());
		powerShell.AddParameter("-ClusterLocalWebserverUri", ClusterLocalWebserverUriString);
		powerShell.AddParameter("-ManagementServerServiceName", OemResources.Instance.Server.ManagementServerServiceName);
		powerShell.AddParameter("-ClusterModuleName", ClusterModuleName);
		powerShell.AddParameter("-VirtualAddress", SafekitVirtualAddress);
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}

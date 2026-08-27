using System;
using System.Globalization;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public static class PowershellHelper
{
	public static async Task InvokeAndThrowIfErrorAsync(PowerShell powershell, ILogger log)
	{
		PSDataCollection<PSObject> pSDataCollection = new PSDataCollection<PSObject>();
		pSDataCollection.DataAdded += delegate(object s, DataAddedEventArgs e)
		{
			LogProgress<PSObject>(log, "output", s, e);
		};
		powershell.Streams.Information.DataAdded += delegate(object s, DataAddedEventArgs e)
		{
			LogProgress<InformationRecord>(log, "information", s, e);
		};
		powershell.Streams.Warning.DataAdded += delegate(object s, DataAddedEventArgs e)
		{
			LogProgress<WarningRecord>(log, "warning", s, e);
		};
		powershell.Streams.Error.DataAdded += delegate(object s, DataAddedEventArgs e)
		{
			LogProgress<ErrorRecord>(log, "error", s, e);
		};
		powershell.Streams.Verbose.DataAdded += delegate(object s, DataAddedEventArgs e)
		{
			LogProgress<VerboseRecord>(log, "verbose", s, e);
		};
		powershell.Streams.Debug.DataAdded += delegate(object s, DataAddedEventArgs e)
		{
			LogProgress<DebugRecord>(log, "debug", s, e);
		};
		IAsyncResult asyncResult = powershell.BeginInvoke<PSObject, PSObject>(null, pSDataCollection);
		await Task.Factory.FromAsync(asyncResult, (Func<IAsyncResult, PSDataCollection<PSObject>>)powershell.EndInvoke);
		if (powershell.HadErrors)
		{
			throw new ScriptErrorException(powershell.Streams.Error);
		}
	}

	private static void LogProgress<T>(ILogger log, string description, object sender, DataAddedEventArgs e)
	{
		string text = Convert.ToString((sender as PSDataCollection<T>)[e.Index], CultureInfo.InvariantCulture)?.Trim();
		if (!string.IsNullOrWhiteSpace(text) && (text.Length != 1 || char.IsLetterOrDigit(text[0])))
		{
			log.LogInformation("Command {Description}: {LogMessage}", description, text);
		}
	}
}

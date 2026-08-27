#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DataCollectorRegisteredServicesResolver : IDataCollectorRegisteredServicesResolver
{
	private readonly object locker = new object();

	private Dictionary<string, Task<int>> Tasks { get; set; }

	private Dictionary<string, List<Guid>> HostNameOnServers { get; set; }

	private IDataCollectorOnRegisteredServicesDataProvider Provider { get; set; }

	public DataCollectorRegisteredServicesResolver()
		: this(new DataCollectorOnRegisteredServicesDataProvider(GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext()), new Dictionary<string, Task<int>>(), new Dictionary<string, List<Guid>>())
	{
	}

	public DataCollectorRegisteredServicesResolver(IDataCollectorOnRegisteredServicesDataProvider provider, Dictionary<string, Task<int>> tasks, Dictionary<string, List<Guid>> hostNameOnServers)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (hostNameOnServers == null)
		{
			throw new ArgumentNullException("hostNameOnServers");
		}
		Provider = provider;
		Tasks = tasks;
		HostNameOnServers = hostNameOnServers;
	}

	public void DoResolve()
	{
		lock (locker)
		{
			try
			{
				CleanUpTasks(Tasks);
				EnqueueResolveService(Provider.QueryRegisteredServersWithoutDataCollectorId(), HostNameOnServers, Tasks, Provider);
				do
				{
					Trace.WriteLine("SystemMonitorManager: Starting tasks: " + Tasks.Count.ToString(CultureInfo.InvariantCulture));
					System.Threading.Tasks.Task[] tasks = Tasks.Select((KeyValuePair<string, Task<int>> i) => i.Value).ToArray();
					System.Threading.Tasks.Task.WaitAny(tasks, 1000);
					KeyValuePair<string, Task<int>> completedTask = Tasks.FirstOrDefault((KeyValuePair<string, Task<int>> i) => i.Value.IsCompleted);
					if (completedTask.Key != null && completedTask.Value != null)
					{
						string key = completedTask.Key;
						Tasks.Remove(key);
						UpdateDatabase(key, HostNameOnServers, completedTask, Provider);
					}
				}
				while (Tasks.Count > 0);
			}
			catch (Exception)
			{
			}
		}
	}

	private static void UpdateDatabase(string hostName, Dictionary<string, List<Guid>> hostNameOnServers, KeyValuePair<string, Task<int>> completedTask, IDataCollectorOnRegisteredServicesDataProvider provider)
	{
		if (hostNameOnServers.ContainsKey(hostName))
		{
			List<Guid> list = hostNameOnServers[hostName];
			if (completedTask.Value.Result != 0)
			{
				foreach (Guid item in list)
				{
					Guid guid = item;
					Trace.WriteLine("SystemMonitorManager: Update datacollectorId for service: " + guid.ToString());
					provider.UpdateDataCollectorsForRegisteredServers(item, completedTask.Value.Result);
				}
			}
		}
		hostNameOnServers.Remove(hostName);
	}

	private static void CleanUpTasks(Dictionary<string, Task<int>> tasks)
	{
		string[] array = (from i in tasks
			where i.Value.IsCompleted
			select i.Key).ToArray();
		foreach (string key in array)
		{
			tasks.Remove(key);
		}
	}

	private static void EnqueueResolveService(RegisteredServer[] unresolvedServices, Dictionary<string, List<Guid>> hostNameOnServers, Dictionary<string, Task<int>> tasks, IDataCollectorOnRegisteredServicesDataProvider provider)
	{
		foreach (RegisteredServer registeredServer in unresolvedServices)
		{
			ServerAddress[] addresses = registeredServer.Addresses;
			for (int j = 0; j < addresses.Length; j++)
			{
				string hostNameFromAddress = GetHostNameFromAddress(addresses[j]);
				if (!tasks.ContainsKey(hostNameFromAddress))
				{
					hostNameOnServers.Add(hostNameFromAddress, new List<Guid> { registeredServer.IDServer });
					tasks.Add(hostNameFromAddress, provider.GetIdFromDataCollector(hostNameFromAddress));
				}
				else if (!hostNameOnServers[hostNameFromAddress].Contains(registeredServer.IDServer))
				{
					hostNameOnServers[hostNameFromAddress].Add(registeredServer.IDServer);
				}
			}
		}
	}

	private static string GetHostNameFromAddress(ServerAddress serverAddress)
	{
		return new Uri(serverAddress.URL).Host;
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RecordingServerFolder : IConfigurationItem
{
	public ICollection<RecordingServer> RecordingServers
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<RecordingServer> list = new List<RecordingServer>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "RecordingServer")
				{
					list.Add(new RecordingServer(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public RecordingServerFolder(ServerId serverId, ConfigurationItem recordingServerFolder)
		: base(serverId, recordingServerFolder)
	{
	}

	public RecordingServerFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RecordingServerFolder(FQID fqid)
		: base(fqid)
	{
	}

	public RecordingServerFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/RecordingServerFolder")
	{
	}

	public ServerTask RemoveRecordingServer(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveRecordingServer");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveRecordingServer"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServerFolder", "Unable to call method 'RemoveRecordingServer':" + ex.Message);
		}
		return null;
	}

	public RemoveRecordingServerServerTask RemoveRecordingServer()
	{
		try
		{
			return new RemoveRecordingServerServerTask(base.Wrapper.ExecuteMethod("RemoveRecordingServer"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServerFolder", "Unable to call method 'RemoveRecordingServer':" + ex.Message);
		}
		return null;
	}
}

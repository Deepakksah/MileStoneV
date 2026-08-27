using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FailoverGroupFolder : IConfigurationItem
{
	public ICollection<FailoverGroup> FailoverGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<FailoverGroup> list = new List<FailoverGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "FailoverGroup")
				{
					list.Add(new FailoverGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ICollection<FailoverRecorder> FailoverRecorders
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<FailoverRecorder> list = new List<FailoverRecorder>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "FailoverRecorder")
				{
					list.Add(new FailoverRecorder(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public FailoverGroupFolder(ServerId serverId, ConfigurationItem failoverGroupFolder)
		: base(serverId, failoverGroupFolder)
	{
	}

	public FailoverGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FailoverGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public FailoverGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/FailoverGroupFolder")
	{
	}

	public ServerTask AddFailoverGroup(string name, string description)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddFailoverGroup");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddFailoverGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "FailoverGroupFolder", "Unable to call method 'AddFailoverGroup':" + ex.Message);
		}
		return null;
	}

	public AddFailoverGroupServerTask AddFailoverGroup()
	{
		try
		{
			return new AddFailoverGroupServerTask(base.Wrapper.ExecuteMethod("AddFailoverGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "FailoverGroupFolder", "Unable to call method 'AddFailoverGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveFailoverGroup(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveFailoverGroup");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveFailoverGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "FailoverGroupFolder", "Unable to call method 'RemoveFailoverGroup':" + ex.Message);
		}
		return null;
	}

	public RemoveFailoverGroupServerTask RemoveFailoverGroup()
	{
		try
		{
			return new RemoveFailoverGroupServerTask(base.Wrapper.ExecuteMethod("RemoveFailoverGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "FailoverGroupFolder", "Unable to call method 'RemoveFailoverGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask MoveFailoverGroup(string failoverServer, string failoverGroup, int position)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MoveFailoverGroup");
			configurationApiItemWrapper.SetProperty("FailoverServer", failoverServer);
			configurationApiItemWrapper.SetProperty("FailoverGroup", failoverGroup);
			configurationApiItemWrapper.SetProperty("Position", position.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MoveFailoverGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "FailoverGroupFolder", "Unable to call method 'MoveFailoverGroup':" + ex.Message);
		}
		return null;
	}

	public MoveFailoverGroupServerTask MoveFailoverGroup()
	{
		try
		{
			return new MoveFailoverGroupServerTask(base.Wrapper.ExecuteMethod("MoveFailoverGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "FailoverGroupFolder", "Unable to call method 'MoveFailoverGroup':" + ex.Message);
		}
		return null;
	}
}

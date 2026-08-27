using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PatrollingProfileFolder : IConfigurationItem
{
	public ICollection<PatrollingProfile> PatrollingProfiles
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<PatrollingProfile> list = new List<PatrollingProfile>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "PatrollingProfile")
				{
					list.Add(new PatrollingProfile(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public PatrollingProfileFolder(ServerId serverId, ConfigurationItem patrollingProfileFolder)
		: base(serverId, patrollingProfileFolder)
	{
	}

	public PatrollingProfileFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PatrollingProfileFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddPatrollingProfile(string name, string description, bool customizeTransitions, double initSpeed, double initTransitionTime, string endPresetId, double endSpeed, double endTransitionTime)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddPatrollingProfile");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("CustomizeTransitions", customizeTransitions.ToString());
			configurationApiItemWrapper.SetProperty("InitSpeed", initSpeed.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("InitTransitionTime", initTransitionTime.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("EndPresetId", endPresetId);
			configurationApiItemWrapper.SetProperty("EndSpeed", endSpeed.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("EndTransitionTime", endTransitionTime.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddPatrollingProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfileFolder", "Unable to call method 'AddPatrollingProfile':" + ex.Message);
		}
		return null;
	}

	public AddPatrollingProfileServerTask AddPatrollingProfile()
	{
		try
		{
			return new AddPatrollingProfileServerTask(base.Wrapper.ExecuteMethod("AddPatrollingProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfileFolder", "Unable to call method 'AddPatrollingProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemovePatrollingProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemovePatrollingProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemovePatrollingProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfileFolder", "Unable to call method 'RemovePatrollingProfile':" + ex.Message);
		}
		return null;
	}

	public RemovePatrollingProfileServerTask RemovePatrollingProfile()
	{
		try
		{
			return new RemovePatrollingProfileServerTask(base.Wrapper.ExecuteMethod("RemovePatrollingProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfileFolder", "Unable to call method 'RemovePatrollingProfile':" + ex.Message);
		}
		return null;
	}
}

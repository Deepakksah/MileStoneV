using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PatrollingProfile : IConfigurationItem
{
	public ICollection<PatrollingEntryChildItem> PatrollingEntryChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<PatrollingEntryChildItem> list = new List<PatrollingEntryChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "PatrollingEntry")
				{
					list.Add(new PatrollingEntryChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public bool CustomizeTransitions
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CustomizeTransitions");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CustomizeTransitions", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public double InitSpeed
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("InitSpeed");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("InitSpeed", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double InitTransitionTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("InitTransitionTime");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("InitTransitionTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EndPresetIdValues => base.Wrapper.GetEnumValues("EndPresetId");

	public string EndPresetId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EndPresetId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndPresetId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double EndSpeed
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EndSpeed");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndSpeed", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double EndTransitionTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EndTransitionTime");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndTransitionTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public PatrollingProfile(ServerId serverId, ConfigurationItem patrollingProfile)
		: base(serverId, patrollingProfile)
	{
	}

	public PatrollingProfile(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PatrollingProfile(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddPatrollingEntry(int order, string presetId, double waitTime)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddPatrollingEntry");
			configurationApiItemWrapper.SetProperty("Order", order.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("PresetId", presetId);
			configurationApiItemWrapper.SetProperty("WaitTime", waitTime.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddPatrollingEntry"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfile", "Unable to call method 'AddPatrollingEntry':" + ex.Message);
		}
		return null;
	}

	public AddPatrollingEntryServerTask AddPatrollingEntry()
	{
		try
		{
			return new AddPatrollingEntryServerTask(base.Wrapper.ExecuteMethod("AddPatrollingEntry"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfile", "Unable to call method 'AddPatrollingEntry':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemovePatrollingEntry(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemovePatrollingEntry");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemovePatrollingEntry"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfile", "Unable to call method 'RemovePatrollingEntry':" + ex.Message);
		}
		return null;
	}

	public RemovePatrollingEntryServerTask RemovePatrollingEntry()
	{
		try
		{
			return new RemovePatrollingEntryServerTask(base.Wrapper.ExecuteMethod("RemovePatrollingEntry"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PatrollingProfile", "Unable to call method 'RemovePatrollingEntry':" + ex.Message);
		}
		return null;
	}
}

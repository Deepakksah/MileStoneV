using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ArchiveStorage : IConfigurationItem
{
	public ICollection<ArchiveScheduleChildItem> ArchiveScheduleChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ArchiveScheduleChildItem> list = new List<ArchiveScheduleChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ArchiveSchedule")
				{
					list.Add(new ArchiveScheduleChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string DiskPath => base.Wrapper.GetPropertyValue("DiskPath");

	public int RetainMinutes
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RetainMinutes");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RetainMinutes", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int MaxSize
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MaxSize");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MaxSize", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FramerateReductionEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FramerateReductionEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FramerateReductionEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public double TargetFramerate
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TargetFramerate");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TargetFramerate", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool Mounted
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Mounted");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public ArchiveStorage(ServerId serverId, ConfigurationItem archiveStorage)
		: base(serverId, archiveStorage)
	{
	}

	public ArchiveStorage(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ArchiveStorage(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ReadArchiveStorageInformation()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("ReadArchiveStorageInformation").ExecuteMethod("ReadArchiveStorageInformation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'ReadArchiveStorageInformation':" + ex.Message);
		}
		return null;
	}

	public ServerTask SetFramerateReductionArchiveStorage(bool framerateReductionEnabled)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SetFramerateReductionArchiveStorage");
			configurationApiItemWrapper.SetProperty("FramerateReductionEnabled", framerateReductionEnabled.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("SetFramerateReductionArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'SetFramerateReductionArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public SetFramerateReductionArchiveStorageServerTask SetFramerateReductionArchiveStorage()
	{
		try
		{
			return new SetFramerateReductionArchiveStorageServerTask(base.Wrapper.ExecuteMethod("SetFramerateReductionArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'SetFramerateReductionArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask MoveArchiveStorage(string diskPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MoveArchiveStorage");
			configurationApiItemWrapper.SetProperty("DiskPath", diskPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MoveArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'MoveArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public MoveArchiveStorageServerTask MoveArchiveStorage()
	{
		try
		{
			return new MoveArchiveStorageServerTask(base.Wrapper.ExecuteMethod("MoveArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'MoveArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask MountArchiveStorage()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("MountArchiveStorage").ExecuteMethod("MountArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'MountArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask UnmountArchiveStorage()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("UnmountArchiveStorage").ExecuteMethod("UnmountArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorage", "Unable to call method 'UnmountArchiveStorage':" + ex.Message);
		}
		return null;
	}
}

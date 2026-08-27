using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LoadTasksServerTask : ClassCollectionServerTask
{
	public bool IncludeRecordingServerTasks
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IncludeRecordingServerTasks");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("IncludeRecordingServerTasks", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string TaskType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TaskType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TaskType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public LoadTasksServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("LoadTasks"));
	}
}

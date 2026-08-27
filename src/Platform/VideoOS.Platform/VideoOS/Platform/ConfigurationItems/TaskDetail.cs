using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class TaskDetail : IConfigurationItem
{
	private IEnumerable<ChildTaskDetail> _childTasks;

	public int Progress
	{
		get
		{
			string property = base.Wrapper.GetProperty("Progress");
			int result = 100;
			if (int.TryParse(property, out result))
			{
				return result;
			}
			return 100;
		}
	}

	public StateEnum State
	{
		get
		{
			StateEnum result = StateEnum.Success;
			if (Enum.TryParse<StateEnum>(base.Wrapper.GetProperty("State"), out result))
			{
				return result;
			}
			return StateEnum.Success;
		}
	}

	public string ErrorCode => base.Wrapper.GetProperty("ErrorCode");

	public string ErrorText => base.Wrapper.GetProperty("ErrorText");

	public string TaskType => base.Wrapper.GetPropertyValue("TaskType");

	public ConfigurationApiProperties Properties => new ConfigurationApiProperties(base.Wrapper);

	public IEnumerable<ChildTaskDetail> ChildTasks
	{
		get
		{
			if (_childTasks == null)
			{
				_childTasks = from c in base.Wrapper.Children()
					select new ChildTaskDetail(base.Wrapper.ServerId, c);
			}
			return _childTasks;
		}
	}

	public TaskDetail(ServerId serverId, ConfigurationItem task)
		: base(serverId, task)
	{
	}

	public TaskDetail(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public void UpdateState()
	{
		if (Progress != 100)
		{
			base.Wrapper.ConfigurationItem = base.Wrapper.GetItem(base.Wrapper.Path);
		}
	}

	public ServerTask Cleanup()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("TaskCleanup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Task", "Unable to call method 'TaskCleanup': " + ex.Message);
		}
		return null;
	}

	public ServerTask Stop()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("TaskStop"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Task", "Unable to call method 'TaskStop': " + ex.Message);
		}
		return null;
	}
}

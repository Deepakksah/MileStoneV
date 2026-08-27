using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.RuleActions;

public sealed class ChangeHardwarePasswordRuleAction : RuleActionBase
{
	private class ChangeHardwarePasswordState
	{
		public volatile int UnsuccessfulTaskResultCount;
	}

	private IAsyncTaskMonitorFactory _taskMonitorFactory;

	private IServerEnvironment _serverEnvironment;

	private IChangeHardwarePasswordTaskBuilderFactory _taskBuilderFactory;

	private Guid[] _hardwareIds;

	public ChangeHardwarePasswordRuleAction(IServerEnvironment serverEnvironment, IAsyncTaskMonitorFactory taskMonitorFactory, IChangeHardwarePasswordTaskBuilderFactory taskBuilderFactory, Guid[] hardwareIds)
		: base(serverEnvironment.RuleEngine)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		if (taskBuilderFactory == null)
		{
			throw new ArgumentNullException("taskBuilderFactory");
		}
		if (taskMonitorFactory == null)
		{
			throw new ArgumentNullException("taskMonitorFactory");
		}
		_serverEnvironment = serverEnvironment;
		_taskMonitorFactory = taskMonitorFactory;
		_taskBuilderFactory = taskBuilderFactory;
		_hardwareIds = hardwareIds;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		Task.Factory.StartNew(ExecuteWorker, TaskCreationOptions.LongRunning);
	}

	private void ExecuteWorker()
	{
		_serverEnvironment.RuleEngine.AddTriggerEvent(new TriggerEvent(SourceGuid.Server, BuiltInEventTypes.EventIDs.ScheduledPasswordChangeStarted));
		IEnumerable<HardwarePasswordConfigData> selectedHardware = GetSelectedHardware();
		if (selectedHardware == null)
		{
			_serverEnvironment.RuleEngine.Logger.LogError("Unable to retrieve hardware list, aborting change hardware password rule action execution.");
			return;
		}
		List<IAsyncTask> list = new List<IAsyncTask>();
		try
		{
			using IChangeHardwarePasswordTaskBuilder builder = _taskBuilderFactory.CreateChangeHardwarePasswordTaskBuilder();
			PasswordGenerator passwordGenerator = new PasswordGenerator();
			foreach (HardwarePasswordConfigData item in selectedHardware)
			{
				PasswordRequirementsParser passwordRequirementsParser = new PasswordRequirementsParser();
				PasswordRequirements requirements = passwordRequirementsParser.GetRequirements(item.PasswordRequirements);
				string password = passwordGenerator.GeneratePassword(item.MinPasswordLength, item.MaxPasswordLength, requirements);
				IAsyncTask asyncTask = CreateChangeHardwarePasswordTask(item.HardwareId, item.Username, password, builder);
				if (asyncTask != null)
				{
					list.Add(asyncTask);
				}
			}
		}
		catch (VmoException)
		{
			_serverEnvironment.RuleEngine.Logger.LogError("Unable to create and launch hardware password change tasks, aborting change hardware password rule action execution.");
			return;
		}
		IAsyncTaskMonitor asyncTaskMonitor = _taskMonitorFactory.CreateAsyncTaskMonitor();
		asyncTaskMonitor.ContextState = new ChangeHardwarePasswordState();
		asyncTaskMonitor.Initialize(list);
		asyncTaskMonitor.TasksCompleted += TasksCompletedHandler;
		asyncTaskMonitor.TimeoutElapsed += TimeoutHandler;
		asyncTaskMonitor.TaskCompleted += TaskCompletedHandler;
		asyncTaskMonitor.Start(TimeSpan.FromHours(4.0));
	}

	private IEnumerable<HardwarePasswordConfigData> GetSelectedHardware()
	{
		List<HardwarePasswordConfigData> list = new List<HardwarePasswordConfigData>();
		IHardwareConfigDataComponent hardwareConfigDataComponent = _serverEnvironment.DataComponentFactory.CreateHardwareConfigDataComponent();
		HardwarePasswordConfigData[] array = null;
		Guid[] activeRecordingServers = _serverEnvironment.RecorderCommManager.QueryConnectedRecorderIds();
		try
		{
			array = ((_hardwareIds.Length == 0 || !(Guid.Parse(_hardwareIds[0].ToString()) == Guid.Parse("C5552DB0-A4ED-4BB0-B01C-E96E500D801F"))) ? hardwareConfigDataComponent.QueryHardwareForPasswordChange(selectAllHardware: false, activeRecordingServers, _hardwareIds) : hardwareConfigDataComponent.QueryHardwareForPasswordChange(selectAllHardware: true, activeRecordingServers, null));
		}
		catch (DatabaseException)
		{
			_serverEnvironment.RuleEngine.Logger.LogError("Unable to retrieve hardware list, aborting change hardware password rule action execution.");
			return null;
		}
		list.AddRange(array);
		return list;
	}

	private void TaskCompletedHandler(object sender, IAsyncTask task)
	{
		try
		{
			if (task.Status != AsyncTaskStatus.Succeeded)
			{
				IAsyncTaskMonitor asyncTaskMonitor = sender as IAsyncTaskMonitor;
				ChangeHardwarePasswordState changeHardwarePasswordState = asyncTaskMonitor.ContextState as ChangeHardwarePasswordState;
				changeHardwarePasswordState.UnsuccessfulTaskResultCount++;
			}
			LogScheduledTaskOperationResult(task);
			task.Cleanup();
		}
		catch (VmoException)
		{
			_serverEnvironment.RuleEngine.Logger.LogError("Task cleanup failed on change hardware password task.");
		}
	}

	private void TimeoutHandler(object sender, EventArgs e)
	{
		_serverEnvironment.RuleEngine.Logger.LogWarning("The change hardware password task monitor timeout expired.");
	}

	private void TasksCompletedHandler(object sender, EventArgs args)
	{
		IAsyncTaskMonitor asyncTaskMonitor = sender as IAsyncTaskMonitor;
		ChangeHardwarePasswordState changeHardwarePasswordState = asyncTaskMonitor.ContextState as ChangeHardwarePasswordState;
		if (changeHardwarePasswordState.UnsuccessfulTaskResultCount == 0)
		{
			_serverEnvironment.RuleEngine.Logger.LogInformation("All scheduled change hardware password tasks have completed successfully.");
			_serverEnvironment.RuleEngine.AddTriggerEvent(new TriggerEvent(SourceGuid.Server, BuiltInEventTypes.EventIDs.ScheduledPasswordChangeCompletedSuccessfully));
		}
		else
		{
			_serverEnvironment.RuleEngine.Logger.LogInformation("All scheduled change hardware password tasks have completed, some with errors.");
			_serverEnvironment.RuleEngine.AddTriggerEvent(new TriggerEvent(SourceGuid.Server, BuiltInEventTypes.EventIDs.ScheduledPasswordChangeCompletedWithErrors));
		}
	}

	private IAsyncTask CreateChangeHardwarePasswordTask(Guid id, string username, string password, IChangeHardwarePasswordTaskBuilder builder)
	{
		try
		{
			return builder.CreateChangeHardwarePasswordTask(id, username, password);
		}
		catch (VmoException)
		{
			_serverEnvironment.RuleEngine.Logger.LogError("Failed to create a change hardware password task due to a VMO error.");
		}
		catch (NotSupportedException)
		{
			_serverEnvironment.RuleEngine.Logger.LogError("Failed to create a change hardware password task, operation not supported.");
		}
		return null;
	}

	private void LogScheduledTaskOperationResult(IAsyncTask task)
	{
		ChangeHardwarePasswordTask changeHardwarePasswordTask = task as ChangeHardwarePasswordTask;
		if (task != null && changeHardwarePasswordTask != null)
		{
			if (task.Status == AsyncTaskStatus.Succeeded)
			{
				_serverEnvironment.ServerLogManager.AddLogEntry(LogEntryId.ScheduledHardwarePasswordChangeSuccess, changeHardwarePasswordTask.HardwareName);
			}
			if (task.Status == AsyncTaskStatus.Failed)
			{
				TaskError errorResult = changeHardwarePasswordTask.ErrorResult;
				_serverEnvironment.ServerLogManager.AddLogEntry(LogEntryId.ScheduledHardwarePasswordChangeFailure, changeHardwarePasswordTask.HardwareName, errorResult.ErrorText);
			}
		}
	}
}

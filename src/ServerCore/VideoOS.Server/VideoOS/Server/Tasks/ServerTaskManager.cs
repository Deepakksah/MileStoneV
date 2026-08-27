using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Tasks;
using VideoOS.Server.CommandHandler;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Tasks;

public sealed class ServerTaskManager : TaskManagerBase
{
	private static object instanceSync = new object();

	private static volatile ServerTaskManager instance;

	private object getTaskSync = new object();

	private long lastTaskSeqNumber;

	private static volatile bool _taskCheckRunning = false;

	public static ServerTaskManager Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			lock (instanceSync)
			{
				if (instance != null)
				{
					return instance;
				}
				ILogger iLogger = Telemetry.GetILogger("TaskManager");
				instance = new ServerTaskManager(new ServerTaskAccessor(iLogger), new ServerTaskCommandFactory(iLogger), new ServerTaskProcessor(), iLogger);
			}
			return instance;
		}
	}

	private ServerTaskManager(ITaskAccessor taskAccessor, ITaskCommandFactory taskCommandFactory, ITaskProcessor taskProcessor, ILogger logger)
		: base(taskAccessor, taskCommandFactory, taskProcessor, logger)
	{
		ServerEnvironment serverEnvironment = new ServerEnvironment();
		CommandProcessor.Register(typeof(DeleteHardwareTaskCommand), new DeleteHardwareTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(HardwareScanTaskCommand), new HardwareScanTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(HardwareScanExpressTaskCommand), new HardwareScanExpressTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(AddHardwareTaskCommand), new AddHardwareTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(MoveHardwareTaskCommand), new MoveHardwareTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ChangeHardwarePasswordTaskCommand), new ChangeHardwarePasswordTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ActivateLicenseTaskCommand), new ActivateLicenseTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(UpdateHardwareTaskCommand), new UpdateHardwareTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(FirmwareUpgradeTaskCommand), new FirmwareUpgradeTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ChangeIdpAdminTaskCommand), new ChangeIdpAdminTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(DeleteIdpUserTaskCommand), new DeleteIdpUserTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(GetDevicePresetsTaskCommand), new GetDevicePresetsTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ReplaceHardwareTaskCommand), new ReplaceHardwareTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(DeleteRecorderTaskCommand), new DeleteRecorderTaskCommandHandler(serverEnvironment), ExecutorType.AdvancedThreadPool);
	}

	public override void CheckForTasksAvailable()
	{
		if (_taskCheckRunning)
		{
			return;
		}
		lock (getTaskSync)
		{
			if (_taskCheckRunning)
			{
				return;
			}
			try
			{
				_taskCheckRunning = true;
				DateTime startTime = DateTime.UtcNow;
				ServerTaskDto[] array = base.TaskAccessor.QueryTasks(lastTaskSeqNumber);
				ProcessConfigurationChangeTasks(array);
				ProcessSecurityConfigurationChangeTasks(array);
				ProcessAsyncTasks(array);
				if (array.Length != 0)
				{
					lastTaskSeqNumber = array.Max((ServerTaskDto t) => t.SequenceNumber);
					DateTime endTime = DateTime.UtcNow;
					Task.Run(delegate
					{
						if ((endTime - startTime).TotalSeconds < 1.0)
						{
							Thread.Sleep((startTime.AddSeconds(1.0) - endTime).Milliseconds);
						}
						_taskCheckRunning = false;
						CheckForTasksAvailable();
					});
				}
				else
				{
					_taskCheckRunning = false;
				}
			}
			catch
			{
				_taskCheckRunning = false;
				throw;
			}
		}
	}

	protected override void ProcessConfigurationChangeTasks(ServerTaskDto[] allServerTasks)
	{
		foreach (ConfigurationDataSet item in ServerTaskBatchHandler.EnumerateConfigurationChangeTaskBatches(allServerTasks))
		{
			base.TaskProcessor.UpdateConfiguration(item);
			item.Dispose();
		}
	}
}

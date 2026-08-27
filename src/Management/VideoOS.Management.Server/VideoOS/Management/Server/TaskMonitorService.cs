using System;
using VideoOS.Utilities.Database;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public sealed class TaskMonitorService : PeriodicTimerServiceBase
{
	protected override TimeSpan InitialTimeInterval => TimeSpan.FromSeconds(new Random().Next(60, 90));

	protected override TimeSpan PeriodicTimeInterval => TimeSpan.FromMinutes(1.0);

	protected override void OnTimer(object state)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		string systemSetting;
		try
		{
			systemSetting = ServerInformation.GetSystemSetting(requestContext, SettingList.TaskRetentionMinutes);
		}
		catch (DatabaseException ex)
		{
			GeminiTrace.Warning(TraceType.General, "A SQL error occurred when reading system setting during cleanup of server tasks. Exception:\r\n{0}", ex.ToString());
			return;
		}
		if (!int.TryParse(systemSetting, out var result))
		{
			GeminiTrace.Warning(TraceType.General, "Failed to parse task retention minutes as an int: String value: '{0}'", systemSetting ?? "<null>");
			return;
		}
		try
		{
			foreach (TaskTypeInformation value in TaskTypeInformation.TaskTypesDictionary.Values)
			{
				if (value.RetentionPolicy == Task.RetentionPolicy.Default)
				{
					Task.CleanupExpiredTasks(requestContext, value.TaskType, result);
				}
			}
		}
		catch (DatabaseException ex2)
		{
			GeminiTrace.Warning(TraceType.General, "A SQL error occurred during cleanup of server tasks. Exception:\r\n{0}", ex2.ToString());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingStorageTaskResultDefinition
{
	public static Property[] CreateSetEncryptionMethodProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("Storage", task.Target);
			list.Add(TaskConfigurationApi.GetPath(path));
		}
		return list.ToArray();
	}

	public static Property[] CreateSetEncryptionPasswordProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("Storage", task.Target);
			list.Add(TaskConfigurationApi.GetPath(path));
		}
		return list.ToArray();
	}

	public static Property[] MoveRecordingStorageProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("Storage", task.Target);
			list.Add(TaskConfigurationApi.GetPath(path));
		}
		list.Add(new Property("pathSource", ConfigurationApiResources.PropertyPathSource, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("pathSource", StringComparison.Ordinal)).Value));
		list.Add(new Property("pathTarget", ConfigurationApiResources.PropertyPathTarget, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("pathTarget", StringComparison.Ordinal)).Value));
		return list.ToArray();
	}
}

using System;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ArchiveStorageTaskResultDefinition
{
	public static Property[] MoveArchiveStorageMethodProperties(Task task)
	{
		if (task.AsyncStatus.ExceptionStatus != ExceptionStatus.Succeeded)
		{
			return Array.Empty<Property>();
		}
		return CreateArchivePathProperty(task, "idArchive");
	}

	public static Property[] MountArchiveStorageMethodProperties(Task task)
	{
		if (task.AsyncStatus.ExceptionStatus != ExceptionStatus.Succeeded)
		{
			return Array.Empty<Property>();
		}
		return CreateArchivePathProperty(task, "idArchive");
	}

	public static Property[] UnmountArchiveStorageMethodProperties(Task task)
	{
		if (task.AsyncStatus.ExceptionStatus != ExceptionStatus.Succeeded)
		{
			return Array.Empty<Property>();
		}
		return CreateArchivePathProperty(task, "idArchive");
	}

	private static Property[] CreateArchivePathProperty(Task task, string taskPropertyName)
	{
		string value = task.TaskProperties.GetValue(taskPropertyName);
		ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("ArchiveStorage", value);
		return new Property[1] { TaskConfigurationApi.GetPath(path) };
	}
}

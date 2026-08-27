using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class DeviceTaskResultDefinition
{
	public static Property[] TruncateDeviceStorageResultProperties(IGeminiRequestContext requestContext, Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath(GetDeviceTypeRelation(requestContext, task, "idDevice").ItemType, task.Target);
			list.Add(TaskConfigurationApi.GetPath(path));
		}
		list.Add(new Property("idRecorder", ConfigurationApiResources.PropertyPathSource, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("idRecorder", StringComparison.Ordinal)).Value));
		list.Add(new Property("target", ConfigurationApiResources.PropertyPathSource, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("target", StringComparison.Ordinal)).Value));
		list.Add(new Property("idStorage", ConfigurationApiResources.PropertyPathSource, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("idStorage", StringComparison.Ordinal)).Value));
		list.Add(new Property("idDevice", ConfigurationApiResources.PropertyPathSource, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("idDevice", StringComparison.Ordinal)).Value));
		list.Add(new Property("deviceName", ConfigurationApiResources.PropertyPathSource, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("deviceName", StringComparison.Ordinal)).Value));
		return list.ToArray();
	}

	public static Property[] MoveDeviceStorageProperties(IGeminiRequestContext requestContext, Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath(GetDeviceTypeRelation(requestContext, task, "idDevice").ItemType, task.Target);
			list.Add(TaskConfigurationApi.GetPath(path));
		}
		list.Add(new Property("idStorageTarget", ConfigurationApiResources.PropertyTargetStorage, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("idStorageTarget", StringComparison.Ordinal)).Value));
		list.Add(new Property("moveData", ConfigurationApiResources.PropertyMoveDeviceData, task.TaskProperties.Single((KeyValue p) => p.Key.Equals("moveData", StringComparison.Ordinal)).Value));
		return list.ToArray();
	}

	private static DeviceTypeRelation GetDeviceTypeRelation(IGeminiRequestContext requestContext, Task task, string deviceIdKey)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		if (deviceIdKey == null)
		{
			throw new ArgumentNullException("deviceIdKey");
		}
		string value = task.TaskProperties.FirstOrDefault((KeyValue f) => string.Equals(f.Key, deviceIdKey)).Value;
		Device device = Device.QueryDeviceById(requestContext, value);
		if (device == null)
		{
			throw new InternalServerException("Unable to find device");
		}
		return DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device.DeviceType) ?? throw new InternalServerException();
	}
}

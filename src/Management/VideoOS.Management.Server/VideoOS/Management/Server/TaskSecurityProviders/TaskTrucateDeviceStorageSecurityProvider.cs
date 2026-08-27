using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskTrucateDeviceStorageSecurityProvider : ITaskSecurityProvider
{
	IEnumerable<Task> ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		string[] array = tasks.Select((Task t) => t.TaskProperties.GetValue("idDevice")).ToArray();
		Device[] source = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			source = deviceComponent.QueryDevicesByIds(array);
		}
		IEnumerable<IGrouping<string, Device>> enumerable = from d in source
			group d by d.RecordingServerId;
		List<Task> list = new List<Task>(array.Length);
		foreach (IGrouping<string, Device> item in enumerable)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", item.Key))
			{
				continue;
			}
			foreach (Device device in item)
			{
				IEnumerable<Task> collection = tasks.Where((Task t) => string.Equals(device.Id, t.TaskProperties.GetValue("idDevice"), StringComparison.OrdinalIgnoreCase));
				list.AddRange(collection);
			}
		}
		return list;
	}

	bool ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, Task task)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		string value = task.TaskProperties.GetValue("idDevice");
		Device device;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			device = deviceComponent.QueryDeviceById(value);
		}
		if (device == null)
		{
			return false;
		}
		if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", device.RecordingServerId))
		{
			return false;
		}
		return true;
	}

	void ITaskSecurityProvider.CheckRead(IGeminiRequestContext requestContext, Task task)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		string value = task.TaskProperties.GetValue("idDevice");
		Device device;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			device = deviceComponent.QueryDeviceById(value);
		}
		if (device == null)
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		securityManagerService.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", device.RecordingServerId);
	}

	void ITaskSecurityProvider.CheckStop(IGeminiRequestContext requestContext, Task task)
	{
		throw new NotSupportedException();
	}
}

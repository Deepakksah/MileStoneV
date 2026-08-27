using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskEdgeStorageRetrivalSecurityProvider : ITaskSecurityProvider
{
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
		if (device.DeviceType == DeviceType.Camera || device.DeviceType == DeviceType.Microphone || device.DeviceType == DeviceType.Speaker || device.DeviceType == DeviceType.Metadata)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			if (!securityManagerService.HasObjectPermission(requestContext, securityNamespace, "GENERIC_READ", value) || !securityManagerService.HasObjectPermission(requestContext, securityNamespace, "READ_REMOTE_RECORDINGS", value))
			{
				return false;
			}
			return true;
		}
		throw new NotImplementedException();
	}

	IEnumerable<Task> ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		List<Task> list = new List<Task>(tasks.Count());
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		IEnumerable<string> ids = tasks.Select((Task t) => t.TaskProperties.GetValue("idDevice")).Distinct();
		ConcurrentBag<string> allowedDevices = new ConcurrentBag<string>();
		Device[] devices;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			devices = deviceComponent.QueryDevicesByIds(ids);
		}
		Device[] array = devices.Where((Device d) => d.DeviceType == DeviceType.Camera || d.DeviceType == DeviceType.Microphone || d.DeviceType == DeviceType.Speaker || d.DeviceType == DeviceType.Metadata).ToArray();
		Parallel.For(0, array.Length, delegate(int i)
		{
			Device device = devices[i];
			string id = device.Id;
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			if (securityManagerService.HasObjectPermission(requestContext, securityNamespace, "GENERIC_READ", id) || securityManagerService.HasObjectPermission(requestContext, securityNamespace, "READ_REMOTE_RECORDINGS", id))
			{
				allowedDevices.Add(id);
			}
		});
		foreach (Task task in tasks)
		{
			if (allowedDevices.Contains(task.TaskProperties.GetValue("idDevice"), StringComparer.OrdinalIgnoreCase))
			{
				list.Add(task);
			}
		}
		return list.ToArray();
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
		if (device.DeviceType == DeviceType.Camera || device.DeviceType == DeviceType.Microphone || device.DeviceType == DeviceType.Speaker || device.DeviceType == DeviceType.Metadata)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			securityManagerService.CheckObjectPermission(requestContext, securityNamespace, "GENERIC_READ", value);
			securityManagerService.CheckObjectPermission(requestContext, securityNamespace, "READ_REMOTE_RECORDINGS", value);
			return;
		}
		throw new NotImplementedException();
	}

	void ITaskSecurityProvider.CheckStop(IGeminiRequestContext requestContext, Task task)
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
		if (device.DeviceType == DeviceType.Camera || device.DeviceType == DeviceType.Microphone || device.DeviceType == DeviceType.Speaker || device.DeviceType == DeviceType.Metadata)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			securityManagerService.CheckObjectPermission(requestContext, securityNamespace, "READ_REMOTE_RECORDINGS", value);
			return;
		}
		throw new NotImplementedException();
	}
}

using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class SystemMonitorStateSourceGeneric
{
	private static GenericObject2[] ConvertToSystemMonitorStateSourceGenericObject2s(SystemMonitorStateSource[] systemMonitorStateSource)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (SystemMonitorStateSource systemMonitorStateSource2 in systemMonitorStateSource)
		{
			list.Add(ConvertToSystemMonitorStateSourceGenericObject2(systemMonitorStateSource2));
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToSystemMonitorStateSourceGenericObject2(SystemMonitorStateSource systemMonitorStateSource)
	{
		if (systemMonitorStateSource == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", systemMonitorStateSource.SourceId));
		list.Add(new ObjectProperty2("Name", systemMonitorStateSource.Name));
		return new GenericObject2(systemMonitorStateSource.SourceId, "SystemMonitorStateSource", list.ToArray(), null);
	}

	public static GenericObject2[] QuerySystemMonitorStateSourcesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		return ConvertToSystemMonitorStateSourceGenericObject2s(SystemMonitorStateSource.QuerySystemMonitorSourcesByIds(requestContext, ids));
	}

	public static GenericObject2[] QuerySystemMonitorStateSourcesByTypes(IGeminiRequestContext requestContext, string[] types)
	{
		return ConvertToSystemMonitorStateSourceGenericObject2s(SystemMonitorStateSource.QuerySystemMonitorStateSourcesByTypes(requestContext, types));
	}
}

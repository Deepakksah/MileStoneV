using System;

namespace VideoOS.Management.Server;

public sealed class SystemMonitorStateSource
{
	public string SourceId { get; set; }

	public string Name { get; set; }

	public int DataCollectorId { get; set; }

	public string ServiceName { get; set; }

	public static SystemMonitorStateSource[] QuerySystemMonitorSourcesByIds(IGeminiRequestContext requestContext, string[] sourceIds)
	{
		if (sourceIds == null)
		{
			throw new ArgumentNullException("sourceIds", "Sources ids should be specified");
		}
		using SystemMonitorStateSourceComponent systemMonitorStateSourceComponent = requestContext.CreateComponent<SystemMonitorStateSourceComponent>();
		return systemMonitorStateSourceComponent.QuerySystemMonitorSourcesByIds(sourceIds);
	}

	public static SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByTypes(IGeminiRequestContext requestContext, string[] sourceTypes)
	{
		if (sourceTypes == null)
		{
			throw new ArgumentNullException("sourceTypes", "Sources ids should be specified");
		}
		using SystemMonitorStateSourceComponent systemMonitorStateSourceComponent = requestContext.CreateComponent<SystemMonitorStateSourceComponent>();
		return systemMonitorStateSourceComponent.QuerySystemMonitorSourcesByTypes(sourceTypes);
	}
}

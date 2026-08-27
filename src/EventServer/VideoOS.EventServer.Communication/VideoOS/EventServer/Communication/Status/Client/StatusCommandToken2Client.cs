using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class StatusCommandToken2Client : ClientBase<IStatusCommandToken2>, IStatusCommandToken2
{
	public StatusCommandToken2Client()
	{
	}

	public StatusCommandToken2Client(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public StatusCommandToken2Client(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StatusCommandToken2Client(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StatusCommandToken2Client(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public MapObjectState[] GetStatus(string token, string ID)
	{
		return base.Channel.GetStatus(token, ID);
	}

	public MapObjectState[] GetStatusTranslated(string token, string ID, string language)
	{
		return base.Channel.GetStatusTranslated(token, ID, language);
	}

	public MapObjectState[] GetStatusByUserIds(string token, string ID, MapUserId[] userIds)
	{
		return base.Channel.GetStatusByUserIds(token, ID, userIds);
	}

	public MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string token, string ID, Guid type, Guid relationType, int maxLevels)
	{
		return base.Channel.GetStatusForAllObjectsOfType(token, ID, type, relationType, maxLevels);
	}

	public string GetStatusDetails(string token, string ID, Guid type, string language)
	{
		return base.Channel.GetStatusDetails(token, ID, type, language);
	}

	public DateTime SetVisualization(string token, VisualizationState visualizationState)
	{
		return base.Channel.SetVisualization(token, visualizationState);
	}

	public VisualizationState GetVisualization(string token)
	{
		return base.Channel.GetVisualization(token);
	}

	public VisualizationState GetVisualizationChanges(string token, DateTime timestamp, int timeout)
	{
		return base.Channel.GetVisualizationChanges(token, timestamp, timeout);
	}
}

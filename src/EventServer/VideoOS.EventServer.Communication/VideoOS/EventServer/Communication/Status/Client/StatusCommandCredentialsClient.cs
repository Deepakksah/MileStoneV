using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class StatusCommandCredentialsClient : ClientBase<IStatusCommandCredentials>, IStatusCommandCredentials
{
	public StatusCommandCredentialsClient()
	{
	}

	public StatusCommandCredentialsClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public StatusCommandCredentialsClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StatusCommandCredentialsClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StatusCommandCredentialsClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public MapObjectState[] GetStatus(string ID)
	{
		return base.Channel.GetStatus(ID);
	}

	public MapObjectState[] GetStatusTranslated(string ID, string language)
	{
		return base.Channel.GetStatusTranslated(ID, language);
	}

	public MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string ID, Guid type, Guid relationType, int maxLevels)
	{
		return base.Channel.GetStatusForAllObjectsOfType(ID, type, relationType, maxLevels);
	}

	public string GetStatusDetails(string ID, Guid type, string language)
	{
		return base.Channel.GetStatusDetails(ID, type, language);
	}

	public DateTime SetVisualization(VisualizationState visualizationState)
	{
		return base.Channel.SetVisualization(visualizationState);
	}

	public VisualizationState GetVisualization()
	{
		return base.Channel.GetVisualization();
	}

	public VisualizationState GetVisualizationChanges(DateTime timestamp, int timeout)
	{
		return base.Channel.GetVisualizationChanges(timestamp, timeout);
	}
}

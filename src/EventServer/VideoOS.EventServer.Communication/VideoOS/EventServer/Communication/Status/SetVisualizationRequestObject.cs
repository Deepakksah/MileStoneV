using VideoOS.EventServer.Communication.Status.Client;

namespace VideoOS.EventServer.Communication.Status;

public class SetVisualizationRequestObject : RequestObject
{
	private VisualizationState _visualizationState;

	public VisualizationState VisualizationState => _visualizationState;

	public SetVisualizationRequestObject(int requestId, VisualizationState visualizationState)
		: base(requestId)
	{
		_visualizationState = visualizationState;
	}
}

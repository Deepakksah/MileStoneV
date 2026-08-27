using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class DeleteObjectRequestObject : RequestObject
{
	private VisualElement _visualElement;

	public VisualElement VisualElement => _visualElement;

	public DeleteObjectRequestObject(int requestId, VisualElement visualElement)
		: base(requestId)
	{
		_visualElement = visualElement;
	}
}

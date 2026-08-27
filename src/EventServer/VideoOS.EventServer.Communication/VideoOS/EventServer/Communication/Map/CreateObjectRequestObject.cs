using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class CreateObjectRequestObject : RequestObject
{
	private VisualElement _visualElement;

	public VisualElement VisualElement => _visualElement;

	public CreateObjectRequestObject(int requestId, VisualElement visualElement)
		: base(requestId)
	{
		_visualElement = visualElement;
	}
}

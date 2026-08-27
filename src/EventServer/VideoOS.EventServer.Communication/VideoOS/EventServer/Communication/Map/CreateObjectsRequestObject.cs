using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class CreateObjectsRequestObject : RequestObject
{
	private VisualElement[] _visualElements;

	public VisualElement[] VisualElements => _visualElements;

	public CreateObjectsRequestObject(int requestId, VisualElement[] visualElements)
		: base(requestId)
	{
		_visualElements = visualElements;
	}
}

using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class DeleteObjectsRequestObject : RequestObject
{
	private VisualElement[] _elementsToDelete;

	public VisualElement[] ElementsToDelete => _elementsToDelete;

	public DeleteObjectsRequestObject(int requestId, VisualElement[] elementsToDelete)
		: base(requestId)
	{
		_elementsToDelete = elementsToDelete;
	}
}

using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class UpdateObjectsRequestObject : RequestObject
{
	private VisualElement[] _visualElements;

	private bool _force;

	public VisualElement[] VisualElements => _visualElements;

	public bool Force => _force;

	public UpdateObjectsRequestObject(int requestId, VisualElement[] visualElements, bool force)
		: base(requestId)
	{
		_visualElements = visualElements;
		_force = force;
	}
}

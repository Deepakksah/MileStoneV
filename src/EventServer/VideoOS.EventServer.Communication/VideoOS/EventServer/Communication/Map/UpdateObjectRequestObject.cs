using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class UpdateObjectRequestObject : RequestObject
{
	private VisualElement _visualElement;

	private bool _force;

	public VisualElement VisualElement => _visualElement;

	public bool Force => _force;

	public UpdateObjectRequestObject(int requestId, VisualElement visualElement, bool force)
		: base(requestId)
	{
		_visualElement = visualElement;
		_force = force;
	}
}

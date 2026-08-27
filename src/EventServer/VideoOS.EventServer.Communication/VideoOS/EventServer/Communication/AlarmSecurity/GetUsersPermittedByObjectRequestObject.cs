namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class GetUsersPermittedByObjectRequestObject : RequestObject
{
	private string _objectId;

	public string ObjectId => _objectId;

	public GetUsersPermittedByObjectRequestObject(int requestId, string objectId)
		: base(requestId)
	{
		_objectId = objectId;
	}
}

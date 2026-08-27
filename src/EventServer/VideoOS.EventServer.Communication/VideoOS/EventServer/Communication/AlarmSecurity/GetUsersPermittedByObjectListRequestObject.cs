namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class GetUsersPermittedByObjectListRequestObject : RequestObject
{
	private string[] _objectIdList;

	public string[] ObjectIdList => _objectIdList;

	public GetUsersPermittedByObjectListRequestObject(int requestId, string[] objectIdList)
		: base(requestId)
	{
		_objectIdList = objectIdList;
	}
}

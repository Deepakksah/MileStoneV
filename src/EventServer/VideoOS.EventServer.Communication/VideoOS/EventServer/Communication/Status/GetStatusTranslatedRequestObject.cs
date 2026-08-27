namespace VideoOS.EventServer.Communication.Status;

public class GetStatusTranslatedRequestObject : RequestObject
{
	private string _id;

	private string _language;

	public string Id => _id;

	public string Language => _language;

	public GetStatusTranslatedRequestObject(int requestId, string id, string language)
		: base(requestId)
	{
		_id = id;
		_language = language;
	}
}

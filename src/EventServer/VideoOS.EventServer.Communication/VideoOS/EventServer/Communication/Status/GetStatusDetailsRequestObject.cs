using System;

namespace VideoOS.EventServer.Communication.Status;

public class GetStatusDetailsRequestObject : RequestObject
{
	private string _id;

	private Guid _type;

	private string _language;

	public string Id => _id;

	public Guid Type => _type;

	public string Language => _language;

	public GetStatusDetailsRequestObject(int requestId, string id, Guid type, string language)
		: base(requestId)
	{
		_id = id;
		_type = type;
		_language = language;
	}
}

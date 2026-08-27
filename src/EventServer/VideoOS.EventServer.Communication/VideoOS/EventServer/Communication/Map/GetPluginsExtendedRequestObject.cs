using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetPluginsExtendedRequestObject : RequestObject
{
	private Guid[] _kinds;

	private string _language;

	public Guid[] Kinds => _kinds;

	public string Language => _language;

	public GetPluginsExtendedRequestObject(int requestId, Guid[] kinds, string language)
		: base(requestId)
	{
		_kinds = kinds;
		_language = language;
	}
}

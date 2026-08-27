using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class GetEventTypesForUserRequestObject : RequestObject
{
	private Guid _sourceId;

	private string _languageId;

	public string LanguageId => _languageId;

	public Guid SourceId => _sourceId;

	public GetEventTypesForUserRequestObject(int requestId, Guid sourceId, string languageId)
		: base(requestId)
	{
		_sourceId = sourceId;
		_languageId = languageId;
	}
}

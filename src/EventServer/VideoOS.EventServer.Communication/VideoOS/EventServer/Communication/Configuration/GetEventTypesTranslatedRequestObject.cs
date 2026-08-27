using System.Globalization;

namespace VideoOS.EventServer.Communication.Configuration;

public class GetEventTypesTranslatedRequestObject : RequestObject
{
	private CultureInfo _cultureInfo;

	public CultureInfo CultureInfo => _cultureInfo;

	public GetEventTypesTranslatedRequestObject(int requestId, CultureInfo cultureInfo)
		: base(requestId)
	{
		_cultureInfo = cultureInfo;
	}
}

namespace VideoOS.EventServer.Communication.Configuration;

public class SetGeneralSettingsRequestObject : RequestObject
{
	private GeneralSettings _generalSettings;

	public GeneralSettings GeneralSettings => _generalSettings;

	public SetGeneralSettingsRequestObject(int requestId, GeneralSettings generalSettings)
		: base(requestId)
	{
		_generalSettings = generalSettings;
	}
}

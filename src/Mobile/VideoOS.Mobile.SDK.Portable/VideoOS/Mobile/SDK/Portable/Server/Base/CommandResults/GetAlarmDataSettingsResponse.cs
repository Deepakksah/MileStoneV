using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAlarmDataSettingsResponse : BaseCommandResponse
{
	public AlarmDataSettingsDetail AlarmDataSettings { get; private set; }

	public GetAlarmDataSettingsResponse(Command command)
		: base(command)
	{
		AlarmDataSettings = SerializableAlarmDataSettings.GetAlarmDataSettings(command.Items);
	}

	public GetAlarmDataSettingsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}

using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAlarmListResponse : BaseCommandResponse
{
	public IEnumerable<AlarmLineDetail> Alarms { get; private set; }

	public GetAlarmListResponse(Command command)
		: base(command)
	{
		Alarms = SerializableAlarm.GetAlarmLinesDetail(command.Items);
	}

	public GetAlarmListResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}

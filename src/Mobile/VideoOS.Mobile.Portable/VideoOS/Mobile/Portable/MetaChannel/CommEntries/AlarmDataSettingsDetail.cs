using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class AlarmDataSettingsDetail
{
	public List<AlarmLevelNameDetail> States { get; private set; }

	public List<AlarmLevelNameDetail> Priorities { get; private set; }

	public List<AlarmLevelNameDetail> Categories { get; private set; }

	public AlarmDataSettingsDetail()
	{
		States = new List<AlarmLevelNameDetail>();
		Priorities = new List<AlarmLevelNameDetail>();
		Categories = new List<AlarmLevelNameDetail>();
	}
}

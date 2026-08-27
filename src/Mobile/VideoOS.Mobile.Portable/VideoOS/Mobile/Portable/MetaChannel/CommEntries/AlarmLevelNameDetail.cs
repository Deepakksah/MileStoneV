using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class AlarmLevelNameDetail
{
	public Guid Id;

	public int Level;

	public string Name;

	public AlarmLevelNameDetail()
	{
	}

	public AlarmLevelNameDetail(Guid id, int level, string name)
	{
		Id = id;
		Level = level;
		Name = name;
	}
}

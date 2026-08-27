using System;
using System.Data;
using VideoOS.Common.SharedTimeProfile;

namespace VideoOS.Common.Integration.Configuration;

public class TimeProfiles : CollectionBase
{
	public new TimeProfile this[Guid id] => base[id] as TimeProfile;

	public TimeProfiles(ConfigurationManager manager)
		: base(manager, manager.Dataset.TimeProfile)
	{
	}

	public TimeProfile Add(Guid timeProfileId, string name, string description, string definitionXml, TimeProfileType timeProfileType)
	{
		DateTime now = DateTime.Now;
		return Add(_configurationManager.Dataset.TimeProfile.AddTimeProfileRow(name, description, timeProfileType.ToString(), timeProfileId.ToString(), definitionXml, now, now)) as TimeProfile;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new TimeProfile(_configurationManager, row as ConfigurationDataSet.TimeProfileRow);
	}
}

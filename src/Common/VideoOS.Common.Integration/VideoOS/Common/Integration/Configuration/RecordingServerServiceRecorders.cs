using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class RecordingServerServiceRecorders : CollectionBase
{
	public new RecordingServerServiceRecorder this[Guid id] => base[id] as RecordingServerServiceRecorder;

	public RecordingServerServiceRecorders(ConfigurationManager manager)
		: base(manager, manager.Dataset.RecordingServerServiceRecorder)
	{
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new RecordingServerServiceRecorder(_configurationManager, row as ConfigurationDataSet.RecordingServerServiceRecorderRow);
	}
}

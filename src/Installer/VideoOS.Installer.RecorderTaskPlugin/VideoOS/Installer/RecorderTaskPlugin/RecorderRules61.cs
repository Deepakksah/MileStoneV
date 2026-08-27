using System;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderRules61 : Rules
{
	public override Version ConfigurationFileVersion => new Version("6.1");

	public RecorderRules61()
	{
		RegisterNodeToOverwrite("/recorderconfig/EdgeStorage/@MaxSimultaneousJobs");
	}
}

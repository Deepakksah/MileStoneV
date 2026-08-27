using System;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderRules102 : Rules
{
	public override Version ConfigurationFileVersion => new Version("10.2");

	public RecorderRules102()
	{
		RegisterNodeToOverwrite("/recorderconfig/pipeline/maxactivetimepipeline2");
	}
}

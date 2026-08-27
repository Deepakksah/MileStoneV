using System;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderRules233 : Rules
{
	public override Version ConfigurationFileVersion => new Version("23.3");

	public RecorderRules233()
	{
		RegisterNodeToOverwrite("/recorderconfig/webserver/requestThread/@maxStackSizeKB");
	}
}

using System;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderRules60 : Rules
{
	public override Version ConfigurationFileVersion => new Version("6.0");

	public RecorderRules60()
	{
		RegisterNodeToOverwrite("/recorderconfig/EdgeStorage/@StartupRetrieveSeconds");
	}
}

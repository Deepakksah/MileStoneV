using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Scripts;

namespace VideoOS.Installer.LogServerPlugins;

public class LsScripts : ScriptListFactoryBase
{
	public LsScripts(ILogger logger)
		: base(logger, "VideoOS.Installer.LogServerPlugins", "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts")
	{
	}

	public override IReadOnlyList<ScriptBase> GetUpgradeScripts()
	{
		return new List<ScriptBase>
		{
			CreateSqlScript(new Version(12, 3, 0), "Schema"),
			CreateSqlScript(new Version(12, 3, 1), "ConfigurationMessagesAndRetention"),
			CreateSqlScript(new Version(12, 3, 2), "TableTypes"),
			CreateSqlScript(new Version(12, 3, 3), "DefaultValues"),
			CreateSqlScript(new Version(12, 3, 4), "MessageInstanceStructure"),
			CreateSqlScript(new Version(21, 2, 1), "MessageInstance_0_update")
		};
	}

	public override IReadOnlyList<ScriptBase> GetBaselineScripts()
	{
		return new List<ScriptBase>();
	}
}

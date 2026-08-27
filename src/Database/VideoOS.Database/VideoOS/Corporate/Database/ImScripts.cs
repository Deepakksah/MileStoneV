using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Scripts;

namespace VideoOS.Corporate.Database;

public class ImScripts : ScriptListFactoryBase
{
	public ImScripts(ILogger logger)
		: base(logger, "VideoOS.Database", "VideoOS.Corporate.Database.Scripts.IM")
	{
	}

	public override IReadOnlyList<ScriptBase> GetUpgradeScripts()
	{
		return new List<ScriptBase> { CreateSqlScript(new Version("23.1.01"), "SIDToString") };
	}

	public override IReadOnlyList<ScriptBase> GetBaselineScripts()
	{
		return new List<ScriptBase>();
	}
}

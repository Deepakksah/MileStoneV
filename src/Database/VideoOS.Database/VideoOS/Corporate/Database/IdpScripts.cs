using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Scripts;

namespace VideoOS.Corporate.Database;

public class IdpScripts : ScriptListFactoryBase
{
	public IdpScripts(ILogger logger)
		: base(logger, "VideoOS.Database", "VideoOS.Corporate.Database.Scripts.IDP")
	{
	}

	public override IReadOnlyList<ScriptBase> GetUpgradeScripts()
	{
		return new List<ScriptBase>
		{
			CreateSqlScript(new Version("23.1.01"), "SIDToString"),
			CreateSqlScript(new Version("23.1.02"), "ConfigurationDbScript"),
			CreateSqlScript(new Version("23.1.03"), "CorporateDataMigration"),
			CreateSqlScript(new Version("23.1.04"), "ApplicationDbScript"),
			CreateSqlScript(new Version("23.1.05"), "PersistedGrantDbScript"),
			CreateSqlScript(new Version("23.1.06"), "DataMigration")
		};
	}

	public override IReadOnlyList<ScriptBase> GetBaselineScripts()
	{
		return new List<ScriptBase>
		{
			CreateBaselineScript("Baseline.Tables"),
			CreateBaselineScript("Baseline.Data"),
			CreateBaselineScript("Baseline.Indexes"),
			CreateBaselineScript("Baseline.ForeignKeys"),
			CreateBaselineScript("Baseline.UserDefinedFunctions")
		};
	}
}

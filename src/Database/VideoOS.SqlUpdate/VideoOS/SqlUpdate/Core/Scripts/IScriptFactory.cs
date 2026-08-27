using System;

namespace VideoOS.SqlUpdate.Core.Scripts;

public interface IScriptFactory
{
	SqlScript CreateEmbedded(string fullyQualifiedScriptName, Version version = null, bool isUpgradeScript = false, bool isTempScript = false, bool supportsTransactions = false);
}

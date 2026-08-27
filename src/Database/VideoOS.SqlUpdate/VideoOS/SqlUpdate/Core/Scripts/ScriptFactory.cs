using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Statics;

namespace VideoOS.SqlUpdate.Core.Scripts;

public class ScriptFactory : IScriptFactory
{
	private readonly IAssemblyWrapper _assemblyWrapper;

	public ScriptFactory(IAssemblyWrapper assemblyWrapper)
	{
		_assemblyWrapper = assemblyWrapper;
	}

	public SqlScript CreateEmbedded(string fullyQualifiedScriptName, Version version = null, bool isUpgradeScript = false, bool isTempScript = false, bool supportsTransactions = false)
	{
		if (string.IsNullOrEmpty(fullyQualifiedScriptName))
		{
			throw new ArgumentNullException("fullyQualifiedScriptName");
		}
		if (isUpgradeScript && version == null)
		{
			throw new ArgumentNullException("version");
		}
		Stream stream = _assemblyWrapper.GetManifestResourceStream(fullyQualifiedScriptName) ?? throw new SqlUpdateException(Messages.EmbeddedScriptNotFound(fullyQualifiedScriptName));
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
		{
			string text = null;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
				{
					list.Add(stringBuilder.ToString());
					stringBuilder.Clear();
				}
				else if (!text.StartsWith("--") && !string.IsNullOrEmpty(text))
				{
					stringBuilder.AppendLine(text);
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			list.Add(stringBuilder.ToString());
		}
		return new SqlScript(fullyQualifiedScriptName, list, version, isTempScript, isUpgradeScript, supportsTransactions);
	}
}

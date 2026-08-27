using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Common.Utilities;

public abstract class Rules
{
	private Rules successor;

	private HashSet<string> forceNewFileNodePaths;

	public abstract Version ConfigurationFileVersion { get; }

	protected Rules()
	{
		forceNewFileNodePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	}

	public void SetSuccessor(Rules successorRules)
	{
		successor = successorRules;
	}

	public bool UseValueFromNewFile(string newFileNodePath)
	{
		if (forceNewFileNodePaths.Contains(newFileNodePath))
		{
			return true;
		}
		if (successor != null)
		{
			return successor.UseValueFromNewFile(newFileNodePath);
		}
		return false;
	}

	protected void RegisterNodeToOverwrite(string nodePath)
	{
		forceNewFileNodePaths.Add(nodePath);
	}
}

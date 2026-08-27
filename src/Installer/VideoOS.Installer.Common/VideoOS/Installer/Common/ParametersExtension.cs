using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Common;

public static class ParametersExtension
{
	public static void Merge(this Dictionary<string, string> @this, Dictionary<string, string> mergeParameters)
	{
		if (@this == null)
		{
			throw new ArgumentNullException("this");
		}
		if (mergeParameters == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> mergeParameter in mergeParameters)
		{
			@this[mergeParameter.Key] = mergeParameter.Value;
		}
	}
}

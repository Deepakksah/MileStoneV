using System;

namespace VideoOS.Management.Server;

public static class GeminiValidation
{
	public static void CheckForNull(object var, string varName)
	{
		if (var == null)
		{
			throw new ArgumentNullException(varName);
		}
	}
}

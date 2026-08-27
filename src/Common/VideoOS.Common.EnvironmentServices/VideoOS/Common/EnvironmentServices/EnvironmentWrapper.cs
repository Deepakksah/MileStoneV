using System;

namespace VideoOS.Common.EnvironmentServices;

public class EnvironmentWrapper : IEnvironmentService
{
	public void Exit(int exitCode)
	{
		Environment.Exit(exitCode);
	}
}

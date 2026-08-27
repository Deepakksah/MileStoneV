using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class SystemEnvironment
{
	public static SystemEnvironment Instance { get; } = new SystemEnvironment();

	public string OsVersion => Environment.OSVersion.ToString();

	private SystemEnvironment()
	{
	}
}

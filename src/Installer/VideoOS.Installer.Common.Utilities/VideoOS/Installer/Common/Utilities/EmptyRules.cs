using System;

namespace VideoOS.Installer.Common.Utilities;

public class EmptyRules : Rules
{
	public override Version ConfigurationFileVersion => new Version(999, 999);
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
[ExcludeFromCodeCoverage]
public class FileSecurityWrapper : IFileSecurity
{
	public FileSecurity Inner { get; }

	public FileSecurityWrapper(FileSecurity inner)
	{
		Inner = inner;
	}

	public void AddAccessRule(FileSystemAccessRule accesRule)
	{
		Inner.AddAccessRule(accesRule);
	}

	public bool RemoveAccessRule(FileSystemAccessRule accessRule)
	{
		return Inner.RemoveAccessRule(accessRule);
	}

	public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
	{
		Inner.SetAccessRuleProtection(isProtected, preserveInheritance);
	}

	public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		return Inner.GetAccessRules(includeExplicit, includeInherited, targetType);
	}
}

using System;
using System.Security.AccessControl;

namespace VideoOS.Common.EnvironmentServices;

public interface IFileSecurity
{
	void AddAccessRule(FileSystemAccessRule accesRule);

	bool RemoveAccessRule(FileSystemAccessRule accessRule);

	AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType);

	void SetAccessRuleProtection(bool isProtected, bool preserveInheritance);
}

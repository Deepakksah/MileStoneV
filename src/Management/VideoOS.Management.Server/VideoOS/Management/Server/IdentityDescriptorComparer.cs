using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class IdentityDescriptorComparer : IComparer<IdentityDescriptor>, IEqualityComparer<IdentityDescriptor>
{
	private static IdentityDescriptorComparer s_instance = new IdentityDescriptorComparer();

	public static IdentityDescriptorComparer Instance => s_instance;

	public int GetHashCode(IdentityDescriptor obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		return obj.IdentityType.GetHashCode() + obj.Identifier.GetHashCode();
	}

	public int Compare(IdentityDescriptor x, IdentityDescriptor y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x == null && y != null)
		{
			return -1;
		}
		if (x != null && y == null)
		{
			return 1;
		}
		int num = string.Compare(x.Identifier, y.Identifier, StringComparison.OrdinalIgnoreCase);
		if (num != 0)
		{
			return num;
		}
		return string.Compare(x.IdentityType, y.IdentityType, StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(IdentityDescriptor x, IdentityDescriptor y)
	{
		return Compare(x, y) == 0;
	}
}

using System;

namespace VideoOS.Platform.AccessControl.Internal;

public class UserDefinedCategory : Category
{
	public UserDefinedCategory(Guid id, string name, Type accessControlType)
		: base(id, name, accessControlType)
	{
	}

	public void UpdateName(string name)
	{
		base.Name = name;
	}
}

using System;
using System.Collections.Generic;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Validators;

public class CommonValidator : ICommonValidator
{
	public void ValidatePath(ConfigurationItemPath path)
	{
		path.NullCheck("", "ValidatePath", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\Validators\\CommonValidator.cs", 13);
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal) && !path.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	public void ValidateItemListPath(string pathString, ISet<string> itemTypes, string propertyKey, out ConfigurationItemPath path)
	{
		path = null;
		if (string.IsNullOrEmpty(pathString))
		{
			return;
		}
		path = ConfigurationItemPath.CreatePath(pathString);
		string text = path.ItemType;
		if (text != "System" && text != "AccessControlRuleGroup" && (string.IsNullOrEmpty(path.Id) || path.Id == Guid.Empty.ToString()))
		{
			if (!pathString.EndsWith("Folder", StringComparison.Ordinal))
			{
				throw new ArgumentException("Path invalid, when ID=empty path is expected to end with 'Folder': " + pathString, propertyKey);
			}
			text = pathString.Substring(0, pathString.Length - 6).TrimStart('/');
		}
		if (itemTypes.Count <= 0 || itemTypes.Contains(text))
		{
			return;
		}
		throw new ArgumentException("Path invalid, ItemType not allowed for this property: " + pathString, propertyKey);
	}

	public void ValidateItemListPath(string pathString, ISet<string> itemTypes, string propertyKey)
	{
		ValidateItemListPath(pathString, itemTypes, propertyKey, out var _);
	}
}

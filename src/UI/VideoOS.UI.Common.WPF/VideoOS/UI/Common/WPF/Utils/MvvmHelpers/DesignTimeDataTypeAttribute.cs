using System;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class DesignTimeDataTypeAttribute : Attribute
{
	private readonly string _originalTypeFullName;

	public string OriginalTypeFullName => _originalTypeFullName;

	public DesignTimeDataTypeAttribute(string originalTypeFullName)
	{
		_originalTypeFullName = originalTypeFullName;
	}
}

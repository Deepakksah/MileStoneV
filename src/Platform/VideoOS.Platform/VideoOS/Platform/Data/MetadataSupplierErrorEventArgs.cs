using System;

namespace VideoOS.Platform.Data;

public class MetadataSupplierErrorEventArgs : EventArgs
{
	private readonly string _errorText;

	public string ErrorText => _errorText;

	public MetadataSupplierErrorEventArgs(string errorText)
	{
		_errorText = errorText;
	}
}

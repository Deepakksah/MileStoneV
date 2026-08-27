using System;

namespace VideoOS.Platform.Data.MetadataSuppliersInternal;

public class InternalMetadataSupplierErrorEventArgs : EventArgs
{
	private readonly string _errorText;

	public string ErrorText => _errorText;

	public InternalMetadataSupplierErrorEventArgs(string errorText)
	{
		_errorText = errorText;
	}
}

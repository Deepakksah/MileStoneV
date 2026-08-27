using System;

namespace VideoOS.Platform.Data.MetadataSuppliersInternal;

public class InternalMetadataSupplierMetadataInvalidatedEventArgs : EventArgs
{
	public DateTime InvalidDataDateTime { get; set; }

	public InternalMetadataSupplierMetadataInvalidatedEventArgs(DateTime invalidDataDateTime)
	{
		InvalidDataDateTime = invalidDataDateTime;
	}
}

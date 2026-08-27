using System;

namespace VideoOS.Platform.Data;

public class MetadataSupplierMetadataInvalidatedEventArgs : EventArgs
{
	public DateTime InvalidDataDateTime { get; public set; }

	public MetadataSupplierMetadataInvalidatedEventArgs(DateTime invalidDataDateTime)
	{
		InvalidDataDateTime = invalidDataDateTime;
	}
}

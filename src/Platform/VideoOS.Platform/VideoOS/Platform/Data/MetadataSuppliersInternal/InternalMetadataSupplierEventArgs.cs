using System;
using VideoOS.Platform.Metadata;

namespace VideoOS.Platform.Data.MetadataSuppliersInternal;

public class InternalMetadataSupplierEventArgs : EventArgs
{
	private readonly MetadataContent _data;

	private readonly DateTime _time;

	public MetadataContent Data => _data;

	public DateTime Time => _time;

	public DateTime? PreviousTime { get; private set; }

	public DateTime? NextTime { get; private set; }

	public InternalMetadataSupplierEventArgs(MetadataContent data, DateTime time, DateTime? previousTime, DateTime? nextTime)
	{
		_data = data;
		_time = time;
		PreviousTime = previousTime;
		NextTime = nextTime;
	}
}

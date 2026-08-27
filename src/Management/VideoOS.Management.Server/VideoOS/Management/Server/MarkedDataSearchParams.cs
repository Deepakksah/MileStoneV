using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class MarkedDataSearchParams : SearchFilter
{
	public string[] DeviceIds { get; set; }

	public DateTime CreatedFromTime { get; set; }

	public DateTime CreatedToTime { get; set; }

	public DateTime MarkedDataFromTime { get; set; }

	public DateTime MarkedDataToTime { get; set; }

	public DateTime TagTimeFromTime { get; set; }

	public DateTime TagTimeToTime { get; set; }

	public DateTime RetentionExpireFromTime { get; set; }

	public DateTime RetentionExpireToTime { get; set; }

	public int PageIndex { get; set; }

	public int PageSize { get; set; }

	public string SortOrder { get; set; }

	public bool SortAscending { get; set; }

	public MarkedDataDefinitions.MarkedDataTypes MarkedDataType { get; set; }
}

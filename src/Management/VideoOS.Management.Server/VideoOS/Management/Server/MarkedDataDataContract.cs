using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public class MarkedDataDataContract
{
	public string Id { get; set; }

	public DateTime LastModified { get; set; }

	public DateTime Created { get; set; }

	public string Reference { get; set; }

	public string UserName { get; set; }

	public string Header { get; set; }

	public string Description { get; set; }

	public int MarkedDataType { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime TagTime { get; set; }

	public DateTime EndTime { get; set; }

	public bool UseRetention { get; set; }

	public int RetentionType { get; set; }

	public int RetentionUnits { get; set; }

	public DateTime RetentionExpire { get; set; }

	public long RetentionSize { get; set; }

	public string[] DeviceIds { get; set; }

	public IEnumerable<StickerSize2> StickerSizes { get; set; }
}

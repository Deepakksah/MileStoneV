using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class AlarmLineDetail
{
	public string AssignedTo { get; set; }

	public Guid CameraId { get; set; }

	public ushort Category { get; set; }

	public string CategoryName { get; set; }

	public string Description { get; set; }

	public Guid Id { get; set; }

	public string Message { get; set; }

	public string Name { get; set; }

	public ushort Priority { get; set; }

	public string PriorityName { get; set; }

	public Guid SourceId { get; set; }

	public string SourceName { get; set; }

	public ushort State { get; set; }

	public string StateName { get; set; }

	public DateTime Timestamp { get; set; }

	public string Type { get; set; }

	public List<Tuple<Guid, string>> RelatedCameras { get; set; }

	public Guid MessageId { get; set; }
}

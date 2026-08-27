using System;

namespace VideoOS.Platform.Data;

public class Bookmark
{
	public FQID BookmarkFQID { get; set; }

	public string Description { get; set; }

	public string Header { get; set; }

	public string Reference { get; set; }

	public DateTime TimeBegin { get; set; }

	public DateTime TimeEnd { get; set; }

	public DateTime TimeTrigged { get; set; }

	public string User { get; set; }

	public Item GetDeviceItem()
	{
		object obj = GetDeviceItem(BookmarkFQID.ParentId, Kind.Camera) ?? GetDeviceItem(BookmarkFQID.ParentId, Kind.Microphone);
		if (obj == null)
		{
			obj = GetDeviceItem(BookmarkFQID.ParentId, Kind.Speaker);
		}
		return (Item)obj;
	}

	private Item GetDeviceItem(Guid id, Guid kind)
	{
		if (BookmarkFQID.ServerId.UserContext != null)
		{
			return BookmarkFQID.ServerId.UserContext.Configuration.GetItem(id, kind);
		}
		return Configuration.Instance.GetItem(id, kind);
	}
}

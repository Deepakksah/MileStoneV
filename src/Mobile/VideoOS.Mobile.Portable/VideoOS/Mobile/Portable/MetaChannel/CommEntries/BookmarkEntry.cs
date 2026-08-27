using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class BookmarkEntry : IBookmarkEntry
{
	public Guid Id { get; set; }

	public string Description { get; set; }

	public string Name { get; set; }

	public string Reference { get; set; }

	public DateTime TimeBegin { get; set; }

	public DateTime TimeEnd { get; set; }

	public DateTime TimeTrigged { get; set; }

	public string User { get; set; }

	public (Guid Id, string Name, SerializableItemTypes Type) RelatedDevice { get; set; }
}

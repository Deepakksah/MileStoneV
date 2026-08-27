using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IBookmarkEntry
{
	string Description { get; set; }

	string Name { get; set; }

	Guid Id { get; set; }

	string Reference { get; set; }

	(Guid Id, string Name, SerializableItemTypes Type) RelatedDevice { get; set; }

	DateTime TimeBegin { get; set; }

	DateTime TimeEnd { get; set; }

	DateTime TimeTrigged { get; set; }

	string User { get; set; }
}

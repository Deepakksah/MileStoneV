using System.Collections.Generic;
using VideoOS.Mobile.Portable.ViewGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableItemTypesHelper
{
	public static readonly Dictionary<SerializableItemTypes, ViewItemType> SerializableItemTypeToViewItemType = new Dictionary<SerializableItemTypes, ViewItemType>
	{
		{
			SerializableItemTypes.Folder,
			ViewItemType.Group
		},
		{
			SerializableItemTypes.View,
			ViewItemType.View
		},
		{
			SerializableItemTypes.Camera,
			ViewItemType.Camera
		},
		{
			SerializableItemTypes.Carousel,
			ViewItemType.Carousel
		},
		{
			SerializableItemTypes.Microphone,
			ViewItemType.Microphone
		},
		{
			SerializableItemTypes.Speaker,
			ViewItemType.Speaker
		},
		{
			SerializableItemTypes.Metadata,
			ViewItemType.Metadata
		}
	};

	public static readonly Dictionary<ViewItemType, SerializableItemTypes> ViewItemTypeToSerializableItemType = new Dictionary<ViewItemType, SerializableItemTypes>
	{
		{
			ViewItemType.Group,
			SerializableItemTypes.Folder
		},
		{
			ViewItemType.View,
			SerializableItemTypes.View
		},
		{
			ViewItemType.Camera,
			SerializableItemTypes.Camera
		},
		{
			ViewItemType.Carousel,
			SerializableItemTypes.Carousel
		},
		{
			ViewItemType.Microphone,
			SerializableItemTypes.Microphone
		},
		{
			ViewItemType.Speaker,
			SerializableItemTypes.Speaker
		},
		{
			ViewItemType.Metadata,
			SerializableItemTypes.Metadata
		}
	};
}

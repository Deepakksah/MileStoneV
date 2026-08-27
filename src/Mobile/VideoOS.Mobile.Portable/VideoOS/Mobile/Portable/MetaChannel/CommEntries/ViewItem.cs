using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.ViewGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ViewItem
{
	public enum ItemType
	{
		[Description("Folder")]
		Folder,
		[Description("View")]
		View,
		[Description("Camera")]
		Camera,
		[Description("Carousel")]
		Carousel,
		[Description("Map")]
		Map,
		[Description("Microphone")]
		Microphone,
		[Description("Speaker")]
		Speaker,
		[Description("Metadata")]
		Metadata
	}

	public const string TypeAttribute = "Type";

	public const string NameAttribute = "Name";

	public const string IdAttribute = "Id";

	public const string SubsCountAttribute = "SubsCount";

	public const string MapsCountAttribute = "MapsCount";

	public const string CarouselsCountAttribute = "CarouselsCount";

	public const string IsValidName = "IsValid";

	public const string CameraTimeout = "CameraTimeout";

	private ItemType _type;

	[JsonProperty("Type")]
	[XmlAttribute(AttributeName = "Type")]
	public string Type
	{
		get
		{
			return EnumsHelper.GetDescription(_type);
		}
		set
		{
			_type = EnumsHelper.EnumConverter<ItemType>(value);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public ItemType TypeItem => _type;

	[JsonProperty("Name")]
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[JsonProperty("Id")]
	[XmlAttribute(AttributeName = "Id")]
	public Guid ID { get; set; }

	[JsonProperty("SubsCount")]
	[XmlAttribute(AttributeName = "SubsCount")]
	public string SubElementsCount { get; set; }

	[JsonProperty("CarouselsCount")]
	[XmlAttribute(AttributeName = "CarouselsCount")]
	public string CarouselsElementsCount { get; set; }

	[JsonProperty("SrcWidth")]
	[XmlAttribute(AttributeName = "SrcWidth")]
	public string SrcWidth { get; set; }

	[JsonProperty("SrcHeight")]
	[XmlAttribute(AttributeName = "SrcHeight")]
	public string SrcHeight { get; set; }

	[JsonProperty("IsValid")]
	[XmlAttribute(AttributeName = "IsValid")]
	public string IsValid { get; set; }

	[JsonProperty("CameraTimeout")]
	[XmlAttribute(AttributeName = "CameraTimeout")]
	public int Timeout { get; set; }

	public ViewItem()
	{
	}

	public ViewItem(IViewGroupItem item, IThumbnailManager iThumbsManager)
	{
		if (item == null)
		{
			return;
		}
		_type = (ItemType)item.ItemType;
		Name = item.ItemName;
		ID = item.ItemId;
		if (item.CameraId != Guid.Empty)
		{
			ID = item.CameraId;
		}
		if (((ViewGroupTree)item).CameraTimeout != 0)
		{
			Timeout = ((ViewGroupTree)item).CameraTimeout;
		}
		if (item is IViewGroupTree viewGroupTree && viewGroupTree.GetMembersList().Any())
		{
			SubElementsCount = viewGroupTree.GetMembersList().Count.ToString();
			CarouselsElementsCount = (from i in viewGroupTree.GetMembersList()
				where i != null && i.ItemType == ViewItemType.Carousel
				select i).Count().ToString();
		}
		if (_type == ItemType.Camera)
		{
			try
			{
				IThumbnail thumbnail = iThumbsManager[item.CameraId];
				SrcWidth = thumbnail.Width.ToString();
				SrcHeight = thumbnail.Height.ToString();
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "ViewItem.ViewItem(ViewGroupTree item)", ex.Message);
			}
		}
	}
}

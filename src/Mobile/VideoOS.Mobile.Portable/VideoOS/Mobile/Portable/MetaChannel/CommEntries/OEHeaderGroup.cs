using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.OEGroupItem;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class OEHeaderGroup : OEGroup
{
	public enum OEGroupType
	{
		[Description("OutputsGroup")]
		OutputsGroup,
		[Description("EventsGroup")]
		EventsGroup
	}

	public const string OEHeaderGroupItem = "OEHeaderGroup";

	public const string GroupsNode = "OEGroup";

	[JsonProperty("OEGroup")]
	[XmlElement(ElementName = "OEGroup")]
	public List<OEGroup> OEGroups;

	private OEGroupType _type;

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
			_type = EnumsHelper.EnumConverter<OEGroupType>(value);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public new Guid CameraId { get; set; }

	public OEGroupType GetOeGroupType()
	{
		return _type;
	}

	public OEHeaderGroup()
	{
		Initialize();
	}

	public OEHeaderGroup(IOEGroupItem groupItem)
		: this()
	{
		if (groupItem == null || !(groupItem is OEGroupTree oEGroupTree))
		{
			return;
		}
		if (oEGroupTree.ItemType == OEItemType.OutputsGroup)
		{
			_type = OEGroupType.OutputsGroup;
		}
		else
		{
			if (oEGroupTree.ItemType != OEItemType.EventsGroup)
			{
				return;
			}
			_type = OEGroupType.EventsGroup;
		}
		base.Name = oEGroupTree.ItemName;
		base.SubElementsCount = oEGroupTree.GetMembersCount().ToString();
		foreach (IOEGroupTree members in groupItem.GetMembersList())
		{
			if (members is OEGroupTree oEGroupTree2)
			{
				if (oEGroupTree2.ItemType == OEItemType.Output || oEGroupTree2.ItemType == OEItemType.Event)
				{
					OEItems.Add(new OEItem(oEGroupTree2));
				}
				else
				{
					OEGroups.Add(new OEGroup(oEGroupTree2));
				}
			}
		}
	}

	private void Initialize()
	{
		OEGroups = new List<OEGroup>();
		OEItems = new List<OEItem>();
	}
}

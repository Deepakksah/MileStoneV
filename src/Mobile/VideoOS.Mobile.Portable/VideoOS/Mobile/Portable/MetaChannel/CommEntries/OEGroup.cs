using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.OEGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class OEGroup
{
	public const string ItemNode = "OEItem";

	[JsonProperty("OEItem")]
	[XmlElement(ElementName = "OEItem")]
	public List<OEItem> OEItems;

	public const string SubsCountAttribute = "SubsCount";

	[JsonProperty("Name")]
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[JsonProperty("CameraId")]
	[XmlAttribute(AttributeName = "CameraId")]
	public Guid CameraId { get; set; }

	[JsonProperty("SubsCount")]
	[XmlAttribute(AttributeName = "SubsCount")]
	public string SubElementsCount { get; set; }

	public OEGroup()
	{
		Initialize();
	}

	public OEGroup(IOEGroupItem groupItem)
		: this()
	{
		if (groupItem == null || !(groupItem is OEGroupTree oEGroupTree))
		{
			return;
		}
		Name = oEGroupTree.ItemName;
		CameraId = oEGroupTree.CameraId;
		SubElementsCount = oEGroupTree.GetMembersCount().ToString();
		foreach (IOEGroupTree members in groupItem.GetMembersList())
		{
			if (!(members is OEGroupTree) || (members as OEGroupTree).GetMembersCount() <= 0)
			{
				OEItems.Add(new OEItem(members));
			}
		}
	}

	private void Initialize()
	{
		OEItems = new List<OEItem>();
	}
}

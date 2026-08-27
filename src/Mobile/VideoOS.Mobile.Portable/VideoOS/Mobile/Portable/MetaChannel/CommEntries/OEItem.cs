using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.OEGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class OEItem
{
	[JsonProperty("Name")]
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[JsonProperty("ObjectId")]
	[XmlAttribute(AttributeName = "ObjectId")]
	public Guid ObjectId { get; set; }

	[JsonProperty("CameraId")]
	[XmlAttribute(AttributeName = "CameraId")]
	public Guid CameraId { get; set; }

	[JsonProperty("CanActivate")]
	[XmlAttribute(AttributeName = "CanActivate")]
	public bool CanActivate { get; set; }

	public OEItem()
	{
	}

	public OEItem(IOEGroupTree item)
	{
		if (item != null)
		{
			Name = item.ItemName;
			ObjectId = item.ObjectId;
			CanActivate = item.CanActivate;
			if (item.CameraId != Guid.Empty)
			{
				CameraId = item.CameraId;
			}
		}
	}
}

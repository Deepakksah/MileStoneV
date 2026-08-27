using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging;

[Serializable]
[DataContract]
public class ViewItemInformation
{
	[DataMember]
	public MyRectangle Rectangle { get; private set; }

	[DataMember]
	public Guid ViewItemType { get; private set; }

	[DataMember]
	public MyKeyValue[] Attributes { get; private set; }

	[DataMember]
	public MyKeyValue[] Properties { get; private set; }

	public ViewItemInformation()
	{
	}

	public ViewItemInformation(Rectangle rectangle, Guid viewItemType, Dictionary<string, string> properties, Dictionary<string, string> attributes)
	{
		Rectangle = new MyRectangle
		{
			X = rectangle.X,
			Y = rectangle.Y,
			Width = rectangle.Width,
			Height = rectangle.Height
		};
		ViewItemType = viewItemType;
		List<MyKeyValue> list = new List<MyKeyValue>();
		if (properties != null)
		{
			foreach (string key in properties.Keys)
			{
				list.Add(new MyKeyValue
				{
					Key = key,
					Value = properties[key]
				});
			}
		}
		Properties = list.ToArray();
		List<MyKeyValue> list2 = new List<MyKeyValue>();
		if (attributes != null)
		{
			foreach (string key2 in attributes.Keys)
			{
				list2.Add(new MyKeyValue
				{
					Key = key2,
					Value = attributes[key2]
				});
			}
		}
		Attributes = list2.ToArray();
	}

	public string ToXml()
	{
		return ViewItemUtility.ConvertToXml(this, includeTopNode: true);
	}
}

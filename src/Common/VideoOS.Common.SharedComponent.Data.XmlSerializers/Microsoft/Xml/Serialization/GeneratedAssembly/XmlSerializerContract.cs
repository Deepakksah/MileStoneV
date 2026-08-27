using System;
using System.Collections;
using System.Xml.Serialization;
using VideoOS.Common.SharedComponent.Data;
using VideoOS.Common.SharedComponent.Data.ViewLayout;

namespace Microsoft.Xml.Serialization.GeneratedAssembly;

public class XmlSerializerContract : XmlSerializerImplementation
{
	private Hashtable readMethods;

	private Hashtable writeMethods;

	private Hashtable typedSerializers;

	public override XmlSerializationReader Reader => new XmlSerializationReader1();

	public override XmlSerializationWriter Writer => new XmlSerializationWriter1();

	public override Hashtable ReadMethods
	{
		get
		{
			if (readMethods == null)
			{
				Hashtable hashtable = new Hashtable();
				hashtable["VideoOS.Common.SharedComponent.Data.Group::"] = "Read8_Group";
				hashtable["VideoOS.Common.SharedComponent.Data.PropertyBase::"] = "Read9_PropertyBase";
				hashtable["VideoOS.Common.SharedComponent.Data.View::"] = "Read10_View";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewGroup::"] = "Read11_ViewGroup";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewItem2::"] = "Read12_ViewItem2";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout::"] = "Read13_ViewLayout";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewItem::"] = "Read14_ViewItem";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayoutGroup::"] = "Read15_ViewLayoutGroup";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayoutGroupConstants::"] = "Read16_ViewLayoutGroupConstants";
				if (readMethods == null)
				{
					readMethods = hashtable;
				}
			}
			return readMethods;
		}
	}

	public override Hashtable WriteMethods
	{
		get
		{
			if (writeMethods == null)
			{
				Hashtable hashtable = new Hashtable();
				hashtable["VideoOS.Common.SharedComponent.Data.Group::"] = "Write8_Group";
				hashtable["VideoOS.Common.SharedComponent.Data.PropertyBase::"] = "Write9_PropertyBase";
				hashtable["VideoOS.Common.SharedComponent.Data.View::"] = "Write10_View";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewGroup::"] = "Write11_ViewGroup";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewItem2::"] = "Write12_ViewItem2";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout::"] = "Write13_ViewLayout";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewItem::"] = "Write14_ViewItem";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayoutGroup::"] = "Write15_ViewLayoutGroup";
				hashtable["VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayoutGroupConstants::"] = "Write16_ViewLayoutGroupConstants";
				if (writeMethods == null)
				{
					writeMethods = hashtable;
				}
			}
			return writeMethods;
		}
	}

	public override Hashtable TypedSerializers
	{
		get
		{
			if (typedSerializers == null)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("VideoOS.Common.SharedComponent.Data.ViewGroup::", new ViewGroupSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.View::", new ViewSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayoutGroup::", new ViewLayoutGroupSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayoutGroupConstants::", new ViewLayoutGroupConstantsSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout::", new ViewLayoutSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.ViewItem2::", new ViewItem2Serializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.Group::", new GroupSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.ViewLayout.ViewItem::", new ViewItemSerializer());
				hashtable.Add("VideoOS.Common.SharedComponent.Data.PropertyBase::", new PropertyBaseSerializer());
				if (typedSerializers == null)
				{
					typedSerializers = hashtable;
				}
			}
			return typedSerializers;
		}
	}

	public override bool CanSerialize(Type type)
	{
		if (type == typeof(Group))
		{
			return true;
		}
		if (type == typeof(PropertyBase))
		{
			return true;
		}
		if (type == typeof(View))
		{
			return true;
		}
		if (type == typeof(ViewGroup))
		{
			return true;
		}
		if (type == typeof(ViewItem2))
		{
			return true;
		}
		if (type == typeof(ViewLayout))
		{
			return true;
		}
		if (type == typeof(ViewItem))
		{
			return true;
		}
		if (type == typeof(ViewLayoutGroup))
		{
			return true;
		}
		if (type == typeof(ViewLayoutGroupConstants))
		{
			return true;
		}
		return false;
	}

	public override XmlSerializer GetSerializer(Type type)
	{
		if (type == typeof(Group))
		{
			return new GroupSerializer();
		}
		if (type == typeof(PropertyBase))
		{
			return new PropertyBaseSerializer();
		}
		if (type == typeof(View))
		{
			return new ViewSerializer();
		}
		if (type == typeof(ViewGroup))
		{
			return new ViewGroupSerializer();
		}
		if (type == typeof(ViewItem2))
		{
			return new ViewItem2Serializer();
		}
		if (type == typeof(ViewLayout))
		{
			return new ViewLayoutSerializer();
		}
		if (type == typeof(ViewItem))
		{
			return new ViewItemSerializer();
		}
		if (type == typeof(ViewLayoutGroup))
		{
			return new ViewLayoutGroupSerializer();
		}
		if (type == typeof(ViewLayoutGroupConstants))
		{
			return new ViewLayoutGroupConstantsSerializer();
		}
		return null;
	}
}

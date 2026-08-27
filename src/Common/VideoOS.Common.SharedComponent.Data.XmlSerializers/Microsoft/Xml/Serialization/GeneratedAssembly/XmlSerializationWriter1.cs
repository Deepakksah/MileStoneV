using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.SharedComponent.Data;
using VideoOS.Common.SharedComponent.Data.ViewLayout;

namespace Microsoft.Xml.Serialization.GeneratedAssembly;

public class XmlSerializationWriter1 : XmlSerializationWriter
{
	public void Write8_Group(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("Group", "");
			return;
		}
		TopLevelElement();
		WriteSerializable((Group)o, "Group", "", isNullable: true, wrapped: true);
	}

	public void Write9_PropertyBase(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("PropertyBase", "");
			return;
		}
		TopLevelElement();
		WriteSerializable((PropertyBase)o, "PropertyBase", "", isNullable: true, wrapped: true);
	}

	public void Write10_View(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("View", "");
			return;
		}
		TopLevelElement();
		WriteSerializable((View)o, "View", "", isNullable: true, wrapped: true);
	}

	public void Write11_ViewGroup(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("ViewGroup", "");
			return;
		}
		TopLevelElement();
		WriteSerializable((ViewGroup)o, "ViewGroup", "", isNullable: true, wrapped: true);
	}

	public void Write12_ViewItem2(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("ViewItem2", "");
			return;
		}
		TopLevelElement();
		WriteSerializable((ViewItem2)o, "ViewItem2", "", isNullable: true, wrapped: true);
	}

	public void Write13_ViewLayout(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("ViewLayout", "");
			return;
		}
		TopLevelElement();
		Write5_ViewLayout("ViewLayout", "", (ViewLayout)o, isNullable: true, needType: false);
	}

	public void Write14_ViewItem(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("ViewItem", "");
			return;
		}
		TopLevelElement();
		Write4_ViewItem("ViewItem", "", (ViewItem)o, isNullable: true, needType: false);
	}

	public void Write15_ViewLayoutGroup(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("ViewLayoutGroup", "");
			return;
		}
		TopLevelElement();
		Write6_ViewLayoutGroup("ViewLayoutGroup", "", (ViewLayoutGroup)o, isNullable: true, needType: false);
	}

	public void Write16_ViewLayoutGroupConstants(object o)
	{
		WriteStartDocument();
		if (o == null)
		{
			WriteNullTagLiteral("ViewLayoutGroupConstants", "");
			return;
		}
		TopLevelElement();
		Write7_ViewLayoutGroupConstants("ViewLayoutGroupConstants", "", (ViewLayoutGroupConstants)o, isNullable: true, needType: false);
	}

	private void Write7_ViewLayoutGroupConstants(string n, string ns, ViewLayoutGroupConstants o, bool isNullable, bool needType)
	{
		if (o == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(n, ns);
			}
			return;
		}
		if (!needType)
		{
			Type type = o.GetType();
			if (!(type == typeof(ViewLayoutGroupConstants)))
			{
				throw CreateUnknownTypeException(o);
			}
		}
		WriteStartElement(n, ns, o, writePrefixed: false, null);
		if (needType)
		{
			WriteXsiType("ViewLayoutGroupConstants", "");
		}
		WriteEndElement(o);
	}

	private void Write6_ViewLayoutGroup(string n, string ns, ViewLayoutGroup o, bool isNullable, bool needType)
	{
		if (o == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(n, ns);
			}
			return;
		}
		if (!needType)
		{
			Type type = o.GetType();
			if (!(type == typeof(ViewLayoutGroup)))
			{
				throw CreateUnknownTypeException(o);
			}
		}
		WriteStartElement(n, ns, o, writePrefixed: false, null);
		if (needType)
		{
			WriteXsiType("ViewLayoutGroup", "");
		}
		WriteElementStringRaw("Id", "", XmlConvert.ToString(o.Id));
		WriteElementString("Name", "", o.Name);
		WriteEndElement(o);
	}

	private void Write4_ViewItem(string n, string ns, ViewItem o, bool isNullable, bool needType)
	{
		if (o == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(n, ns);
			}
			return;
		}
		if (!needType)
		{
			Type type = o.GetType();
			if (!(type == typeof(ViewItem)))
			{
				throw CreateUnknownTypeException(o);
			}
		}
		WriteStartElement(n, ns, o, writePrefixed: false, null);
		if (needType)
		{
			WriteXsiType("ViewItem", "");
		}
		Write2_Point("Position", "", o.Position, needType: false);
		Write3_Size("Size", "", o.Size, needType: false);
		WriteEndElement(o);
	}

	private void Write3_Size(string n, string ns, Size o, bool needType)
	{
		if (!needType)
		{
			Type type = o.GetType();
			if (!(type == typeof(Size)))
			{
				throw CreateUnknownTypeException(o);
			}
		}
		WriteStartElement(n, ns, o, writePrefixed: false, null);
		if (needType)
		{
			WriteXsiType("Size", "");
		}
		WriteElementStringRaw("Width", "", XmlConvert.ToString(o.Width));
		WriteElementStringRaw("Height", "", XmlConvert.ToString(o.Height));
		WriteEndElement(o);
	}

	private void Write2_Point(string n, string ns, Point o, bool needType)
	{
		if (!needType)
		{
			Type type = o.GetType();
			if (!(type == typeof(Point)))
			{
				throw CreateUnknownTypeException(o);
			}
		}
		WriteStartElement(n, ns, o, writePrefixed: false, null);
		if (needType)
		{
			WriteXsiType("Point", "");
		}
		WriteElementStringRaw("X", "", XmlConvert.ToString(o.X));
		WriteElementStringRaw("Y", "", XmlConvert.ToString(o.Y));
		WriteEndElement(o);
	}

	private void Write5_ViewLayout(string n, string ns, ViewLayout o, bool isNullable, bool needType)
	{
		if (o == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(n, ns);
			}
			return;
		}
		if (!needType)
		{
			Type type = o.GetType();
			if (!(type == typeof(ViewLayout)))
			{
				throw CreateUnknownTypeException(o);
			}
		}
		WriteStartElement(n, ns, o, writePrefixed: false, null);
		if (needType)
		{
			WriteXsiType("ViewLayout", "");
		}
		WriteAttribute("ViewLayoutType", "", o.ViewLayoutType);
		WriteAttribute("Id", "", XmlConvert.ToString(o.Id));
		WriteElementStringRaw("ViewLayoutIcon", "", XmlSerializationWriter.FromByteArrayBase64(o.BinaryIcon));
		List<ViewItem> viewItems = o.ViewItems;
		if (viewItems != null)
		{
			WriteStartElement("ViewItems", "", null, writePrefixed: false);
			for (int i = 0; i < ((ICollection)viewItems).Count; i++)
			{
				Write4_ViewItem("ViewItem", "", viewItems[i], isNullable: true, needType: false);
			}
			WriteEndElement();
		}
		WriteEndElement(o);
	}

	protected override void InitCallbacks()
	{
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Security;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.SharedComponent.Data;
using VideoOS.Common.SharedComponent.Data.ViewLayout;

namespace Microsoft.Xml.Serialization.GeneratedAssembly;

public class XmlSerializationReader1 : XmlSerializationReader
{
	private string id2_Item;

	private string id1_Group;

	private string id8_ViewItem;

	private string id5_ViewGroup;

	private string id15_Width;

	private string id9_ViewLayoutGroup;

	private string id18_X;

	private string id12_Name;

	private string id13_Position;

	private string id3_PropertyBase;

	private string id17_Point;

	private string id19_Y;

	private string id4_View;

	private string id10_ViewLayoutGroupConstants;

	private string id22_ViewItems;

	private string id16_Height;

	private string id6_ViewItem2;

	private string id14_Size;

	private string id11_Id;

	private string id7_ViewLayout;

	private string id20_ViewLayoutType;

	private string id21_ViewLayoutIcon;

	public object Read8_Group()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id1_Group || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = (Group)ReadSerializable(new Group());
		}
		else
		{
			UnknownNode(null, ":Group");
		}
		return result;
	}

	public object Read9_PropertyBase()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id3_PropertyBase || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = (PropertyBase)ReadSerializable(new PropertyBase());
		}
		else
		{
			UnknownNode(null, ":PropertyBase");
		}
		return result;
	}

	public object Read10_View()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id4_View || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = (View)ReadSerializable(new View());
		}
		else
		{
			UnknownNode(null, ":View");
		}
		return result;
	}

	public object Read11_ViewGroup()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id5_ViewGroup || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = (ViewGroup)ReadSerializable(new ViewGroup());
		}
		else
		{
			UnknownNode(null, ":ViewGroup");
		}
		return result;
	}

	public object Read12_ViewItem2()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id6_ViewItem2 || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = (ViewItem2)ReadSerializable(new ViewItem2());
		}
		else
		{
			UnknownNode(null, ":ViewItem2");
		}
		return result;
	}

	public object Read13_ViewLayout()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id7_ViewLayout || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = Read5_ViewLayout(isNullable: true, checkType: true);
		}
		else
		{
			UnknownNode(null, ":ViewLayout");
		}
		return result;
	}

	public object Read14_ViewItem()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id8_ViewItem || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = Read4_ViewItem(isNullable: true, checkType: true);
		}
		else
		{
			UnknownNode(null, ":ViewItem");
		}
		return result;
	}

	public object Read15_ViewLayoutGroup()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id9_ViewLayoutGroup || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = Read6_ViewLayoutGroup(isNullable: true, checkType: true);
		}
		else
		{
			UnknownNode(null, ":ViewLayoutGroup");
		}
		return result;
	}

	public object Read16_ViewLayoutGroupConstants()
	{
		object result = null;
		base.Reader.MoveToContent();
		if (base.Reader.NodeType == XmlNodeType.Element)
		{
			if ((object)base.Reader.LocalName != id10_ViewLayoutGroupConstants || (object)base.Reader.NamespaceURI != id2_Item)
			{
				throw CreateUnknownNodeException();
			}
			result = Read7_ViewLayoutGroupConstants(isNullable: true, checkType: true);
		}
		else
		{
			UnknownNode(null, ":ViewLayoutGroupConstants");
		}
		return result;
	}

	private ViewLayoutGroupConstants Read7_ViewLayoutGroupConstants(bool isNullable, bool checkType)
	{
		XmlQualifiedName xmlQualifiedName = (checkType ? GetXsiType() : null);
		bool flag = false;
		if (isNullable)
		{
			flag = ReadNull();
		}
		if (checkType && !(xmlQualifiedName == null) && ((object)xmlQualifiedName.Name != id10_ViewLayoutGroupConstants || (object)xmlQualifiedName.Namespace != id2_Item))
		{
			throw CreateUnknownTypeException(xmlQualifiedName);
		}
		if (flag)
		{
			return null;
		}
		ViewLayoutGroupConstants viewLayoutGroupConstants = new ViewLayoutGroupConstants();
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(viewLayoutGroupConstants);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return viewLayoutGroupConstants;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		int whileIterations = 0;
		int readerCount = base.ReaderCount;
		while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				UnknownNode(viewLayoutGroupConstants, "");
			}
			else
			{
				UnknownNode(viewLayoutGroupConstants, "");
			}
			base.Reader.MoveToContent();
			CheckReaderCount(ref whileIterations, ref readerCount);
		}
		ReadEndElement();
		return viewLayoutGroupConstants;
	}

	private ViewLayoutGroup Read6_ViewLayoutGroup(bool isNullable, bool checkType)
	{
		XmlQualifiedName xmlQualifiedName = (checkType ? GetXsiType() : null);
		bool flag = false;
		if (isNullable)
		{
			flag = ReadNull();
		}
		if (checkType && !(xmlQualifiedName == null) && ((object)xmlQualifiedName.Name != id9_ViewLayoutGroup || (object)xmlQualifiedName.Namespace != id2_Item))
		{
			throw CreateUnknownTypeException(xmlQualifiedName);
		}
		if (flag)
		{
			return null;
		}
		ViewLayoutGroup viewLayoutGroup = new ViewLayoutGroup();
		bool[] array = new bool[2];
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(viewLayoutGroup);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return viewLayoutGroup;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		int whileIterations = 0;
		int readerCount = base.ReaderCount;
		while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (!array[0] && (object)base.Reader.LocalName == id11_Id && (object)base.Reader.NamespaceURI == id2_Item)
				{
					viewLayoutGroup.Id = XmlConvert.ToGuid(base.Reader.ReadElementString());
					array[0] = true;
				}
				else if (!array[1] && (object)base.Reader.LocalName == id12_Name && (object)base.Reader.NamespaceURI == id2_Item)
				{
					viewLayoutGroup.Name = base.Reader.ReadElementString();
					array[1] = true;
				}
				else
				{
					UnknownNode(viewLayoutGroup, ":Id, :Name");
				}
			}
			else
			{
				UnknownNode(viewLayoutGroup, ":Id, :Name");
			}
			base.Reader.MoveToContent();
			CheckReaderCount(ref whileIterations, ref readerCount);
		}
		ReadEndElement();
		return viewLayoutGroup;
	}

	private ViewItem Read4_ViewItem(bool isNullable, bool checkType)
	{
		XmlQualifiedName xmlQualifiedName = (checkType ? GetXsiType() : null);
		bool flag = false;
		if (isNullable)
		{
			flag = ReadNull();
		}
		if (checkType && !(xmlQualifiedName == null) && ((object)xmlQualifiedName.Name != id8_ViewItem || (object)xmlQualifiedName.Namespace != id2_Item))
		{
			throw CreateUnknownTypeException(xmlQualifiedName);
		}
		if (flag)
		{
			return null;
		}
		ViewItem viewItem = new ViewItem();
		bool[] array = new bool[2];
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(viewItem);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return viewItem;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		int whileIterations = 0;
		int readerCount = base.ReaderCount;
		while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (!array[0] && (object)base.Reader.LocalName == id13_Position && (object)base.Reader.NamespaceURI == id2_Item)
				{
					viewItem.Position = Read2_Point(checkType: true);
					array[0] = true;
				}
				else if (!array[1] && (object)base.Reader.LocalName == id14_Size && (object)base.Reader.NamespaceURI == id2_Item)
				{
					viewItem.Size = Read3_Size(checkType: true);
					array[1] = true;
				}
				else
				{
					UnknownNode(viewItem, ":Position, :Size");
				}
			}
			else
			{
				UnknownNode(viewItem, ":Position, :Size");
			}
			base.Reader.MoveToContent();
			CheckReaderCount(ref whileIterations, ref readerCount);
		}
		ReadEndElement();
		return viewItem;
	}

	private Size Read3_Size(bool checkType)
	{
		XmlQualifiedName xmlQualifiedName = (checkType ? GetXsiType() : null);
		if (checkType && !(xmlQualifiedName == null) && ((object)xmlQualifiedName.Name != id14_Size || (object)xmlQualifiedName.Namespace != id2_Item))
		{
			throw CreateUnknownTypeException(xmlQualifiedName);
		}
		Size size;
		try
		{
			size = (Size)Activator.CreateInstance(typeof(Size), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], null);
		}
		catch (MissingMethodException)
		{
			throw CreateInaccessibleConstructorException("global::System.Drawing.Size");
		}
		catch (SecurityException)
		{
			throw CreateCtorHasSecurityException("global::System.Drawing.Size");
		}
		bool[] array = new bool[2];
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(size);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return size;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		int whileIterations = 0;
		int readerCount = base.ReaderCount;
		while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (!array[0] && (object)base.Reader.LocalName == id15_Width && (object)base.Reader.NamespaceURI == id2_Item)
				{
					size.Width = XmlConvert.ToInt32(base.Reader.ReadElementString());
					array[0] = true;
				}
				else if (!array[1] && (object)base.Reader.LocalName == id16_Height && (object)base.Reader.NamespaceURI == id2_Item)
				{
					size.Height = XmlConvert.ToInt32(base.Reader.ReadElementString());
					array[1] = true;
				}
				else
				{
					UnknownNode(size, ":Width, :Height");
				}
			}
			else
			{
				UnknownNode(size, ":Width, :Height");
			}
			base.Reader.MoveToContent();
			CheckReaderCount(ref whileIterations, ref readerCount);
		}
		ReadEndElement();
		return size;
	}

	private Point Read2_Point(bool checkType)
	{
		XmlQualifiedName xmlQualifiedName = (checkType ? GetXsiType() : null);
		if (checkType && !(xmlQualifiedName == null) && ((object)xmlQualifiedName.Name != id17_Point || (object)xmlQualifiedName.Namespace != id2_Item))
		{
			throw CreateUnknownTypeException(xmlQualifiedName);
		}
		Point point;
		try
		{
			point = (Point)Activator.CreateInstance(typeof(Point), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], null);
		}
		catch (MissingMethodException)
		{
			throw CreateInaccessibleConstructorException("global::System.Drawing.Point");
		}
		catch (SecurityException)
		{
			throw CreateCtorHasSecurityException("global::System.Drawing.Point");
		}
		bool[] array = new bool[2];
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(point);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return point;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		int whileIterations = 0;
		int readerCount = base.ReaderCount;
		while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (!array[0] && (object)base.Reader.LocalName == id18_X && (object)base.Reader.NamespaceURI == id2_Item)
				{
					point.X = XmlConvert.ToInt32(base.Reader.ReadElementString());
					array[0] = true;
				}
				else if (!array[1] && (object)base.Reader.LocalName == id19_Y && (object)base.Reader.NamespaceURI == id2_Item)
				{
					point.Y = XmlConvert.ToInt32(base.Reader.ReadElementString());
					array[1] = true;
				}
				else
				{
					UnknownNode(point, ":X, :Y");
				}
			}
			else
			{
				UnknownNode(point, ":X, :Y");
			}
			base.Reader.MoveToContent();
			CheckReaderCount(ref whileIterations, ref readerCount);
		}
		ReadEndElement();
		return point;
	}

	private ViewLayout Read5_ViewLayout(bool isNullable, bool checkType)
	{
		XmlQualifiedName xmlQualifiedName = (checkType ? GetXsiType() : null);
		bool flag = false;
		if (isNullable)
		{
			flag = ReadNull();
		}
		if (checkType && !(xmlQualifiedName == null) && ((object)xmlQualifiedName.Name != id7_ViewLayout || (object)xmlQualifiedName.Namespace != id2_Item))
		{
			throw CreateUnknownTypeException(xmlQualifiedName);
		}
		if (flag)
		{
			return null;
		}
		ViewLayout viewLayout = new ViewLayout();
		if (viewLayout.ViewItems == null)
		{
			viewLayout.ViewItems = new List<ViewItem>();
		}
		_ = viewLayout.ViewItems;
		bool[] array = new bool[4];
		while (base.Reader.MoveToNextAttribute())
		{
			if (!array[0] && (object)base.Reader.LocalName == id20_ViewLayoutType && (object)base.Reader.NamespaceURI == id2_Item)
			{
				viewLayout.ViewLayoutType = base.Reader.Value;
				array[0] = true;
			}
			else if (!array[1] && (object)base.Reader.LocalName == id11_Id && (object)base.Reader.NamespaceURI == id2_Item)
			{
				viewLayout.Id = XmlConvert.ToGuid(base.Reader.Value);
				array[1] = true;
			}
			else if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(viewLayout, ":ViewLayoutType, :Id");
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return viewLayout;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		int whileIterations = 0;
		int readerCount = base.ReaderCount;
		while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (!array[2] && (object)base.Reader.LocalName == id21_ViewLayoutIcon && (object)base.Reader.NamespaceURI == id2_Item)
				{
					viewLayout.BinaryIcon = ToByteArrayBase64(isNull: false);
					array[2] = true;
				}
				else if ((object)base.Reader.LocalName == id22_ViewItems && (object)base.Reader.NamespaceURI == id2_Item)
				{
					if (!ReadNull())
					{
						if (viewLayout.ViewItems == null)
						{
							viewLayout.ViewItems = new List<ViewItem>();
						}
						List<ViewItem> viewItems = viewLayout.ViewItems;
						if (base.Reader.IsEmptyElement)
						{
							base.Reader.Skip();
						}
						else
						{
							base.Reader.ReadStartElement();
							base.Reader.MoveToContent();
							int whileIterations2 = 0;
							int readerCount2 = base.ReaderCount;
							while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
							{
								if (base.Reader.NodeType == XmlNodeType.Element)
								{
									if ((object)base.Reader.LocalName == id8_ViewItem && (object)base.Reader.NamespaceURI == id2_Item)
									{
										if (viewItems == null)
										{
											base.Reader.Skip();
										}
										else
										{
											viewItems.Add(Read4_ViewItem(isNullable: true, checkType: true));
										}
									}
									else
									{
										UnknownNode(null, ":ViewItem");
									}
								}
								else
								{
									UnknownNode(null, ":ViewItem");
								}
								base.Reader.MoveToContent();
								CheckReaderCount(ref whileIterations2, ref readerCount2);
							}
							ReadEndElement();
						}
					}
				}
				else
				{
					UnknownNode(viewLayout, ":ViewLayoutIcon, :ViewItems");
				}
			}
			else
			{
				UnknownNode(viewLayout, ":ViewLayoutIcon, :ViewItems");
			}
			base.Reader.MoveToContent();
			CheckReaderCount(ref whileIterations, ref readerCount);
		}
		ReadEndElement();
		return viewLayout;
	}

	protected override void InitCallbacks()
	{
	}

	protected override void InitIDs()
	{
		id2_Item = base.Reader.NameTable.Add("");
		id1_Group = base.Reader.NameTable.Add("Group");
		id8_ViewItem = base.Reader.NameTable.Add("ViewItem");
		id5_ViewGroup = base.Reader.NameTable.Add("ViewGroup");
		id15_Width = base.Reader.NameTable.Add("Width");
		id9_ViewLayoutGroup = base.Reader.NameTable.Add("ViewLayoutGroup");
		id18_X = base.Reader.NameTable.Add("X");
		id12_Name = base.Reader.NameTable.Add("Name");
		id13_Position = base.Reader.NameTable.Add("Position");
		id3_PropertyBase = base.Reader.NameTable.Add("PropertyBase");
		id17_Point = base.Reader.NameTable.Add("Point");
		id19_Y = base.Reader.NameTable.Add("Y");
		id4_View = base.Reader.NameTable.Add("View");
		id10_ViewLayoutGroupConstants = base.Reader.NameTable.Add("ViewLayoutGroupConstants");
		id22_ViewItems = base.Reader.NameTable.Add("ViewItems");
		id16_Height = base.Reader.NameTable.Add("Height");
		id6_ViewItem2 = base.Reader.NameTable.Add("ViewItem2");
		id14_Size = base.Reader.NameTable.Add("Size");
		id11_Id = base.Reader.NameTable.Add("Id");
		id7_ViewLayout = base.Reader.NameTable.Add("ViewLayout");
		id20_ViewLayoutType = base.Reader.NameTable.Add("ViewLayoutType");
		id21_ViewLayoutIcon = base.Reader.NameTable.Add("ViewLayoutIcon");
	}
}

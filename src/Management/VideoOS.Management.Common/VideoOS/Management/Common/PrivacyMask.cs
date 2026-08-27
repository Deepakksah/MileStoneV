using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace VideoOS.Management.Common;

public class PrivacyMask
{
	public enum GridSize
	{
		Grid8X8 = 8,
		Grid16X16 = 0x10,
		Grid32X32 = 0x20,
		Grid64X64 = 0x40
	}

	public class Cell
	{
		public int X { get; }

		public int Y { get; }

		public int MethodId { get; }

		public string ListKey => GetListKey(X, Y);

		public Cell(int x, int y, int methodId)
		{
			X = x;
			Y = y;
			MethodId = methodId;
		}

		public static string GetListKey(int x, int y)
		{
			return x.ToString(CultureInfo.InvariantCulture) + ":" + y.ToString(CultureInfo.InvariantCulture);
		}
	}

	public class CellCollection : IEnumerable<Cell>, IEnumerable
	{
		private Dictionary<string, Cell> _cells = new Dictionary<string, Cell>();

		public GridSize Size { get; private set; }

		public int NumberOfXCells => (int)Size;

		public int NumberOfYCells => (int)Size;

		private PrivacyMask Owner { get; }

		private static Lazy<string> AvailableEnumsMessage { get; } = new Lazy<string>(CreateSupportedGridSizeIntValuesMessage);

		public int Count => _cells.Count;

		public Cell this[string cellKey] => _cells[cellKey];

		public event EventHandler<EventArgs> Changed;

		public CellCollection(GridSize size, PrivacyMask owner)
		{
			Owner = owner;
			Initialize(size);
		}

		public void Initialize(GridSize size)
		{
			Size = size;
			_cells.Clear();
			OnChanged();
		}

		private void OnChanged()
		{
			this.Changed?.Invoke(this, new EventArgs());
		}

		private static string CreateSupportedGridSizeIntValuesMessage()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Array values = Enum.GetValues(typeof(GridSize));
			for (int i = 0; i < values.Length; i++)
			{
				if (i > 0 && i == values.Length - 1)
				{
					stringBuilder.Append(" and ");
				}
				else if (i > 0)
				{
					stringBuilder.Append(" ,");
				}
				stringBuilder.Append((int)values.GetValue(i));
			}
			return stringBuilder.ToString();
		}

		private static GridSize GridSizeFromInt(int value)
		{
			return value switch
			{
				8 => GridSize.Grid8X8, 
				16 => GridSize.Grid16X16, 
				32 => GridSize.Grid32X32, 
				64 => GridSize.Grid64X64, 
				_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unsupported size : {0}. Accepted values are {1}", value, AvailableEnumsMessage.Value)), 
			};
		}

		public void Initialize(int numberOfXCells, int numberOfYCells)
		{
			if (numberOfXCells != numberOfYCells)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Aspect ratios are not supported. Possible size values are {0}", AvailableEnumsMessage.Value));
			}
			GridSize size = GridSizeFromInt(numberOfXCells);
			Initialize(size);
		}

		private void ValidateCoordinates(int x, int y)
		{
			if (x < 0 || x >= NumberOfXCells)
			{
				throw new ArgumentOutOfRangeException("x", string.Format(CultureInfo.InvariantCulture, "Invalid privacy mask cell coordinates. X coordinate must be non-negative and less than '{0}'.", "NumberOfXCells"));
			}
			if (y < 0 || y >= NumberOfYCells)
			{
				throw new ArgumentOutOfRangeException("y", string.Format(CultureInfo.InvariantCulture, "Invalid privacy mask cell coordinates. Y coordinates must be non-negative and less than '{0}'.", "NumberOfYCells"));
			}
		}

		public void Add(int x, int y, int methodId)
		{
			ValidateCoordinates(x, y);
			Owner.GetMaskingMethod(methodId);
			_cells.Add(Cell.GetListKey(x, y), new Cell(x, y, methodId));
			OnChanged();
		}

		public void AddRange(IEnumerable<Point> cellCoordinates, int methodId)
		{
			Owner.GetMaskingMethod(methodId);
			foreach (Point cellCoordinate in cellCoordinates)
			{
				ValidateCoordinates(cellCoordinate.X, cellCoordinate.Y);
			}
			foreach (Point cellCoordinate2 in cellCoordinates)
			{
				_cells.Add(Cell.GetListKey(cellCoordinate2.X, cellCoordinate2.Y), new Cell(cellCoordinate2.X, cellCoordinate2.Y, methodId));
			}
			OnChanged();
		}

		public void Remove(int x, int y, int methodId)
		{
			string listKey = Cell.GetListKey(x, y);
			if (_cells.TryGetValue(listKey, out var value) && value.MethodId == methodId)
			{
				_cells.Remove(Cell.GetListKey(x, y));
				OnChanged();
			}
		}

		public void Clear()
		{
			_cells.Clear();
			OnChanged();
		}

		public bool Contains(int x, int y)
		{
			return _cells.ContainsKey(Cell.GetListKey(x, y));
		}

		public IEnumerator<Cell> GetEnumerator()
		{
			return _cells.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _cells.Values.GetEnumerator();
		}
	}

	private Dictionary<int, MaskingMethod> maskingMethods = new Dictionary<int, MaskingMethod>(10);

	private bool _loading;

	public static Color SolidColor { get; set; } = Color.Black;

	public GridSize Size => CellList.Size;

	public int NumberOfXCells => CellList.NumberOfXCells;

	public int NumberOfYCells => CellList.NumberOfYCells;

	public IEnumerable<MaskingMethod> Methods => maskingMethods.Values;

	public IEnumerable<Cell> Cells => CellList;

	public CellCollection CellList{ get; private set; }

	private static Lazy<XmlSchemaSet> PrivacyMaskSchemaSet { get; } = new Lazy<XmlSchemaSet>(LoadPrivacyMaskSchemaSet);

	public event EventHandler<EventArgs> Changed;

	public PrivacyMask(GridSize size)
	{
		CellList = new CellCollection(size, this);
		CellList.Changed += CellsChanged;
	}

	public PrivacyMask()
		: this(GridSize.Grid8X8)
	{
	}

	public void Initialize(GridSize gridSize)
	{
		CellList.Clear();
		maskingMethods.Clear();
		CellList.Initialize(gridSize);
		OnChanged();
	}

	public void Clear()
	{
		Initialize(GridSize.Grid8X8);
	}

	public SolidMaskingMethod AddSolidMethod(bool includeInMotionDetection, bool removable)
	{
		SolidMaskingMethod solidMaskingMethod = new SolidMaskingMethod(this, FindFreeMethodId(), includeInMotionDetection, removable);
		AddMethod(solidMaskingMethod);
		return solidMaskingMethod;
	}

	public BlurMaskingMethod AddBlurMethod(bool includeInMotionDetection, bool removable, int level)
	{
		BlurMaskingMethod blurMaskingMethod = new BlurMaskingMethod(this, FindFreeMethodId(), includeInMotionDetection, removable, level);
		AddMethod(blurMaskingMethod);
		return blurMaskingMethod;
	}

	private byte FindFreeMethodId()
	{
		byte b = 0;
		while (maskingMethods.ContainsKey(b))
		{
			b++;
		}
		return b;
	}

	private void AddMethod(MaskingMethod method)
	{
		if (maskingMethods.ContainsKey(method.Id))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Duplicate masking method id not allowed: {0}", method.Id), "method");
		}
		if (method.Id < 0 || method.Id > 255)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Masking method id allowed (0-255): {0}", method.Id), "method");
		}
		maskingMethods.Add(method.Id, method);
		OnChanged();
	}

	private MaskingMethod GetMaskingMethod(int methodId)
	{
		if (maskingMethods.TryGetValue(methodId, out var value))
		{
			return value;
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown masking method id : {0}", methodId), "methodId");
	}

	private void OnChanged()
	{
		if (!_loading)
		{
			this.Changed?.Invoke(this, EventArgs.Empty);
		}
	}

	private void CellsChanged(object sender, EventArgs e)
	{
		OnChanged();
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		using (StringWriter output = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
		{
			using XmlWriter xmlWriter = XmlWriter.Create(output, xmlWriterSettings);
			xmlWriter.WriteStartElement("mask");
			WriteMethodsAsXml(xmlWriter, maskingMethods.Values);
			WriteGridAsXml(xmlWriter, CellList);
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}
		return stringBuilder.ToString();
	}

	private static void WriteMethodsAsXml(XmlWriter wr, IEnumerable<MaskingMethod> privacyMaskMethods)
	{
		wr.WriteStartElement("methods");
		foreach (MaskingMethod item in privacyMaskMethods.OrderBy((MaskingMethod m) => m.Id))
		{
			switch (item.MaskType)
			{
			case MaskType.Solid:
				WriteSolidMethodAsXml(wr, (SolidMaskingMethod)item);
				break;
			case MaskType.Blurring:
				WriteBlurringMethodAsXml(wr, (BlurMaskingMethod)item);
				break;
			}
		}
		wr.WriteEndElement();
	}

	private static void WriteSolidMethodAsXml(XmlWriter wr, SolidMaskingMethod method)
	{
		wr.WriteStartElement("solid");
		wr.WriteAttributeString("id", method.Id.ToString(CultureInfo.InvariantCulture));
		wr.WriteAttributeString("motionDetection", method.IncludedInMotionDetection ? "true" : "false");
		wr.WriteAttributeString("removable", method.Removable ? "true" : "false");
		wr.WriteEndElement();
	}

	private static void WriteBlurringMethodAsXml(XmlWriter wr, BlurMaskingMethod method)
	{
		wr.WriteStartElement("blur");
		wr.WriteAttributeString("id", method.Id.ToString(CultureInfo.InvariantCulture));
		wr.WriteAttributeString("motionDetection", method.IncludedInMotionDetection ? "true" : "false");
		wr.WriteAttributeString("removable", method.Removable ? "true" : "false");
		wr.WriteAttributeString("level", method.Level.ToString(CultureInfo.InvariantCulture));
		wr.WriteEndElement();
	}

	private static void WriteGridAsXml(XmlWriter wr, CellCollection cells)
	{
		wr.WriteStartElement("grid");
		wr.WriteAttributeString("size", GridSizeToString(cells.Size));
		wr.WriteStartElement("cells");
		foreach (IGrouping<int, Cell> item in from c in cells
			group c by c.Y into g
			orderby g.Key
			select g)
		{
			foreach (Cell item2 in item.OrderBy((Cell g) => g.X))
			{
				wr.WriteStartElement("cell");
				wr.WriteAttributeString("x", item2.X.ToString(CultureInfo.InvariantCulture));
				wr.WriteAttributeString("y", item2.Y.ToString(CultureInfo.InvariantCulture));
				wr.WriteAttributeString("methodId", item2.MethodId.ToString(CultureInfo.InvariantCulture));
				wr.WriteEndElement();
			}
		}
		wr.WriteEndElement();
	}

	private static string GridSizeToString(GridSize size)
	{
		return size switch
		{
			GridSize.Grid8X8 => "8x8", 
			GridSize.Grid16X16 => "16x16", 
			GridSize.Grid32X32 => "32x32", 
			GridSize.Grid64X64 => "64x64", 
			_ => throw new ArgumentOutOfRangeException("size", size, null), 
		};
	}

	private static XmlSchemaSet LoadPrivacyMaskSchemaSet()
	{
		XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
		using (XmlReader schemaDocument = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("VideoOS.Management.Common.VmoObjectXmlParsers.Schemas.PrivacyMask.xsd")))
		{
			xmlSchemaSet.Add(null, schemaDocument);
		}
		xmlSchemaSet.Compile();
		return xmlSchemaSet;
	}

	public static PrivacyMask FromXml(string xml)
	{
		PrivacyMask privacyMask = new PrivacyMask();
		privacyMask.LoadXml(xml);
		return privacyMask;
	}

	public void LoadXml(string xml)
	{
		_loading = true;
		try
		{
			Clear();
			if (string.IsNullOrEmpty(xml))
			{
				return;
			}
			using StringReader input = new StringReader(xml);
			XmlReaderSettings settings = new XmlReaderSettings
			{
				IgnoreComments = true,
				IgnoreProcessingInstructions = true,
				IgnoreWhitespace = true,
				XmlResolver = null,
				ValidationType = ValidationType.Schema,
				Schemas = PrivacyMaskSchemaSet.Value
			};
			using XmlReader xmlReader = XmlReader.Create(input, settings);
			xmlReader.MoveToContent();
			xmlReader.Read();
			ReadMethodsTableFromXml(xmlReader, this);
			xmlReader.Read();
			ReadGridElementFromXml(xmlReader, CellList);
		}
		finally
		{
			_loading = false;
		}
	}

	private static void ReadMethodsTableFromXml(XmlReader reader, PrivacyMask mask)
	{
		if (reader.IsEmptyElement)
		{
			return;
		}
		while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !(reader.Name == "methods")))
		{
			if (reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			string name = reader.Name;
			if (!(name == "solid"))
			{
				if (name == "blur")
				{
					int methodId = 0;
					bool includeInMotionDetection = false;
					bool removable = false;
					int level = 0;
					reader.MoveToFirstAttribute();
					do
					{
						switch (reader.Name)
						{
						case "id":
							methodId = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
							break;
						case "motionDetection":
							includeInMotionDetection = XmlConvert.ToBoolean(reader.Value);
							break;
						case "removable":
							removable = XmlConvert.ToBoolean(reader.Value);
							break;
						case "level":
							level = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
							break;
						}
					}
					while (reader.MoveToNextAttribute());
					mask.AddMethod(new BlurMaskingMethod(mask, methodId, includeInMotionDetection, removable, level));
					continue;
				}
				throw new ArgumentException("Unrecognized method : " + reader.Name);
			}
			int methodId2 = 0;
			bool includeInMotionDetection2 = false;
			bool removable2 = false;
			reader.MoveToFirstAttribute();
			do
			{
				switch (reader.Name)
				{
				case "id":
					methodId2 = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
					break;
				case "motionDetection":
					includeInMotionDetection2 = XmlConvert.ToBoolean(reader.Value);
					break;
				case "removable":
					removable2 = XmlConvert.ToBoolean(reader.Value);
					break;
				}
			}
			while (reader.MoveToNextAttribute());
			mask.AddMethod(new SolidMaskingMethod(mask, methodId2, includeInMotionDetection2, removable2));
		}
	}

	private static void ReadGridElementFromXml(XmlReader reader, CellCollection cells)
	{
		reader.MoveToFirstAttribute();
		cells.Initialize(reader.Value switch
		{
			"8x8" => GridSize.Grid8X8, 
			"16x16" => GridSize.Grid16X16, 
			"32x32" => GridSize.Grid32X32, 
			"64x64" => GridSize.Grid64X64, 
			_ => throw new ArgumentException("Unsupported grid size : " + reader.Value), 
		});
		reader.MoveToElement();
		reader.Read();
		if (reader.IsEmptyElement)
		{
			return;
		}
		while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !(reader.Name == "cells")))
		{
			if (!string.Equals(reader.Name, "cell", StringComparison.Ordinal))
			{
				continue;
			}
			int x = 0;
			int y = 0;
			int methodId = 0;
			reader.MoveToFirstAttribute();
			do
			{
				switch (reader.Name)
				{
				case "x":
					x = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
					break;
				case "y":
					y = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
					break;
				case "methodId":
					methodId = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
					break;
				}
			}
			while (reader.MoveToNextAttribute());
			cells.Add(x, y, methodId);
		}
	}
}

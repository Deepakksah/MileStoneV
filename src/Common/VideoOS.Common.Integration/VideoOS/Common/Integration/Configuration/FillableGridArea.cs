using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace VideoOS.Common.Integration.Configuration;

public class FillableGridArea
{
	private const string CONFIGSECTION = "config";

	private const string XREGIONS = "xregions";

	private const string YREGIONS = "yregions";

	private const string REGIONPOINTS = "regionpoints";

	private const string POINT = "point";

	private const string XVALUE = "x";

	private const string YVALUE = "y";

	private byte[] _regions { get; set; }

	public int RegionsXAxis { get; set; }

	public int RegionsYAxis { get; set; }

	public int NumberOfRegions => _regions.Length;

	public bool HasFilledCell => _regions.Any((byte cell) => cell == 1);

	public FillableGridArea(int regionsXAxis, int regionsYAxis)
	{
		InitializeGridArea(regionsXAxis, regionsYAxis);
	}

	public FillableGridArea(FillableGridArea gridArea)
	{
		if (gridArea == null)
		{
			throw new ArgumentNullException("gridArea");
		}
		InitializeGridArea(gridArea.RegionsXAxis, gridArea.RegionsYAxis);
		for (int i = 0; i < RegionsXAxis; i++)
		{
			for (int j = 0; j < RegionsYAxis; j++)
			{
				if (gridArea.IsRegionFilled(i, j))
				{
					SetRegion(i, j, filled: true);
				}
			}
		}
	}

	public void InitializeGridArea(int regionsXAxis, int regionsYAxis)
	{
		RegionsXAxis = regionsXAxis;
		RegionsYAxis = regionsYAxis;
		_regions = new byte[RegionsXAxis * RegionsYAxis];
	}

	public void ClearGridArea()
	{
		InitializeGridArea(RegionsXAxis, RegionsYAxis);
	}

	public void SetRegion(int x, int y, bool filled)
	{
		_regions[y * RegionsXAxis + x] = (filled ? ((byte)1) : ((byte)0));
	}

	public bool SetRegion(int xMiddle, int yMiddle, int radius, bool filled)
	{
		bool result = false;
		if (radius <= 0)
		{
			throw new ArgumentException("Radius parameter must be greater than zero.");
		}
		int num = xMiddle - (radius - 1);
		int num2 = xMiddle + (radius - 1);
		int num3 = yMiddle - (radius - 1);
		int num4 = yMiddle + (radius - 1);
		for (int i = num; i < num2 + 1; i++)
		{
			for (int j = num3; j < num4 + 1; j++)
			{
				if (i >= 0 && i < RegionsXAxis && j >= 0 && j < RegionsYAxis && IsRegionFilled(i, j) != filled)
				{
					result = true;
					SetRegion(i, j, filled);
				}
			}
		}
		return result;
	}

	public bool IsRegionFilled(int x, int y)
	{
		return _regions[y * RegionsXAxis + x] != 0;
	}

	public void IncreaseNumberOfRegions(int increaseFactor)
	{
		if (increaseFactor <= 0)
		{
			throw new ArgumentOutOfRangeException("increaseFactor", "Increase factor must be non-negative in FillableGridArea.IncreaseNumberOfRegions().");
		}
		FillableGridArea fillableGridArea = new FillableGridArea(this);
		InitializeGridArea(RegionsXAxis * increaseFactor, RegionsYAxis * increaseFactor);
		for (int i = 0; i < fillableGridArea.RegionsXAxis; i++)
		{
			for (int j = 0; j < fillableGridArea.RegionsYAxis; j++)
			{
				if (!fillableGridArea.IsRegionFilled(i, j))
				{
					continue;
				}
				for (int k = 0; k < increaseFactor; k++)
				{
					for (int l = 0; l < increaseFactor; l++)
					{
						SetRegion(i * increaseFactor + k, j * increaseFactor + l, filled: true);
					}
				}
			}
		}
	}

	public static FillableGridArea MergeGridAreas(FillableGridArea area1, FillableGridArea area2)
	{
		if (area1 == null && area2 == null)
		{
			throw new ArgumentException("Both fillable grid area parameters cannot be null when merging.", "area2");
		}
		if (area1 == null)
		{
			return new FillableGridArea(area2);
		}
		if (area2 == null)
		{
			return new FillableGridArea(area1);
		}
		if (area1.NumberOfRegions == 0)
		{
			return new FillableGridArea(area2);
		}
		if (area2.NumberOfRegions == 0)
		{
			return new FillableGridArea(area1);
		}
		FillableGridArea fillableGridArea = null;
		FillableGridArea fillableGridArea2 = null;
		if ((float)area1.RegionsXAxis / (float)area1.RegionsYAxis != (float)area2.RegionsXAxis / (float)area2.RegionsYAxis)
		{
			throw new ArgumentException("The two fillable grid areas must have the same aspect ratio to be merged.", "area2");
		}
		if (area1.RegionsXAxis > area2.RegionsXAxis)
		{
			if (area1.RegionsXAxis % area2.RegionsXAxis > 0)
			{
				throw new ArgumentException("The size difference (X/Y regions divided) of the privacy mask and motion exclude regions must be an integer (no decimals) when merging them.", "area2");
			}
			fillableGridArea = new FillableGridArea(area2);
			fillableGridArea.IncreaseNumberOfRegions(area1.RegionsXAxis / area2.RegionsXAxis);
			fillableGridArea2 = area1;
		}
		else if (area2.RegionsXAxis > area1.RegionsXAxis)
		{
			if (area2.RegionsXAxis % area1.RegionsXAxis > 0)
			{
				throw new ArgumentException("The size difference (X/Y regions divided) of the privacy mask and motion exclude regions must be an integer (no decimals) when merging them.", "area2");
			}
			fillableGridArea = new FillableGridArea(area1);
			fillableGridArea.IncreaseNumberOfRegions(area2.RegionsXAxis / area1.RegionsXAxis);
			fillableGridArea2 = area2;
		}
		else
		{
			fillableGridArea = new FillableGridArea(area1);
			fillableGridArea2 = area2;
		}
		for (int i = 0; i < fillableGridArea2.RegionsXAxis; i++)
		{
			for (int j = 0; j < fillableGridArea2.RegionsYAxis; j++)
			{
				if (fillableGridArea2.IsRegionFilled(i, j))
				{
					fillableGridArea.SetRegion(i, j, filled: true);
				}
			}
		}
		return fillableGridArea;
	}

	public byte[] GetRegionsAsByteArray()
	{
		return _regions;
	}

	public byte[] GetRegionsAsByteArrayCopy()
	{
		byte[] array = new byte[_regions.Length];
		Array.Copy(_regions, array, _regions.Length);
		return array;
	}

	public bool[] GetRegionsAsBooleanArray(bool invert)
	{
		bool[] array = new bool[_regions.Length];
		for (int i = 0; i < _regions.Length; i++)
		{
			array[i] = ((_regions[i] == 0) ? invert : (!invert));
		}
		return array;
	}

	public void LoadGridAreaFromXml(string xml)
	{
		if (string.IsNullOrEmpty(xml))
		{
			return;
		}
		using XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml));
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element)
			{
				switch (xmlTextReader.Name)
				{
				case "xregions":
					RegionsXAxis = Convert.ToInt32(xmlTextReader.ReadString(), CultureInfo.InvariantCulture);
					break;
				case "yregions":
					RegionsYAxis = Convert.ToInt32(xmlTextReader.ReadString(), CultureInfo.InvariantCulture);
					break;
				case "regionpoints":
					InitializeGridArea(RegionsXAxis, RegionsYAxis);
					break;
				case "point":
				{
					int x = Convert.ToInt32(xmlTextReader.GetAttribute("x"), CultureInfo.InvariantCulture);
					int y = Convert.ToInt32(xmlTextReader.GetAttribute("y"), CultureInfo.InvariantCulture);
					SetRegion(x, y, filled: true);
					break;
				}
				}
			}
		}
	}

	public string GetGridAreaAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteElementString("xregions", RegionsXAxis.ToString(CultureInfo.InvariantCulture));
		xmlTextWriter.WriteElementString("yregions", RegionsYAxis.ToString(CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("regionpoints");
		for (int i = 0; i < RegionsXAxis; i++)
		{
			for (int j = 0; j < RegionsYAxis; j++)
			{
				if (IsRegionFilled(i, j))
				{
					xmlTextWriter.WriteStartElement("point");
					xmlTextWriter.WriteAttributeString("x", Convert.ToString(i, CultureInfo.InvariantCulture));
					xmlTextWriter.WriteAttributeString("y", Convert.ToString(j, CultureInfo.InvariantCulture));
					xmlTextWriter.WriteEndElement();
				}
			}
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		return stringBuilder.ToString();
	}
}

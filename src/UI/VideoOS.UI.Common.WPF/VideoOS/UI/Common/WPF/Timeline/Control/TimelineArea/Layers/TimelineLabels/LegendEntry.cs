using System;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.TimelineLabels;

public class LegendEntry : IEquatable<LegendEntry>
{
	private readonly string _name;

	private readonly Brush _brush;

	private readonly ImageSource _imageSource;

	public string Name => _name;

	public Brush Brush => _brush;

	public ImageSource ImageSource => _imageSource;

	public LegendEntry(string name, Brush brush, ImageSource imageSource)
	{
		_name = name;
		_brush = brush;
		_imageSource = imageSource;
	}

	public override int GetHashCode()
	{
		return Name.GetHashCode();
	}

	public bool Equals(LegendEntry other)
	{
		return Name == other.Name;
	}
}

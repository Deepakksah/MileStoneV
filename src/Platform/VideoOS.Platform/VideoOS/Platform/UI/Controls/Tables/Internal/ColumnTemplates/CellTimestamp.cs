using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public partial class CellTimestamp : ContentControl, IComponentConnector
{
	public static readonly DependencyProperty TimestampProperty = DependencyProperty.Register("Timestamp", typeof(DateTime), typeof(CellTimestamp), new PropertyMetadata(null));

	public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(CellTimestamp), new PropertyMetadata(string.Empty));

	public DateTime Timestamp
	{
		get
		{
			return (DateTime)GetValue(TimestampProperty);
		}
		set
		{
			SetValue(TimestampProperty, value);
		}
	}

	public string Format
	{
		get
		{
			return (string)GetValue(FormatProperty);
		}
		set
		{
			SetValue(FormatProperty, value);
		}
	}

	public CellTimestamp()
	{
		InitializeComponent();
	}
}

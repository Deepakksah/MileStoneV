using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class LabelReplacementExtension : MarkupExtension, IValueConverter
{
	public LabelReplacementCollection ReplacementList { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && ReplacementList != null)
		{
			if (value is IEnumerable<LabeledValue> original)
			{
				return GetReplacedEnumerable(original, ReplacementList);
			}
			string text = value.ToString();
			foreach (LabelReplacement replacement in ReplacementList)
			{
				if (text == replacement.OriginalLabel)
				{
					return replacement.ReplacedLabel;
				}
			}
		}
		return value;
	}

	private IEnumerable<LabeledValue> GetReplacedEnumerable(IEnumerable<LabeledValue> original, LabelReplacementCollection replacementList)
	{
		foreach (LabeledValue item in original)
		{
			LabeledValue labeledValue = new LabeledValue(item.Label, item.Value);
			foreach (LabelReplacement replacement in replacementList)
			{
				if (labeledValue.Label == replacement.OriginalLabel)
				{
					labeledValue.Label = replacement.ReplacedLabel;
					break;
				}
			}
			yield return labeledValue;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Binding.DoNothing;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}

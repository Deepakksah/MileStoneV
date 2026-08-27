using System;
using System.Linq;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;

namespace VideoOS.Platform.Search;

public abstract class SearchFilter
{
	public Guid Id { get; set; } = Guid.Empty;

	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public bool MultipleValuesAllowed { get; set; }

	[Obsolete("Obsolete member. Please set the DisplayMode-property in the return value of the GetFilterConfiguration().")]
	public bool UseInlineEdit { get; set; }

	public bool UsesScopeItems { get; set; }

	public abstract FilterConfigurationBase GetFilterConfiguration();

	public abstract FilterValueBase CreateValue();

	public abstract void ResetValue(FilterValueBase value);

	public virtual bool IsEmptyValue(FilterValueBase value)
	{
		if (value is StringFilterValue)
		{
			return string.IsNullOrWhiteSpace(((StringFilterValue)value).Text);
		}
		if (value is DoubleRangeFilterValue)
		{
			DoubleRangeFilterValue doubleRangeFilterValue = (DoubleRangeFilterValue)value;
			if (GetFilterConfiguration() is RangeSliderFilterConfiguration rangeSliderFilterConfiguration)
			{
				if (Math.Abs(doubleRangeFilterValue.Low - rangeSliderFilterConfiguration.Minimum) < 0.0001)
				{
					return Math.Abs(doubleRangeFilterValue.High - rangeSliderFilterConfiguration.Maximum) < 0.0001;
				}
				return false;
			}
		}
		if (value is SelectionFilterValue)
		{
			return !((SelectionFilterValue)value).SelectedIds.Any();
		}
		return false;
	}

	public virtual string GetDisplayValue(FilterValueBase value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		return value.FallbackDisplayValue();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using VideoOS.Platform.Resources;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;

namespace VideoOS.Platform.Search.SearchFilters;

public sealed class ColorSearchFilter : SearchFilter
{
	private readonly IDictionary<Guid, (string DisplayName, Color Value)> _items;

	private ListSelectionFilterConfiguration _configuration;

	private IComparer<(string DisplayName, Color Value)> _colorDisplayNameComparer = new ColorDisplayNameComparer();

	public ColorSearchFilter(Guid id, string name, IDictionary<Guid, (string DisplayName, Color Value)> items)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("Id cannot be empty GUID", "id");
		}
		base.Id = id;
		base.Name = name ?? throw new ArgumentNullException("name");
		_items = items ?? throw new ArgumentNullException("items");
	}

	public override FilterValueBase CreateValue()
	{
		return new SelectionFilterValue();
	}

	public override FilterConfigurationBase GetFilterConfiguration()
	{
		if (_configuration == null)
		{
			_configuration = new ListSelectionFilterConfiguration();
			foreach (KeyValuePair<Guid, (string, Color)> item in _items.OrderBy((KeyValuePair<Guid, (string DisplayName, Color Value)> x) => x.Value, _colorDisplayNameComparer))
			{
				_configuration.Items[item.Key] = item.Value.Item1;
			}
		}
		return _configuration;
	}

	public override void ResetValue(FilterValueBase value)
	{
		if (value is SelectionFilterValue selectionFilterValue)
		{
			selectionFilterValue.SetSelectedIds(Enumerable.Empty<Guid>());
			return;
		}
		throw new ArgumentException($"Invalid filter value '{value}' received.");
	}

	public override string GetDisplayValue(FilterValueBase value)
	{
		if (!(value is SelectionFilterValue selectionFilterValue))
		{
			throw new ArgumentException("Does not match expected type: " + typeof(SelectionFilterValue).Name, "value");
		}
		if (selectionFilterValue.SelectedIds.Count == 0)
		{
			return Strings.ColorSearchFilter_ColorNotSelected;
		}
		return string.Join(", ", selectionFilterValue.SelectedIds.Select((Guid id) => _items[id].DisplayName));
	}

	public Color GetItemValue(Guid id)
	{
		if (!_items.TryGetValue(id, out (string, Color) value))
		{
			throw new ArgumentException($"Invalid item id '{id}' received.");
		}
		return value.Item2;
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.Search.FilterValues;

public class SelectionFilterValue : FilterValueBase
{
	private List<Guid> _selectedIds = new List<Guid>();

	private const char _separator = '\u001b';

	public IReadOnlyCollection<Guid> SelectedIds { get; }

	public SelectionFilterValue()
	{
		SelectedIds = new ReadOnlyCollection<Guid>(_selectedIds);
	}

	public override string FallbackDisplayValue()
	{
		return string.Format(CultureInfo.CurrentCulture, Strings.SelectionFilterValueFallbackDisplayValue, SelectedIds.Count);
	}

	public void SetSelectedIds(IEnumerable<Guid> selectedIds)
	{
		List<Guid> list = ((selectedIds != null) ? selectedIds.ToList() : Enumerable.Empty<Guid>().ToList());
		if (_selectedIds.Except(list).Any() || list.Except(_selectedIds).Any())
		{
			_selectedIds.Clear();
			_selectedIds.AddRange(list);
			FireChangedEvent();
		}
	}

	public override string Serialize()
	{
		return string.Join('\u001b'.ToString(), SelectedIds.Select((Guid i) => i.ToString()).ToList());
	}

	public override bool Deserialize(string data)
	{
		if (string.IsNullOrWhiteSpace(data))
		{
			return false;
		}
		string[] array = data.Split(new char[1] { '\u001b' }, StringSplitOptions.None);
		List<Guid> list = new List<Guid>(array.Length);
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (Guid.TryParse(array2[i], out var result))
			{
				list.Add(result);
				continue;
			}
			return false;
		}
		SetSelectedIds(list);
		return true;
	}
}

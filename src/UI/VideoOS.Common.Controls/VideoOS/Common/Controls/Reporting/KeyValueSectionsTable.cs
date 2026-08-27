using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace VideoOS.Common.Controls.Reporting;

public class KeyValueSectionsTable : ReportTableBase
{
	private List<KeyValuePair<string, List<KeyValuePair<string, string>>>> _sections = new List<KeyValuePair<string, List<KeyValuePair<string, string>>>>();

	public int SectionCount { get; private set; }

	public override DataTable DataTable
	{
		get
		{
			DataTable dataTable = base.DataTable;
			foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> section in _sections)
			{
				dataTable.Rows.Add(section.Key, "", "");
				foreach (KeyValuePair<string, string> item in section.Value)
				{
					dataTable.Rows.Add("", item.Key, item.Value);
				}
			}
			return dataTable;
		}
	}

	public KeyValueSectionsTable()
	{
		base.ColumnCount = 3;
	}

	public void AddSection(string name)
	{
		_sections.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>(name, new List<KeyValuePair<string, string>>()));
		SectionCount++;
	}

	public void AddRow(string key, string value)
	{
		if (_sections.Count == 0)
		{
			throw new InvalidOperationException("No section to add key-value to.");
		}
		_sections[_sections.Count - 1].Value.Add(new KeyValuePair<string, string>(key, value));
		base.RowCount++;
	}

	public string GetValue(string section, string rowKey)
	{
		return (from _003C_003Eh__TransparentIdentifier0 in _sections.Where(delegate(KeyValuePair<string, List<KeyValuePair<string, string>>> localSection)
			{
				string text = section;
				KeyValuePair<string, List<KeyValuePair<string, string>>> keyValuePair = localSection;
				return text == keyValuePair.Key;
			}).SelectMany(delegate(KeyValuePair<string, List<KeyValuePair<string, string>>> localSection)
			{
				KeyValuePair<string, List<KeyValuePair<string, string>>> keyValuePair = localSection;
				return keyValuePair.Value;
			}, (KeyValuePair<string, List<KeyValuePair<string, string>>> localSection, KeyValuePair<string, string> row) => new { localSection, row })
			where rowKey == _003C_003Eh__TransparentIdentifier0.row.Key
			select _003C_003Eh__TransparentIdentifier0.row.Value).FirstOrDefault();
	}
}

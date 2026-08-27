using System;
using System.Collections.Generic;
using System.Data;

namespace VideoOS.Common.Controls.Reporting;

public class SectionsTable : ReportTableBase
{
	private List<KeyValuePair<string, List<string[]>>> _sections = new List<KeyValuePair<string, List<string[]>>>();

	public int SectionCount { get; private set; }

	public override DataTable DataTable
	{
		get
		{
			DataTable dataTable = base.DataTable;
			foreach (KeyValuePair<string, List<string[]>> section in _sections)
			{
				string[] array = new string[base.ColumnCount];
				array[0] = section.Key;
				DataRowCollection rows = dataTable.Rows;
				object[] values = array;
				rows.Add(values);
				foreach (string[] item in section.Value)
				{
					string[] array2 = new string[base.ColumnCount];
					item.CopyTo(array2, 1);
					DataRowCollection rows2 = dataTable.Rows;
					values = array2;
					rows2.Add(values);
				}
			}
			return dataTable;
		}
	}

	public void AddSection(string name)
	{
		_sections.Add(new KeyValuePair<string, List<string[]>>(name, new List<string[]>()));
		SectionCount++;
	}

	public void AddRow(params string[] row)
	{
		if (_sections.Count == 0)
		{
			throw new InvalidOperationException("No section to add key-value to.");
		}
		if (row == null)
		{
			throw new ArgumentNullException("row");
		}
		if (base.ColumnCount == 0)
		{
			base.ColumnCount = row.Length + 1;
		}
		else if (row.Length + 1 != base.ColumnCount)
		{
			throw new ArgumentException("Invalid number of parameters.");
		}
		_sections[_sections.Count - 1].Value.Add(row);
		base.RowCount++;
	}
}

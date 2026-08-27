using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace VideoOS.Common.Controls.Reporting;

public class KeyValueImageTable : ReportTableBase
{
	private List<KeyValuePair<string, string>> _rows = new List<KeyValuePair<string, string>>();

	public override DataTable DataTable
	{
		get
		{
			DataTable dataTable = base.DataTable;
			foreach (KeyValuePair<string, string> row in _rows)
			{
				dataTable.Rows.Add(row.Key, row.Value);
			}
			while (dataTable.Rows.Count < MinRowCount)
			{
				dataTable.Rows.Add();
			}
			return dataTable;
		}
	}

	public Image Image { get; set; }

	public int MinRowCount { get; set; }

	public KeyValueImageTable()
	{
		base.ColumnCount = 3;
	}

	public void AddRow(string key, string value)
	{
		_rows.Add(new KeyValuePair<string, string>(key, value));
		base.RowCount++;
	}
}

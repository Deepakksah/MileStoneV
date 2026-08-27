using System.Collections.Generic;
using System.Data;

namespace VideoOS.Common.Controls.Reporting;

public class KeyValueTable : ReportTableBase
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
			return dataTable;
		}
	}

	public KeyValueTable()
	{
		base.ColumnCount = 2;
	}

	public void AddRow(string key, string value)
	{
		_rows.Add(new KeyValuePair<string, string>(key, value));
		base.RowCount++;
	}
}

using System;
using System.Collections.Generic;
using System.Data;

namespace VideoOS.Common.Controls.Reporting;

public class StandardTable : ReportTableBase
{
	private List<string[]> _rows = new List<string[]>();

	public override DataTable DataTable
	{
		get
		{
			DataTable dataTable = base.DataTable;
			foreach (string[] row in _rows)
			{
				DataRowCollection rows = dataTable.Rows;
				object[] values = row;
				rows.Add(values);
			}
			return dataTable;
		}
	}

	public void AddRow(params string[] row)
	{
		if (row == null)
		{
			throw new ArgumentNullException("row");
		}
		if (base.ColumnCount == 0)
		{
			base.ColumnCount = row.Length;
		}
		else if (row.Length != base.ColumnCount)
		{
			throw new ArgumentException("Invalid number of parameters.");
		}
		_rows.Add(row);
		base.RowCount++;
	}

	public void RemoveLastRow()
	{
		_rows.RemoveAt(_rows.Count - 1);
		base.RowCount--;
	}
}

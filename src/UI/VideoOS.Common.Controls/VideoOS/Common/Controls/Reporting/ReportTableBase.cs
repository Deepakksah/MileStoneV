using System;
using System.Data;
using System.Globalization;

namespace VideoOS.Common.Controls.Reporting;

public abstract class ReportTableBase
{
	private string[] _headers;

	private int[] _columnWidths;

	public bool SuppressTableStyle { get; set; }

	public bool RepeatHeader { get; set; }

	public float HeaderAngle { get; set; }

	public int ColumnCount { get; protected set; }

	public int RowCount { get; protected set; }

	public virtual DataTable DataTable
	{
		get
		{
			DataTable dataTable = new DataTable();
			dataTable.Locale = CultureInfo.CurrentUICulture;
			if (_headers == null)
			{
				for (int i = 0; i < ColumnCount; i++)
				{
					dataTable.Columns.Add();
				}
			}
			else
			{
				string[] headers = _headers;
				foreach (string columnName in headers)
				{
					dataTable.Columns.Add(columnName);
				}
			}
			return dataTable;
		}
	}

	public string[] GetHeaders()
	{
		return _headers;
	}

	public void SetHeaders(params string[] headers)
	{
		if (headers != null && headers.Length != ColumnCount)
		{
			if (ColumnCount != 0)
			{
				throw new ArgumentException("Invalid number of columns.", "headers");
			}
			ColumnCount = headers.Length;
		}
		_headers = headers;
	}

	public int[] GetColumnWidths()
	{
		return _columnWidths;
	}

	public void SetColumnWidths(params int[] columnWidths)
	{
		if (columnWidths != null && columnWidths.Length != ColumnCount)
		{
			if (ColumnCount != 0)
			{
				throw new ArgumentException("Invalid number of columns.", "columnWidths");
			}
			ColumnCount = columnWidths.Length;
		}
		_columnWidths = columnWidths;
	}
}

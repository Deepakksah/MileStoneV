using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class DataGridViewPasswordTextBoxColumn : DataGridViewTextBoxColumn
{
	private const char DefaultPasswordChar = '*';

	private const bool DefaultUseSystemPasswordChar = true;

	private const string DefaultNullValueDisplayString = "";

	private char _passwordChar;

	private bool _useSystemPasswordChar;

	private string _nullValueDisplayString;

	[Category("Password")]
	[Description("Gets or sets the masking character for the text box.")]
	[DefaultValue('*')]
	public char PasswordChar
	{
		get
		{
			return _passwordChar;
		}
		set
		{
			if (_passwordChar == value)
			{
				return;
			}
			_passwordChar = value;
			if (CellTemplate is DataGridViewPasswordTextBoxCell dataGridViewPasswordTextBoxCell)
			{
				dataGridViewPasswordTextBoxCell.PasswordChar = value;
			}
			if (base.DataGridView == null)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)base.DataGridView.Rows)
			{
				if (item.Cells[base.Index] is DataGridViewPasswordTextBoxCell dataGridViewPasswordTextBoxCell2)
				{
					dataGridViewPasswordTextBoxCell2.PasswordChar = value;
				}
			}
			base.DataGridView.Refresh();
		}
	}

	[Category("Password")]
	[Description("Gets or sets a value indicating whether the operating system-supplied password character should be used. This property has precedence over PasswordChar.")]
	[DefaultValue(true)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return _useSystemPasswordChar;
		}
		set
		{
			if (_useSystemPasswordChar == value)
			{
				return;
			}
			_useSystemPasswordChar = value;
			if (CellTemplate is DataGridViewPasswordTextBoxCell dataGridViewPasswordTextBoxCell)
			{
				dataGridViewPasswordTextBoxCell.UseSystemPasswordChar = value;
			}
			if (base.DataGridView == null)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)base.DataGridView.Rows)
			{
				if (item.Cells[base.Index] is DataGridViewPasswordTextBoxCell dataGridViewPasswordTextBoxCell2)
				{
					dataGridViewPasswordTextBoxCell2.UseSystemPasswordChar = value;
				}
			}
			base.DataGridView.Refresh();
		}
	}

	[Category("Password")]
	[Description("The string displayed if cell's Value property is null and hasn't been edited by the user yet.")]
	[DefaultValue("")]
	public string NullValueDisplayString
	{
		get
		{
			return _nullValueDisplayString;
		}
		set
		{
			if (!(_nullValueDisplayString != value))
			{
				return;
			}
			_nullValueDisplayString = value;
			if (CellTemplate is DataGridViewPasswordTextBoxCell dataGridViewPasswordTextBoxCell)
			{
				dataGridViewPasswordTextBoxCell.NullValueDisplayString = value;
			}
			if (base.DataGridView == null)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)base.DataGridView.Rows)
			{
				if (item.Cells[base.Index] is DataGridViewPasswordTextBoxCell dataGridViewPasswordTextBoxCell2)
				{
					dataGridViewPasswordTextBoxCell2.NullValueDisplayString = value;
				}
			}
			base.DataGridView.Refresh();
		}
	}

	public override DataGridViewCell CellTemplate
	{
		get
		{
			return base.CellTemplate;
		}
		set
		{
			if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewPasswordTextBoxCell)))
			{
				throw new InvalidCastException("Must be a PasswordTextBoxCell");
			}
			base.CellTemplate = value;
		}
	}

	public DataGridViewPasswordTextBoxColumn()
	{
		CellTemplate = new DataGridViewPasswordTextBoxCell();
		PasswordChar = '*';
		UseSystemPasswordChar = true;
		NullValueDisplayString = "";
	}

	public override object Clone()
	{
		DataGridViewPasswordTextBoxColumn obj = base.Clone() as DataGridViewPasswordTextBoxColumn;
		obj.PasswordChar = _passwordChar;
		obj.UseSystemPasswordChar = _useSystemPasswordChar;
		obj.NullValueDisplayString = _nullValueDisplayString;
		return obj;
	}
}

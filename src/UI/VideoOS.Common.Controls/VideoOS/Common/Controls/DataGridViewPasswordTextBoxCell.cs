using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class DataGridViewPasswordTextBoxCell : DataGridViewTextBoxCell
{
	private static readonly char SystemPasswordCharacter = Convert.ToChar(9679);

	private const int FixedMaskCharacterLength = 8;

	private bool _editedByUser;

	private char _editingControlPasswordChar;

	private bool _editingControlUseSystemPasswordChar;

	public char PasswordChar { get; set; }

	public bool UseSystemPasswordChar { get; set; }

	public string NullValueDisplayString { get; set; }

	public DataGridViewPasswordTextBoxCell()
	{
		base.Style.NullValue = null;
	}

	public override object Clone()
	{
		DataGridViewPasswordTextBoxCell obj = base.Clone() as DataGridViewPasswordTextBoxCell;
		obj.PasswordChar = PasswordChar;
		obj.UseSystemPasswordChar = UseSystemPasswordChar;
		obj.NullValueDisplayString = NullValueDisplayString;
		return obj;
	}

	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		object result = base.GetFormattedValue(_editedByUser ? value : NullValueDisplayString, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
		if (ReadOnly || _editedByUser || value != null)
		{
			if (UseSystemPasswordChar)
			{
				result = new string(SystemPasswordCharacter, 8);
			}
			else if (PasswordChar != 0)
			{
				result = new string(PasswordChar, 8);
			}
		}
		return result;
	}

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		TextBox textBox = base.DataGridView.EditingControl as TextBox;
		_editingControlPasswordChar = textBox.PasswordChar;
		_editingControlUseSystemPasswordChar = textBox.UseSystemPasswordChar;
		textBox.PasswordChar = PasswordChar;
		textBox.UseSystemPasswordChar = UseSystemPasswordChar;
		base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		if (base.Value == null && !_editedByUser)
		{
			textBox.Clear();
			base.DataGridView.CellValueChanged += DataGridView_CellValueChanged;
		}
	}

	public override void DetachEditingControl()
	{
		base.DetachEditingControl();
		TextBox obj = base.DataGridView.EditingControl as TextBox;
		obj.PasswordChar = _editingControlPasswordChar;
		obj.UseSystemPasswordChar = _editingControlUseSystemPasswordChar;
		base.DataGridView.CellValueChanged -= DataGridView_CellValueChanged;
	}

	private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		_editedByUser = true;
	}
}

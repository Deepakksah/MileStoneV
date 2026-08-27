using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class DataGridViewComboAndImageCell : DataGridViewComboBoxCell
{
	private Image imageValue;

	private int imageOffset;

	private bool showAsLink;

	private Color saveColor;

	public bool ShowAsLink
	{
		get
		{
			return showAsLink;
		}
		set
		{
			if (showAsLink != value)
			{
				showAsLink = value;
				if (showAsLink)
				{
					Font font = base.InheritedStyle.Font;
					base.Style.Font = new Font(font, FontStyle.Underline);
					saveColor = base.Style.ForeColor;
					base.Style.ForeColor = SystemColors.HotTrack;
				}
				else
				{
					base.Style.Font = new Font(base.InheritedStyle.Font, FontStyle.Regular);
					base.Style.ForeColor = saveColor;
				}
			}
		}
	}

	public Image Image
	{
		get
		{
			if (imageValue != null || OwningTextAndImageColumn == null)
			{
				return imageValue;
			}
			return OwningTextAndImageColumn.Image;
		}
		set
		{
			if (imageValue != value)
			{
				imageValue = value;
				CalculatePadding();
			}
		}
	}

	public int ImageOffset
	{
		get
		{
			if (imageOffset > 0 || OwningTextAndImageColumn == null)
			{
				return imageOffset;
			}
			return OwningTextAndImageColumn.ImageOffset;
		}
		set
		{
			if (imageOffset != value)
			{
				imageOffset = value;
				CalculatePadding();
			}
		}
	}

	private DataGridViewComboAndImageColumn OwningTextAndImageColumn => base.OwningColumn as DataGridViewComboAndImageColumn;

	public event EventHandler<DataGridViewComboBoxEventArgs> SelectedIndexChanged;

	public event EventHandler<DataGridViewComboBoxEventArgs> SelectionChangeCommitted;

	public event EventHandler<DataGridViewComboBoxEventArgs> DropDown;

	public event EventHandler<DataGridViewComboBoxEventArgs> DropDownClosed;

	public override object Clone()
	{
		DataGridViewComboAndImageCell obj = base.Clone() as DataGridViewComboAndImageCell;
		obj.imageValue = imageValue;
		obj.imageOffset = imageOffset;
		return obj;
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		base.OnMouseEnter(rowIndex);
		if (showAsLink)
		{
			base.DataGridView.Cursor = Cursors.Hand;
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		base.OnMouseLeave(rowIndex);
		if (showAsLink)
		{
			base.DataGridView.Cursor = Cursors.Default;
		}
	}

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewComboBoxEditingControl obj = (DataGridViewComboBoxEditingControl)base.DataGridView.EditingControl;
		obj.DropDownStyle = ComboBoxStyle.DropDownList;
		obj.SelectedIndexChanged += cbo_SelectedIndexChanged;
		obj.SelectionChangeCommitted += cbo_SelectionChangeCommitted;
		obj.DropDown += cbo_DropDown;
		obj.DropDownClosed += cbo_DropDownClosed;
		obj.DataSource = DataSource;
		obj.DisplayMember = DisplayMember;
		obj.ValueMember = ValueMember;
	}

	public override void DetachEditingControl()
	{
		base.DetachEditingControl();
		DataGridViewComboBoxEditingControl obj = (DataGridViewComboBoxEditingControl)base.DataGridView.EditingControl;
		obj.SelectedIndexChanged -= cbo_SelectedIndexChanged;
		obj.SelectionChangeCommitted -= cbo_SelectionChangeCommitted;
		obj.DropDown -= cbo_DropDown;
		obj.DropDownClosed -= cbo_DropDownClosed;
	}

	private void cbo_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.SelectedIndexChanged != null)
		{
			DataGridViewComboBoxEditingControl dataGridViewComboBoxEditingControl = sender as DataGridViewComboBoxEditingControl;
			DataGridView editingControlDataGridView = dataGridViewComboBoxEditingControl.EditingControlDataGridView;
			this.SelectedIndexChanged(this, new DataGridViewComboBoxEventArgs(editingControlDataGridView.CurrentCell.ColumnIndex, editingControlDataGridView.CurrentCell.RowIndex, dataGridViewComboBoxEditingControl));
		}
	}

	private void cbo_SelectionChangeCommitted(object sender, EventArgs e)
	{
		if (this.SelectionChangeCommitted != null)
		{
			DataGridViewComboBoxEditingControl dataGridViewComboBoxEditingControl = sender as DataGridViewComboBoxEditingControl;
			DataGridView editingControlDataGridView = dataGridViewComboBoxEditingControl.EditingControlDataGridView;
			this.SelectionChangeCommitted(this, new DataGridViewComboBoxEventArgs(editingControlDataGridView.CurrentCell.ColumnIndex, editingControlDataGridView.CurrentCell.RowIndex, dataGridViewComboBoxEditingControl));
		}
	}

	private void cbo_DropDown(object sender, EventArgs e)
	{
		if (this.DropDown != null)
		{
			DataGridViewComboBoxEditingControl dataGridViewComboBoxEditingControl = sender as DataGridViewComboBoxEditingControl;
			DataGridView editingControlDataGridView = dataGridViewComboBoxEditingControl.EditingControlDataGridView;
			this.DropDown(this, new DataGridViewComboBoxEventArgs(editingControlDataGridView.CurrentCell.ColumnIndex, editingControlDataGridView.CurrentCell.RowIndex, dataGridViewComboBoxEditingControl));
		}
	}

	private void cbo_DropDownClosed(object sender, EventArgs e)
	{
		if (this.DropDownClosed != null)
		{
			DataGridViewComboBoxEditingControl dataGridViewComboBoxEditingControl = sender as DataGridViewComboBoxEditingControl;
			DataGridView editingControlDataGridView = dataGridViewComboBoxEditingControl.EditingControlDataGridView;
			this.DropDownClosed(this, new DataGridViewComboBoxEventArgs(editingControlDataGridView.CurrentCell.ColumnIndex, editingControlDataGridView.CurrentCell.RowIndex, dataGridViewComboBoxEditingControl));
		}
	}

	private void CalculatePadding()
	{
		Padding padding = base.InheritedStyle.Padding;
		base.Style.Padding = new Padding(Image.Size.Width + ImageOffset, padding.Top, padding.Right, padding.Bottom);
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
		if (Image != null)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			GraphicsContainer container = graphics.BeginContainer();
			graphics.SetClip(cellBounds);
			graphics.DrawImageUnscaled(Image, cellBounds.Location.X + ImageOffset, cellBounds.Location.Y + cellBounds.Height / 2 - Image.Height / 2);
			graphics.EndContainer(container);
		}
	}
}

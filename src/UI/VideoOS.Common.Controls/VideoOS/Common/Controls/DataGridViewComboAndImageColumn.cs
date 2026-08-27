using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class DataGridViewComboAndImageColumn : DataGridViewComboBoxColumn
{
	private Image imageValue;

	private int imageOffset;

	public Image Image
	{
		get
		{
			return imageValue;
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
			return imageOffset;
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

	public DataGridViewComboAndImageColumn()
	{
		CellTemplate = new DataGridViewComboAndImageCell();
	}

	public override object Clone()
	{
		DataGridViewComboAndImageColumn obj = base.Clone() as DataGridViewComboAndImageColumn;
		obj.imageValue = imageValue;
		obj.imageOffset = imageOffset;
		return obj;
	}

	private void CalculatePadding()
	{
		if (imageValue != null && InheritedStyle != null)
		{
			Padding padding = InheritedStyle.Padding;
			DefaultCellStyle.Padding = new Padding(imageValue.Size.Width + imageOffset, padding.Top, padding.Right, padding.Bottom);
		}
		else
		{
			DefaultCellStyle.Padding = InheritedStyle.Padding;
		}
	}
}
